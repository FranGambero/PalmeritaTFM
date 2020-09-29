using ElJardin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler {
    public Vector3 ogV;
    public LayerMask layerMask;
    //public GameObject cartLinked;
    private CardData cardData;

    private void Start() {
        ogV = transform.position;
        //cardData = cartLinked.transform.GetChild(0).GetComponent<Card>();
        cardData = GetComponent<Card>().cardData;
    }

    public void OnDrag(PointerEventData eventData) {
        //if (GameManager.Instance.myCharacterController.turnIndex == Semaphore.Instance.currentTurn) {
        if (GameManager.Instance.myCharacterController.isMyTurn) {
            transform.position = Input.mousePosition;
            BuildManager.Instance.changeBuildValues(
                cardData.amount, cardData.direction);
        }
    }
    public void OnEndDrag(PointerEventData eventData) {
        if (GameManager.Instance.myCharacterController.isMyTurn) {
            transform.position = ogV;
            buildNewChannel();
            hideCard();
        }
    }

    private void buildNewChannel() {
        BuildManager.Instance.ChangeNodesInList();
        MapManager.Instance.CheckFullRiver();
    }

    private void hideCard() {
        Debug.Log("IO ERA: " + GetComponent<Card>().transformIndex);
        CardManager.Instance.moveCards(GetComponent<Card>().transformIndex);
        gameObject.SetActive(false);
    }

}
