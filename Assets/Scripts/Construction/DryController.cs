using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElJardin {
    public class DryController : MonoBehaviour, ITurn {
        public int numTurns = 3;
        private int currTurns;
        public bool secadoDelTo;
        public int turnIndex { get { return _turnIndex; } set { _turnIndex = value; } }

        public bool turneable { get => _turneable; set => _turneable = value; }

        public int _turnIndex;
        public bool _turneable = false;
        public bool active;

        public void onTurnStart(int currentIndex) {
            if (turnIndex == currentIndex) {
                checkDry();
            }
        }

        public void initDry(int newIndex) {
            //List<ITurn> listaTurnosActuales = Semaphore.Instance.turnBasedElementList;
            //turnIndex = listaTurnosActuales[listaTurnosActuales.Count].turnIndex + 1;
            if (!active) {
                active = true;
                turneable = true;
                secadoDelTo = false;
                turnIndex = newIndex;
                currTurns = numTurns;
                Semaphore.Instance.AddTurn(this);
            }
        }
        public void StopDry(bool resetGround) {
            if (active) {
                active = false;
                turneable = false;
                secadoDelTo = false;
                Semaphore.Instance.RemoveTurn(this);
                gameObject.GetComponent<Node>().ResetMat();
                Debug.LogWarning("Quiero ser una casilla de verdad!");
                if (resetGround) {
                   BuildManager.Instance?.BuildGround(GetComponent<Node>());
                }
                else
                    VFXDirector.Instance.Play("OnStopDryGround", transform.position);
            }
        }

        private void checkDry() {
            if (currTurns > 0) {
                VFXDirector.Instance.Play("OnDryGround", transform.position);
                currTurns--;

                switch (currTurns) {
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
            if (GetComponent<Node>().water.IsActive() /*||
             BuildManager.Instance.CheckWaterAndActue(GetComponent<Node>())*/) {

                StopDry(false);

            } else if (secadoDelTo) {

                // TODO, vuelve a poner como suelo normal :3  y quitar el dry, sera el Node posiblemete
                StopDry(true);
            }
        }

        private void OnDestroy() {
            //secadoDelTo = true;
            Semaphore.Instance.onTurnStart -= onTurnStart;
        }


    }
}