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

        private int lastIndexUsed = -1;

        public int LastIndexUsed { get => lastIndexUsed; set => lastIndexUsed = value; }

        private void Init() {
            maxHand = 5;
            handList = new List<Card>();
            cardQueue = new Queue<CardData>(cardList);
            totalCards = cardQueue.Count;
        }


        public void firstDrawCard() {
            int index = 0;

            MenuDirector.Instance.ActivateCardCanvas(true);
            Init();
            while (index < cardList.Count) {
                Card tmpCard;
                tmpCard = Instantiate(cardPrefab, transformList[index]);
                handList.Add(tmpCard);
                handList[index].CardData = cardQueue.Dequeue();
                handList[index].loadCardData();
                moveCardToDeck(handList[index]);
                StartCoroutine(handList[index].changeCardTransform(index));
                index++;
            }

            refreshLabels();
        }

        public void moveCards(int currentIndex) {
            Card tmpCard = handList.Find(c => c.transformIndex == currentIndex);
            handList.ForEach(c => {
                if (c.transformIndex > currentIndex) {
                    StartCoroutine(c.changeCardTransform(c.transformIndex - 1));
                }
            });
            //Lo movemos a ultima posicion
            StartCoroutine(tmpCard.changeCardTransform(maxHand - 1));
        }

        public void DrawNextCard() {
            if(this.gameObject.activeInHierarchy)
                StartCoroutine(DrawNextCardCoroutine());
        }
        private IEnumerator DrawNextCardCoroutine() {
            yield return new WaitForSeconds(.1f);
            bool canDraw = handList.FindAll(c => c.gameObject.activeSelf).Count < maxHand;
            Debug.Log("Cartas posibles: " + canDraw);
            if (cardQueue.Count > 0 && canDraw) {
                int lastHandIndex = maxHand - 1;
                Card tmpCard = handList.Find(c => !c.gameObject.activeSelf);
                if (tmpCard == null) {
                    tmpCard = Instantiate(cardPrefab, transformList[lastHandIndex]);
                    handList.Add(tmpCard);
                }
                tmpCard.StopAllCoroutines();

                tmpCard.CardData = cardQueue.Dequeue();
                tmpCard.loadCardData();
                moveCardToDeck(tmpCard);
                //AkSoundEngine.PostEvent("Carta_Slide_In", gameObject);

                tmpCard.gameObject.SetActive(true);
                // tmpCard.changeCardTransform(lastHandIndex, true);

                refreshLabels();
            }
            if (LastIndexUsed >= 0)
                moveCards(LastIndexUsed);

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
