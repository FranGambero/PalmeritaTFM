using System;
using DG.Tweening;
using ElJardin;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler {
    public Vector3 originalPosition;
    public Vector3 originalHandPosition;
    public bool starting, hoving;
    public LayerMask layerMask;
    private CardData cardData;
    private float upOffset = 7f;

    Card ActionCard => GetComponent<Card>();


    private void Start() {
        starting = false;
        originalPosition = transform.position;
        hoving = false;
        GameManager.Instance.Sepalo.OnEndWalk.AddListener(HoverAround);
    }
    
    public void LoadCardData(CardData cardData) {
        this.cardData = cardData;
        originalHandPosition = CardManager.Instance.transformList[GetComponent<Card>().transformIndex].position;
        transform.GetChild(0).GetComponent<Image>().sprite = cardData.sprite;

        switch (cardData.cardType) {
            case CardData.CardType.Undefined:
            case CardData.CardType.Construction:
                GetComponent<Image>().color = Color.green;
                // Provisional
                transform.GetChild(1).gameObject.SetActive(true);
                transform.GetChild(2).gameObject.SetActive(true);
                break;
            case CardData.CardType.Summon:
                GetComponent<Image>().color = Color.yellow;
                // Provisional
                transform.GetChild(1).gameObject.SetActive(true);
                transform.GetChild(2).gameObject.SetActive(false);
                break;
            case CardData.CardType.Buff:
                GetComponent<Image>().color = Color.gray;
                // Provisional
                transform.GetChild(1).gameObject.SetActive(false);
                transform.GetChild(2).gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }


    public void OnDrag(PointerEventData eventData) {
        Debug.Log("Starting Drag");
        if (GameManager.Instance.Sepalo.IsMyTurn) {
            if (!starting) {
                starting = true;
                //GameManager.Instance.DraggingCard = true;
                GameManager.Instance.selectedCard = ActionCard;
                AkSoundEngine.PostEvent("Carta_Select_In", gameObject);
                HoverAround();
            }
            
            transform.position = Input.mousePosition;
            BuildManager.Instance.changeBuildValues(cardData.amount);
        }
    }
    public void OnEndDrag(PointerEventData eventData) {
        //GameManager.Instance.DraggingCard = false;
        GameManager.Instance.selectedCard = null;
        DoTheAction();
        starting = false;
        hoving = false;
    }
    
    public void HoverAround() {
        if (!GameManager.Instance.Sepalo.isMoving && !hoving && starting) {
            hoving = true;
            BuildManager.Instance.HoverAroundNode(cardData.amount);
        }
    }
    
    /*
     * Se llama solamente al usar la carta, hay que pasarlo a BuildRiver ICardAction pero esta jodido
     */
    private void DoTheAction() {
        Debug.Log("Doing the action");
        if (GameManager.Instance.Sepalo.IsMyTurn && starting) {
            if (buildNewChannel() && !GameManager.Instance.Sepalo.isMoving) {
                transform.position = originalPosition;
                //Aqui va el sonido de colocar carta
                //AkSoundEngine.PostEvent("Carta_Posicion_In", gameObject);
                ///
                HideCard();
                GameManager.Instance.Sepalo.onTurnFinished();
            } else {
                BuildManager.Instance.StopHoverCoroutine();
                transform.position = originalHandPosition;
            }
            BuildManager.Instance.UnHoverNodesInList();
        }
    }

    private IEnumerator Wait() {
        yield return new WaitForSeconds(.1f);
    }

    private bool buildNewChannel() {
        bool hasBuild = BuildManager.Instance.ChangeNodesInList();
        //  if (hasBuild)
        //MapManager.Instance.CheckFullRiver();
        return hasBuild;
    }

    private void HideCard() {
        // CardManager.Instance.moveCards(GetComponent<Card>().transformIndex);
        gameObject.SetActive(false);
        CardManager.Instance.LastIndexUsed = GetComponent<Card>().transformIndex;
    }

    public void OnMouseHoverEnter() {
        if (GameManager.Instance.Sepalo.IsMyTurn && !starting) {
            transform.DOMove(originalHandPosition + Vector3.up * upOffset, .2f).SetEase(Ease.Linear);
            transform.parent.GetComponentInChildren<OutlineController>().Activate(true);
        }
    }

    public void OnMouseHoverExit() {
        if (GameManager.Instance.Sepalo.IsMyTurn && !starting && gameObject.activeSelf) {
            transform.DOMove(originalHandPosition, .5f);
            transform.parent.GetComponentInChildren<OutlineController>().Activate(false);
        }
    }
}
