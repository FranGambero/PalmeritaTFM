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
            if (SessionVariables.Instance.sceneData.lastScene == -1) {
                AkSoundEngine.PostEvent("Musica_Switch_In", gameObject);
            }

            if (PlayerPrefs.HasKey(Keys.Volume.PREF_VOL_SFX)) {
                AkSoundEngine.SetRTPCValue(Keys.WWise.RTPC_SFX, PlayerPrefs.GetInt(Keys.Volume.PREF_VOL_SFX));
            }
            if (PlayerPrefs.HasKey(Keys.Volume.PREF_VOL_MUSIC)) {
                AkSoundEngine.SetRTPCValue(Keys.WWise.RTPC_Music, PlayerPrefs.GetInt(Keys.Volume.PREF_VOL_MUSIC));
            }
        }

        public void toggleMusicIngameState(bool inGame = true) {
            levelInGame = inGame;
            if (levelInGame) {
                Debug.LogError("MUSIQUITA> " + levelInGame);
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

        public void setAmbientMusic() {
            AkSoundEngine.PostEvent("Amb_Base_In", gameObject);
        }
    }
}
