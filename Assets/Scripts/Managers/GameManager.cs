using ElJardin;
using System.Collections;
using System.Collections.Generic;
using ElJardin.Characters;
using UnityEngine;

namespace ElJardin {
    public class GameManager : Singleton<GameManager> {
        public SepaloController Sepalo;
        public ConfigMenuManager mycoso;


        private void Awake()
        {
            Sepalo = FindObjectOfType<SepaloController>();
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
