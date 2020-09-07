using ElJardin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler {
    public Vector3 ogV;
    public LayerMask layerMask;
    public GameObject cartLinked;
    private Card cardData;

    private void Start() {
        ogV = transform.position;
        cardData = cartLinked.transform.GetChild(0).GetComponent<Card>();
    }

    public void OnDrag(PointerEventData eventData) {
        transform.position = Input.mousePosition;
        BuildManager.Instance.changeBuildValues(
            cardData.amount, cardData.direction);
        //BuildManager.Instance.changeBuildValues(
        //    cartLinked.transform.GetChild(0).GetComponent<Card>().amount,
        //    cartLinked.transform.GetChild(0).GetComponent<Card>().direction);
    }
    public void OnEndDrag(PointerEventData eventData) {
        transform.position = ogV;
        buildNewChannel();
        callData();
    }

    private void buildNewChannel() {
        BuildManager.Instance.ChangeNodesInList();
        MapManager.Instance.CheckFullRiver();
    }

    private void callData() {
        if (cartLinked) {
            Debug.LogWarning("TIENES AHI " + cartLinked.transform.GetChild(0).GetComponent<Card>().amount);
        }
    }

}
