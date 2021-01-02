using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElJardin {
    public class Flower : MonoBehaviour, ITurn {
        public bool flowerOpened;
        public List<DoGrow> listaElementosCrecedores;
        public Node nodito;
        public List<Node> listaNoditos;
        public bool _turneable = true;
        public int turnIndex { get { return _turnIndex; } set { _turnIndex = value; } }

        public bool turneable { get => _turneable; set => _turneable = true; }

        public int _turnIndex;

        private void Awake() {
            flowerOpened = false;
            listaNoditos = new List<Node>();
        }

        private void Start() {
            if (nodito != null)
                getNodeNeighbors();
        }

        [ContextMenu("CRECE CARLA")]
        public void activateFlor() {
            flowerOpened = true;
            GetComponentInChildren<LotoAnimatorController>().LotoOpen();
            listaElementosCrecedores.ForEach(e => e.RandomGrow());
        }

        [ContextMenu("DESCRECE CARLA")]
        public void deActivateFlor() {
            flowerOpened = false;
            // Hace la animación un poco regulinchi
            GetComponentInChildren<LotoAnimatorController>().LotoClose();
            listaElementosCrecedores.ForEach(e => e.Shrink());
        }

        public void getNodeNeighbors() {
            listaNoditos = nodito.GetListNeighbors();
            foreach (var item in listaNoditos) {
                Debug.Log("NODITO " + item.name);
            }
        }

        private bool CheckWaterAround() {
            return listaNoditos.Find(e => e.water.isGonnaHaveDaWote || e.water.hasWater);
        }

        public void onTurnStart(int currentIndex) {
            if (currentIndex == turnIndex) {
                if (CheckWaterAround() && !flowerOpened) {
                    activateFlor();
                } else if (!CheckWaterAround() && flowerOpened) {
                    deActivateFlor();
                }
            }

            onTurnFinished();
        }

        public void onTurnFinished() {
            Semaphore.Instance.onTurnEnd(turnIndex);
        }
    }
}
