using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ElJardin {
    public class AudioManager : Singleton<AudioManager> {
        private bool levelInGame;

        private void Awake() {
            levelInGame = false;
        }

        private void Start() {
            // Si va en el Awake peta, esta linea irá en la clase que inicie bien el juego
            //AkSoundEngine.PostEvent("Amb_Base_In", gameObject);
            AkSoundEngine.PostEvent("Musica_Switch_In", gameObject);
        }

        public void toggleMusicIngameState(bool inGame = true) {
            levelInGame = inGame;
            if (levelInGame) {
                AkSoundEngine.SetState("General_Music", "Musica_Ingame");
            } else {
                AkSoundEngine.SetState("General_Music", "Musica_Inicio");
            }
        }
    }
}
