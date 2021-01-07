using System;
using DG.Tweening;
using ElJardin;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public Vector3 originalPosition;
    public Vector3 originalHandPosition;
    public bool starting, hoving;
    public LayerMask layerMask;
    private CardData cardData;
    private float upOffset = 7f;

    public Image cardSprite;

    Card ActionCard => GetComponent<Card>();
    void EndTurn() => GameManager.Instance.Sepalo.onTurnFinished();


    private void Start()
    {
        starting = false;
        originalPosition = transform.position;
        hoving = false;
        GameManager.Instance.Sepalo.OnEndWalk.AddListener(HoverAroundNode);
    }

    #region Load
    public void LoadCardData(CardData cardData)
    {
        this.cardData = cardData;
        originalHandPosition = CardManager.Instance.transformList[GetComponent<Card>().transformIndex].position;
        cardSprite.sprite = cardData.sprite;

        switch(cardData.cardType)
        {
            case CardData.CardType.Undefined:
            case CardData.CardType.Construction:
                GetComponent<Image>().color = Color.green;
                // Provisional
                transform.GetChild(1).gameObject.SetActive(true);
                break;
            case CardData.CardType.Summon:
                GetComponent<Image>().color = Color.yellow;
                // Provisional
                transform.GetChild(1).gameObject.SetActive(true);
                break;
            case CardData.CardType.Buff:
                GetComponent<Image>().color = Color.gray;
                // Provisional
                transform.GetChild(1).gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }
    #endregion

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Starting Drag");
        if(GameManager.Instance.Sepalo.IsMyTurn)
        {
            if(!starting)
            {
                starting = true;

                GameManager.Instance.selectedCard = ActionCard;
                AkSoundEngine.PostEvent("Carta_Select_In", gameObject);
                HoverAroundNode();
            }

            transform.position = Input.mousePosition;
            //BuildManager.Instance.changeBuildValues(cardData.amount);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameManager.Instance.selectedCard = null;
        
        DoTheAction();
        starting = false;
        hoving = false;
    }

    #region Hover Around Node
    void HoverAroundNode()
    {
        if(!GameManager.Instance.Sepalo.isMoving && !hoving && starting)
        {
            hoving = true;
            ActionCard.HoverOnGrab();
            //BuildManager.Instance.HoverAroundNode(cardData.amount);
        }
    }
    #endregion

    #region CardAction
    /*
     * Se llama solamente al usar la carta, hay que pasarlo a BuildRiver ICardAction pero esta jodido
     */
    void DoTheAction()
    {
        if(CanUseCardAction())
        {
            var mouseNode = GameManager.Instance.SelectedNode;
            
            ActionCard.OnActionCompleted.RemoveAllListeners();
            ActionCard.OnActionCompleted.AddListener(EndCardActions);
            ActionCard.Action(mouseNode);
            TurnsCounter.Instance.OnCardUsed();
        }
    }

    void EndCardActions(bool actionCompleted)
    {
        if( actionCompleted && !GameManager.Instance.Sepalo.isMoving)
        {
            //Aqui va el sonido de colocar carta
            //AkSoundEngine.PostEvent("Carta_Posicion_In", gameObject);
            ResetCardPosition();
            HideCard();
                
            EndTurn();
        }
        else
        {
            BuildManager.Instance.StopHoverCoroutine();
            ResetCardPosition();
        }
        ActionCard.UnHover();
        BuildManager.Instance.UnHoverNodesInList();
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(.1f);
    }

    bool CanUseCardAction()
    {
        return GameManager.Instance.Sepalo.IsMyTurn && starting;
    }
    private bool buildNewChannel()
    {
        bool hasBuild = BuildManager.Instance.ChangeNodesInList();
        return hasBuild;
    }
    #endregion
    
    #region CardVisuals
    private void HideCard()
    {
        // CardManager.Instance.moveCards(GetComponent<Card>().transformIndex);
        gameObject.SetActive(false);
        CardManager.Instance.LastIndexUsed = GetComponent<Card>().transformIndex;
    }

    public void OnMouseHoverEnter()
    {
        if(GameManager.Instance.Sepalo.IsMyTurn && !starting)
        {
            transform.DOMove(originalHandPosition + Vector3.up * upOffset, .2f).SetEase(Ease.Linear);
            transform.parent.GetComponentInChildren<OutlineController>().Activate(true);
        }
    }

    public void OnMouseHoverExit()
    {
        if(GameManager.Instance.Sepalo.IsMyTurn && !starting && gameObject.activeSelf)
        {
            transform.DOMove(originalHandPosition, .5f);
            transform.parent.GetComponentInChildren<OutlineController>().Activate(false);
        }
    }

    void ResetCardPosition()
    {
        transform.position = originalHandPosition;
    }
    #endregion
}