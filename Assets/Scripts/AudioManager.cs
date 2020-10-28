using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ElJardin {
    public class AudioManager : Singleton<AudioManager> {
        private bool levelInGame;

        private void Awake() {
            levelInGame = false;
        }

        private void Start() {
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
                //AkSoundEngine.SetState("General_Music", "None");
            }
        }

        public void setIngameMusic() {
            AkSoundEngine.SetState("General_Music", "Musica_Ingame");

        }
    }
}
