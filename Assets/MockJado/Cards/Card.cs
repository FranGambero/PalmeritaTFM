using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace ElJardin {
    public class Card : MonoBehaviour {
        public CardData cardData;
        public TextMeshProUGUI amountText;
        public int transformIndex;
        public bool usedCard;

        private void Awake() {
            usedCard = false;
        }

        private void Start() {
            loadCardData();
        }

        public void loadCardData() {
            amountText.text = cardData.amount.ToString();

        }

        public void changeCardTransform(int newIndex, bool wait = true) {
            transformIndex = newIndex;
            float waitTime = wait ? transformIndex * .5f : 0f;
            Invoke(nameof(jumpCard), waitTime);

            transform.SetParent(CardManager.Instance.transformList[transformIndex]);
        }

        private void jumpCard() {
            //if (transformIndex != CardManager.Instance.maxHand - 1) {
                AkSoundEngine.PostEvent("Carta_Slide_In", gameObject);
            //}
            transform.DOJump(CardManager.Instance.transformList[transformIndex].position, 3, 1, .5f);
        }
    }
}