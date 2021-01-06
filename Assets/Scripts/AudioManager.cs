using DG.Tweening;
using DG.Tweening.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ElJardin {
    public class AudioManager : Singleton<AudioManager> {
        private bool levelInGame;
        private float currentTime;
        private const float maxiTime = 5f;

        private void Awake() {
            levelInGame = false;
            currentTime = 0f;
        }

        private void Start() {
            if (SessionVariables.Instance.sceneData.lastScene == -1) {
                AkSoundEngine.PostEvent("Musica_Switch_In", gameObject);
            }

            //if (SessionVariables.Instance.sceneData.currentScene == 2) {
            //    StartCoroutine(LPFCoroutine(true));
            //} else {
            //    StartCoroutine(LPFCoroutine(false));
            //}

            //setUILPF(0);

            if (PlayerPrefs.HasKey(Keys.Volume.PREF_VOL_SFX)) {
                AkSoundEngine.SetRTPCValue(Keys.WWise.RTPC_SFX, PlayerPrefs.GetInt(Keys.Volume.PREF_VOL_SFX));
            }
            if (PlayerPrefs.HasKey(Keys.Volume.PREF_VOL_MUSIC)) {
                AkSoundEngine.SetRTPCValue(Keys.WWise.RTPC_Music, PlayerPrefs.GetInt(Keys.Volume.PREF_VOL_MUSIC));
            }
        }


        /// <summary>
        /// Le pone el filtro en el time que el digamos
        /// </summary>
        /// <param name="increase"> Si sube o baja</param>
        /// <param name="time">El tiempo, si no se especifica es el maxiTime</param>
        public void StartSetUILPF(bool increase, float time = maxiTime) {

            var value = increase ? 0f : 100f;
            DOTween.To(() => value, x => value = x, increase ? 100 : 0, time).OnUpdate(() => setUILPF(value));
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

        public void setHappyMusic() {
            Debug.Log("Vamos a poner happy music");
            AkSoundEngine.SetState("General_Music", "Musica_Inicio");
        }

        public void setAmbientMusic() {
            // Me llaman al entrar a Mapamundi
            Debug.Log("Paso 1 y 3.- Me llaman al entrar a Mapamundi");
            AkSoundEngine.PostEvent("Amb_Base_In", gameObject);
            //AkSoundEngine.PostEvent("UI_LPF_In", gameObject);
        }

        public void unSetAmbientMusic() {
            //Al volver de Mapamundi a UI
            Debug.Log("Quito ambientes");
            AkSoundEngine.PostEvent("Amb_Base_Out", gameObject);
        }

        public void setUILPF(float newValue) {
            AkSoundEngine.SetRTPCValue(Keys.WWise.RTPC_LPF, newValue);
        }

        private IEnumerator LPFCoroutine(bool goUp) {
            Debug.Log("Se vino la LPF bola");
            float lerp = 0f;
            float duration = 5f;

            int initialValue;
            int targetValue;

            if (goUp) {
                initialValue = 0;
                targetValue = 100;

            } else {
                initialValue = 100;
                targetValue = 0;
            }

            while (initialValue != targetValue) {
                Debug.Log("}}}}}}}}}}}}}}}}}}}}}}}}} Er tiempecito " + initialValue + " / " + targetValue);
                lerp += Time.deltaTime / duration;
                initialValue = (int)Mathf.Lerp(initialValue, targetValue, lerp);

                setUILPF(initialValue);

                yield return new WaitForEndOfFrame();
            }

        }
    }
}
