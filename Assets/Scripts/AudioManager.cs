using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ElJardin {
    public class AudioManager : Singleton<AudioManager> {
        private float musicValue, sfxValue;

        private void Awake() {

        }

        private void Start() {
            //AkSoundEngine.SetState("General_Music", "Musica_Inicio");
            // Si va en el Awake peta, esta linea irá en la clase que inicie bien el juego
            AkSoundEngine.PostEvent("Amb_Base_In", gameObject);

        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.O)) {

                AkSoundEngine.SetState("General_Music", "Musica_Ingame");
            } else if (Input.GetKeyDown(KeyCode.L)) {

                AkSoundEngine.SetState("General_Music", "Musica_Inicio");
                //AkSoundEngine.PostEvent("UI_Select_In", gameObject);
            }
        }
    }
}
