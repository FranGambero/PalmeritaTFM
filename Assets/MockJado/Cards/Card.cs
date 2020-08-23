using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElJardin {
    public class Card : Singleton<Card> {
        public int amount;
        public DirectionType direction;

        public void generateCard() {
            // Ahora mismo los datos son randoms, esto se cambiará a datos fijos de la baraja y el set se guardará en un orden concreto
            // Podemos usar JSON o XML para guardarlas y leerlas facilmente?
            // Se cambiará a PickCard
            Debug.Log("Me han llamao jojo");
            amount = Random.Range(1, 6);
            direction = getNewDirection();
        }

        public void pickCard() {
            // To Be Done
        }

        public DirectionType getNewDirection() {
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