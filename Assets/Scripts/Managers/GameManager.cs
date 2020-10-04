using ElJardin;
using System.Collections;
using System.Collections.Generic;
using ElJardin.Characters;
using UnityEngine;

namespace ElJardin {
    public class GameManager : Singleton<GameManager> {
        public SepaloController Sepalo;

        private void Awake() {
            Sepalo = FindObjectOfType<SepaloController>();
        }

    }
}
