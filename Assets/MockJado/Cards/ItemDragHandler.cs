using ElJardin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler {
    public Vector3 originalPosition;
    public Vector3 originalHandPosition;
    public bool starting;
    public LayerMask layerMask;
    private CardData cardData;

    private void Start() {
        starting = false;
        originalPosition = transform.position;
        cardData = GetComponent<Card>().cardData;

        GameManager.Instance.Sepalo.OnEndWalk.AddListener(DoTheAction);
    }

    public void OnDrag(PointerEventData eventData) {
        if (GameManager.Instance.Sepalo.IsMyTurn) {
            if (!starting) {
                starting = true;
                GameManager.Instance.draggingCard = true;
                BuildManager.Instance.HoverAroundNode(GameManager.Instance.Sepalo.CurrentNode, cardData.amount);
                originalHandPosition = transform.position;
            }
            transform.position = Input.mousePosition;
            BuildManager.Instance.changeBuildValues(
                cardData.amount, cardData.direction);
        }
    }
    public void OnEndDrag(PointerEventData eventData) {
        starting = false;
        GameManager.Instance.draggingCard = false;
        DoTheAction();
    }

    private void DoTheAction() {
        if (GameManager.Instance.Sepalo.IsMyTurn && starting && !GameManager.Instance.Sepalo.isMoving) {
            if (buildNewChannel()) {
                transform.position = originalPosition;
                AkSoundEngine.PostEvent("Carta_Select_In", gameObject);
                GameManager.Instance.Sepalo.onTurnFinished();
                hideCard();
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

    private void hideCard() {
        CardManager.Instance.moveCards(GetComponent<Card>().transformIndex);
        gameObject.SetActive(false);
    }

}
