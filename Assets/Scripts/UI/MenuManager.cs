using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ElJardin {
    public class MenuManager : Singleton<MenuManager> {
        public GameObject configMenuPanel;
        public ConfirmPanel confirmPanel;

        private void Awake() {
            configMenuPanel.SetActive(false);
            if (!confirmPanel)
                confirmPanel = FindObjectOfType<ConfirmPanel>();
        }


        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape) && !confirmPanel.gameObject.activeSelf) {
                MenuDirector.Instance.ToggleConfigMenu();
            }
        }

        public void goToStartMenu() {
            SceneManager.LoadScene(0);
        }

    }
}
