using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElJardin {
    public class DryController : MonoBehaviour, ITurn {
        public int numTurns = 3;
        public bool secadoDelTo;
        public int turnIndex { get { return _turnIndex; } set { _turnIndex = value; } }
        public int _turnIndex;

        public void onTurnStart(int currentIndex) {
            if (turnIndex == currentIndex) {
                checkDry();
            }
        }

        public void initDry(int newIndex) {
            //List<ITurn> listaTurnosActuales = Semaphore.Instance.turnBasedElementList;
            //turnIndex = listaTurnosActuales[listaTurnosActuales.Count].turnIndex + 1;
            secadoDelTo = false;
            turnIndex = newIndex;
        }

        private void checkDry() {
            if (numTurns > 0 && !secadoDelTo) {
                numTurns--;
                Debug.Log("Me quedan " + numTurns + " turnos");
                switch (numTurns) {
                    case 2:
                        GetComponentInChildren<MeshRenderer>().material = MapManager.Instance.peligro1;
                        break;
                    case 1:
                        GetComponentInChildren<MeshRenderer>().material = MapManager.Instance.peligro2;
                        break;
                    case 0:
                        GetComponentInChildren<MeshRenderer>().material = MapManager.Instance.peligro3;
                        break;
                    default:
                        break;
                }
            } else {
                //Lo destruye
                secadoDelTo = true;
            }

            onTurnFinished();
        }

        public void onTurnFinished() {
            Semaphore.Instance.onTurnEnd(turnIndex);
            if (secadoDelTo) {
                Semaphore.Instance.RemoveTurn(turnIndex);
                // TODO, vuelve a poner como suelo normal :3  y quitar el dry, sera el Node posiblemete
                gameObject.GetComponent<Node>().RemoveDryComponent();
                Debug.LogWarning("Quiero ser una casilla de verdad!");
            }
        }

        private void OnDestroy() {
            secadoDelTo = true;
        }
    }
}