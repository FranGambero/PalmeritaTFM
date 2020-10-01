using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigMenuManager : MonoBehaviour {
    public MenuButton backBtn;
    public Slider musicSlider, sfxSlider;

    private void Awake() {
        musicSlider.value = .5f;
        sfxSlider.value = .6f;

        musicSlider.onValueChanged.AddListener(delegate {
            changeSoundSliderValue(musicSlider, "Musica_Bus");
            });

        sfxSlider.onValueChanged.AddListener(delegate {
            changeSoundSliderValue(sfxSlider, "SFX_Bus");
        });

        backBtn.OnClickEvent += CloseCongifMenu;
    }

    private void OnDisable() {
    }

    public void CloseCongifMenu() {
        AkSoundEngine.PostEvent("UI_Back_In", gameObject);
        gameObject.SetActive(false);
    }

    public void changeSoundSliderValue(Slider targetMusicSlider, string stringParam) {
        Debug.LogWarning("IUSDFYUSYFUDYF " + targetMusicSlider + " con " + targetMusicSlider.value);

        AkSoundEngine.SetRTPCValue(stringParam, targetMusicSlider.value);



        //if (isMusicSlider) {
        //    musicSlider.value;
        //    Debug.LogWarning("MUSIC VALUE: " + musicValue);
        //    //AkSoundEngine.SetRTPCValue("NOMBRE", musicValue);
        //} else {
        //    sfxValue = sfxSlider.value;
        //    Debug.LogWarning("SFX VALUE: " + sfxValue);
        //    //AkSoundEngine.SetRTPCValue("NOMBRE2", sfxValue);
        //}
    }
}
