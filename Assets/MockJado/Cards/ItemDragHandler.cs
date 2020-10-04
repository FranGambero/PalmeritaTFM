using ElJardin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler {
    public Vector3 originalPosition;
    public LayerMask layerMask;
    private CardData cardData;

    private void Start() {
        originalPosition = transform.position;
        cardData = GetComponent<Card>().cardData;
    }

    public void OnDrag(PointerEventData eventData) {
        if (GameManager.Instance.Sepalo.IsMyTurn) {
            transform.position = Input.mousePosition;
            BuildManager.Instance.changeBuildValues(
                cardData.amount, cardData.direction);
        }
    }
    public void OnEndDrag(PointerEventData eventData) {
        if (GameManager.Instance.Sepalo.IsMyTurn) {
            transform.position = originalPosition;
            hideCard();
            buildNewChannel();
        }
    }

    private void buildNewChannel() {
        BuildManager.Instance.ChangeNodesInList();
        MapManager.Instance.CheckFullRiver();
    }

    private void hideCard() {
        CardManager.Instance.moveCards(GetComponent<Card>().transformIndex);
        gameObject.SetActive(false);
    }

}
