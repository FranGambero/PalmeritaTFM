using ElJardin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour {
    private int totalNumCards;
    private int currentNumCards;
    private int handNumCards;

    public GameObject cardPrefab;
    public List<GameObject> cardList;
    public List<Transform> cardPosList;

    private void Awake() {
        cardList = new List<GameObject>();
    }

    private void Start() {
        spawnCards();
    }

    private void spawnCards() {
        for (int i = 0; i < 5; i++) {
            GameObject newCard = Instantiate(cardPrefab, cardPosList[i].position, cardPrefab.transform.rotation);
            newCard.GetComponent<Card>().generateCard();
            newCard.transform.SetParent(cardPosList[i]);
        }
    }
}
