using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ElJardin {
    public class AudioManager : Singleton<AudioManager> {
        /*
         *    Empieza con Switch_In y al empezar partida lo quitamos con Amb_Out
         *    
         *             EL AMB_BASE_IN SUENA AL EMPEZAR PARTIDA  y OUT AL TERMINARLA
         * 
         * */
        private bool levelInGame;

        private void Awake() {
            levelInGame = false;
        }

        private void Start() {
            // Si va en el Awake peta, esta linea irá en la clase que inicie bien el juego
            //AkSoundEngine.PostEvent("Amb_Base_In", gameObject);   ESTO SUENA AL EMPEZAR LA PARTIDAAAAAA
            AkSoundEngine.PostEvent("Musica_Switch_In", gameObject);
        }

        public void toggleMusicIngameState(bool inGame = true) {
            levelInGame = inGame;
            if (levelInGame) {
                if (SceneManager.GetActiveScene().buildIndex == 0) {
                    AkSoundEngine.SetState("General_Music", "Musica_Inicio");
                } else {
                    setIngameMusic();
                }
            } else {
                AkSoundEngine.SetState("General_Music", "None");
            }
        }

        public void setIngameMusic() {
            AkSoundEngine.SetState("General_Music", "Musica_Ingame");

        }
    }
}
