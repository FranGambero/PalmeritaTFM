using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        public void changeCardTransform(int newIndex) {
            transformIndex = newIndex;
            this.transform.position = CardManager.Instance.transformList[transformIndex].position;
            transform.SetParent(CardManager.Instance.transformList[transformIndex]);
        }

        private DirectionType getNewDirection() {
            int numResult = Random.Range(0, 4);
            DirectionType newDirection;

            switch (numResult) {
                case 0:
                    newDirection = DirectionType.North;
                    break;
                case 1:
                    newDirection = DirectionType.East;
                    break;
                case 2:
                    newDirection = DirectionType.South;
                    break;
                case 3:
                    newDirection = DirectionType.West;
                    break;
                default:
                    newDirection = DirectionType.North;
                    break;
            }
            return newDirection;
        }

    }
}