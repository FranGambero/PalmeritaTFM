using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ElJardin {
    public class Card : MonoBehaviour {
        public int amount;
        public DirectionType direction;
        public TextMeshProUGUI amountText;

        private void Start() {
            loadCardData();
        }

        private void loadCardData() {
            amountText.text = amount.ToString();
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