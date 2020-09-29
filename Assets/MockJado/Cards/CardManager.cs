using ElJardin;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElJardin {
    public class CardManager : Singleton<CardManager> {
        [SerializeField]
        public Queue<CardData> cardQueue;
        [SerializeField]
        public List<CardData> cardList;
        public List<Card> handList;
        public List<Transform> transformList;
        public int maxHand;
        public Card cardPrefab;

        private void Awake() {
            maxHand = 5;
            handList = new List<Card>();
            cardQueue = new Queue<CardData>(cardList);
        }

        private void Start() {
            firstDrawCard();
        }

        private void firstDrawCard() {
            int index = 0;
            while (index < maxHand - 1) {
                Card tmpCard;
                tmpCard = Instantiate(cardPrefab, transformList[index]);
                handList.Add(tmpCard);
                handList[index].cardData = cardQueue.Dequeue();
                handList[index].loadCardData();
                handList[index].changeCardTransform(index);
                index++;
            }
        }

        public void moveCards(int currentIndex) {
            Card tmpCard = handList.Find(c => c.transformIndex == currentIndex);
            handList.ForEach(c => {
                if (c.transformIndex > currentIndex) {
                    c.changeCardTransform(c.transformIndex - 1);
                }
            });

            tmpCard.changeCardTransform(maxHand - 1);
        }

        public void drawNextCard() {
            Debug.LogWarning("QUE ROBO EH" + cardQueue.Count);
            if (cardQueue.Count > 0) {
                int lastHandIndex = maxHand - 1;
                Card tmpCard = handList.Find(c => !c.gameObject.activeSelf);
                if(tmpCard == null) {
                    tmpCard = Instantiate(cardPrefab, transformList[lastHandIndex]);
                    handList.Add(tmpCard);
                }
                Debug.LogWarning("La cartica eh, " + tmpCard);
                tmpCard.cardData = cardQueue.Dequeue();
                tmpCard.loadCardData();
                tmpCard.changeCardTransform(lastHandIndex);
                tmpCard.gameObject.SetActive(true);
            }
        }
    }
}
