using ElJardin;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ElJardin {
    public class CardManager : Singleton<CardManager> {
        public Card cardPrefab;
        [SerializeField]
        public Queue<CardData> cardQueue;
        [SerializeField]
        public List<CardData> cardList;
        public List<Card> handList;
        public List<Transform> transformList;
        public int maxHand;
        private int totalCards;

        public TextMeshProUGUI cardsLeftLabel;
        public Image indicatorCardImage;
        public Gradient gradient;

        private void Awake() {
            maxHand = 5;
            handList = new List<Card>();
            cardQueue = new Queue<CardData>(cardList);
            totalCards = cardQueue.Count;
        }

        private void Start() {
            firstDrawCard();
        }

        private void firstDrawCard() {
            int index = 0;
            while (index < maxHand ) {
                Card tmpCard;
                tmpCard = Instantiate(cardPrefab, transformList[index]);
                handList.Add(tmpCard);
                handList[index].cardData = cardQueue.Dequeue();
                handList[index].loadCardData();
                moveCardToDeck(handList[index]);

                handList[index].changeCardTransform(index);
                index++;
            }

            refreshLabels();
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
            bool canDraw = handList.FindAll(c => c.gameObject.activeSelf).Count < maxHand;
            if (cardQueue.Count > 0 && canDraw) {
                int lastHandIndex = maxHand - 1;
                Card tmpCard = handList.Find(c => !c.gameObject.activeSelf);

                if(tmpCard == null) {
                    tmpCard = Instantiate(cardPrefab, transformList[lastHandIndex]);
                    handList.Add(tmpCard);
                }

                tmpCard.cardData = cardQueue.Dequeue();
                tmpCard.loadCardData();
                moveCardToDeck(tmpCard);
                tmpCard.gameObject.SetActive(true);
                tmpCard.changeCardTransform(lastHandIndex, false);

                refreshLabels();
            }
        }

        private void moveCardToDeck(Card cardToMove) {
            cardToMove.transform.position = indicatorCardImage.transform.position;

        }

        private void refreshLabels() {
            float newValue = (float)cardQueue.Count / (float)totalCards;
            indicatorCardImage.color = gradient.Evaluate(1 - newValue);
            cardsLeftLabel.text = cardQueue.Count.ToString();
        }
    }
}
