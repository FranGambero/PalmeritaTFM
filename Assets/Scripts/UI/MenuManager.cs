using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ElJardin {
    public class MenuManager : MonoBehaviour {
        public GameObject configMenuPanel;
        public ConfirmPanel confirmPanel;

        private void Awake() {
            configMenuPanel.SetActive(false);
            confirmPanel = FindObjectOfType<ConfirmPanel>();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape) && !confirmPanel.gameObject.activeSelf) {
                toggleConfigMenu();
            }
        }

        private void toggleConfigMenu() {
            if (configMenuPanel.activeSelf) {
                configMenuPanel.SetActive(false);
            } else {
                configMenuPanel.SetActive(true);
            }
        }

        public void goToStartMenu() {
            // Quiza se puede poner en un ondestroy de esto o el Mapamundi scene?
            AudioManager.Instance.unSetUILPF();
            SceneManager.LoadScene(0);
        }
    }
}
