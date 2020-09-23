using ElJardin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElJardin {
    public class GameManager : Singleton<GameManager> {
        public CharacterController myCharacterController;

        private void Awake() {
            myCharacterController = FindObjectOfType<CharacterController>();
        }

    }
}
