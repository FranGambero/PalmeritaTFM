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
            while (index < maxHand - 1) {
                Card tmpCard;
                tmpCard = Instantiate(cardPrefab, transformList[index]);
                handList.Add(tmpCard);
                handList[index].cardData = cardQueue.Dequeue();
                handList[index].loadCardData();
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
            if (cardQueue.Count > 0) {
                int lastHandIndex = maxHand - 1;
                Card tmpCard = handList.Find(c => !c.gameObject.activeSelf);

                if(tmpCard == null) {
                    tmpCard = Instantiate(cardPrefab, transformList[lastHandIndex]);
                    handList.Add(tmpCard);
                }

                tmpCard.cardData = cardQueue.Dequeue();
                tmpCard.loadCardData();
                tmpCard.changeCardTransform(lastHandIndex);
                tmpCard.gameObject.SetActive(true);

                refreshLabels();
            }
        }

        private void refreshLabels() {
            float newValue = (float)cardQueue.Count / (float)totalCards;
            indicatorCardImage.color = gradient.Evaluate(1 - newValue);
            cardsLeftLabel.text = cardQueue.Count.ToString();
        }
    }
}
