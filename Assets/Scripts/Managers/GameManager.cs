using ElJardin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElJardin {
    public class GameManager : Singleton<GameManager> {
        public CharacterController myCharacterController;
        public ConfigMenuManager mycoso;

        private void Awake() {
            myCharacterController = FindObjectOfType<CharacterController>();
            //mycoso = FindObjectOfType<ConfigMenuManager>();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (mycoso.gameObject.activeSelf)
                    mycoso.CloseCongifMenu();
                else
                    mycoso.gameObject.SetActive(true);
            }
        }

    }
}
