using DG.Tweening;
using ElJardin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler {
    public Vector3 originalPosition;
    public Vector3 originalHandPosition;
    public bool starting, hoving;
    public LayerMask layerMask;
    private CardData cardData;
    private float upOffset = 7f;


    private void Start() {
        starting = false;
        originalPosition = transform.position;
        hoving = false;
        GameManager.Instance.Sepalo.OnEndWalk.AddListener(HoverAround);
    }
    public void LoadCardData(CardData cardData) {
        this.cardData = cardData;
        originalHandPosition = CardManager.Instance.transformList[GetComponent<Card>().transformIndex].position;

    }
    public void OnDrag(PointerEventData eventData) {
        if (GameManager.Instance.Sepalo.IsMyTurn) {
            if (!starting) {
                starting = true;
                GameManager.Instance.draggingCard = true;
                AkSoundEngine.PostEvent("Carta_Select_In", gameObject);
                HoverAround();
            }
            transform.position = Input.mousePosition;
            BuildManager.Instance.changeBuildValues(
                cardData.amount, cardData.direction);
        }
    }
    public void OnEndDrag(PointerEventData eventData) {
        GameManager.Instance.draggingCard = false;
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
    private void DoTheAction() {
        if (GameManager.Instance.Sepalo.IsMyTurn && starting) {
            if (buildNewChannel() && !GameManager.Instance.Sepalo.isMoving) {
                transform.position = originalPosition;
                //AkSoundEngine.PostEvent("Carta_Select_In", gameObject);
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
        }
    }

    public void OnMouseHoverExit() {
        if (GameManager.Instance.Sepalo.IsMyTurn && !starting && gameObject.activeSelf) {
            transform.DOMove(originalHandPosition, .5f);
        }
    }
}
