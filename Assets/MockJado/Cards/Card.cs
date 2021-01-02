using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace ElJardin {
    public class Card : MonoBehaviour {
        private CardData cardData;
        public TextMeshProUGUI amountText;
        public int transformIndex;
        public bool usedCard;

        public CardData CardData { get => cardData; set => cardData = value; }

        private void Awake() {
            usedCard = false;
        }

        private void Start() {
            loadCardData();
        }

        public void loadCardData() {
            amountText.text = CardData.amount.ToString();
            GetComponent<ItemDragHandler>().LoadCardData(CardData);
            GetComponentInChildren<OutlineController>().Activate(false);
        }
        
        public IEnumerator changeCardTransform(int newIndex, bool wait = true) {
            float animTime=.5f;
            transformIndex = newIndex;
            float waitTime = wait ? transformIndex * .5f : 0f;
            yield return new WaitForSeconds(waitTime);
            jumpCard(animTime);
            yield return new WaitForSeconds(animTime);

            transform.SetParent(CardManager.Instance.transformList[transformIndex]);
        }

        private void jumpCard(float animTime) {
            if (gameObject.activeSelf) {
                AkSoundEngine.PostEvent("Carta_Slide_In", gameObject);
            }
            transform.DOJump(CardManager.Instance.transformList[transformIndex].position, 3, 1, animTime);

            GetComponent<ItemDragHandler>().originalHandPosition = CardManager.Instance.transformList[transformIndex].position;
        }
    }
}