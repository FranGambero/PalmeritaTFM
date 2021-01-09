using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ElJardin {
    public class Flower : MonoBehaviour, ITurn {
        public bool flowerOpened;
        public List<DoGrow> listaElementosCrecedores;
        public Node nodito;
        public List<Node> listaNoditos;
        public bool _turneable = true;
        public LayerMask groundLayer;

        public int turnIndex { get { return _turnIndex; } set { _turnIndex = value; } }

        public bool turneable { get => _turneable; set => _turneable = true; }

        public int _turnIndex;

        private void Awake() {
            flowerOpened = false;
            listaNoditos = new List<Node>();
        }

        private void Start() {
            if (!nodito) {
                CheckGrownd();
            }
            if (nodito)
                getNodeNeighbors();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.D)) {
                AkSoundEngine.SetRTPCValue(Keys.WWise.RTPC_Flor, 25);

            }

            if (Input.GetKeyDown(KeyCode.C)) {
                AkSoundEngine.SetRTPCValue(Keys.WWise.RTPC_Flor, 50);

            }
        }

        [ContextMenu("CRECE CARLA")]
        public void activateFlor() {
            flowerOpened = true;
            GetComponentInChildren<LotoAnimatorController>().LotoOpen();
            AkSoundEngine.PostEvent("Flor_Abrir_In", gameObject);
            //AkSoundEngine.SetRTPCValue(Keys.WWise.RTPC_Flor, 100);
            FlowerManager.Instance.OnFlowerOpening();
            listaElementosCrecedores.ForEach(e => { if (e) e.RandomGrow(); });
        }

        //public void ChangeSound() {
        //    int value = 0;
        //    DOTween.To(() => value, x => value = x, 100, 1).OnUpdate(() => coso(value));
        //}

        //private void coso(int value) {
        //    AkSoundEngine.SetRTPCValue(Keys.WWise.RTPC_Flor, value);

        //}

        [ContextMenu("DESCRECE CARLA")]
        public void deActivateFlor() {
            flowerOpened = false;
            // Hace la animación un poco regulinchi
            GetComponentInChildren<LotoAnimatorController>().LotoClose();
            listaElementosCrecedores.ForEach(e => { if (e) e.Shrink(); });
        }

        public void getNodeNeighbors() {
            listaNoditos = nodito.GetListNeighbors();
        }

        private bool CheckWaterAround() {
            return listaNoditos.Find(e => e.water.IsActive());
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
        [ContextMenu("oh")]
        public void CheckGrownd() {
            RaycastHit groundHits;
            //TODO Que el suelo tenga su porpia layer y que aqui se coja sola esa layer y no todas
            if (Physics.Raycast(transform.position, Vector3.down, out groundHits, 2f, groundLayer)) {


                nodito = groundHits.collider.GetComponent<Node>();
            }
        }
    }
}
