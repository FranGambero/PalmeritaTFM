using ElJardin;
using System.Collections;
using System.Collections.Generic;
using ElJardin.Characters;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ElJardin {
    public class GameManager : Singleton<GameManager> {
        public SepaloController Sepalo;
        public ConfigMenuManager configMenu;
        public GameObject instructMenu;
        public GameObject _positionHover;
        public Vector3 tmpRot;
        private Node selectedNode;
        public bool draggingCard;

        public Node SelectedNode { get => selectedNode; set => selectedNode = value; }
        public GameObject PositionHover {
            get {
                return _positionHover;
            }
            set => _positionHover = value;
        }
        public void PosPositionHover(Vector3 newPosition) {
            _positionHover.transform.position = newPosition;
            _positionHover.transform.LookAt(new Vector3(Sepalo.transform.position.x, _positionHover.transform.position.y, Sepalo.transform.position.z));
        }
        private void Awake() {
            _positionHover.SetActive(false);
            draggingCard = false;
            Sepalo = FindObjectOfType<SepaloController>();
        }

        private void Start() {
            AkSoundEngine.PostEvent("Amb_Base_In", gameObject);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (configMenu.gameObject.activeSelf)
                    configMenu.CloseCongifMenu();
                else
                    configMenu.gameObject.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.R) && SceneManager.GetActiveScene().buildIndex != 0) {
                PlayerPrefs.SetInt("NextLevel", SceneManager.GetActiveScene().buildIndex);
                SceneManager.LoadScene("LoadScene");
            }
        }

        public void showInstructions(bool showInstruc) {
            if (showInstruc) {
                instructMenu.SetActive(true);
            } else {
                instructMenu.SetActive(false);
            }
        }

        public void goToStartMenu() {
            PlayerPrefs.SetInt("LastScene", SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadScene(0);
        }

        public void goNextLevel(int nextLevelIndex) {
            PlayerPrefs.SetInt("NextLevel", nextLevelIndex);
            SceneManager.LoadScene("LoadScene");
        }
    }
}
