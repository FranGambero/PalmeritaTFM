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



        private void Awake()
        {
            Sepalo = FindObjectOfType<SepaloController>();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (configMenu.gameObject.activeSelf)
                    configMenu.CloseCongifMenu();
                else
                    configMenu.gameObject.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.R) && SceneManager.GetActiveScene().buildIndex != 0) {
                                        
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
            SceneManager.LoadScene(0);
        }
    }
}
