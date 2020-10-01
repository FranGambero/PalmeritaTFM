using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigMenuManager : MonoBehaviour {
    public MenuButton backBtn;
    public Slider musicSlider, sfxSlider;
    private float volMusicValue, volSFXValue;

    private void Awake() {
        //To be changed when we have both buses working, currently only SFX does -------------------
        musicSlider.value = 1f;
        sfxSlider.value = 1f;
        volMusicValue = volSFXValue = 100;
        // --------------------

        musicSlider.onValueChanged.AddListener(delegate {
            changeSoundSliderValue(musicSlider, "Vol_Musica");
        });

        sfxSlider.onValueChanged.AddListener(delegate {
            changeSoundSliderValue(sfxSlider, "Vol_SFX");
        });

        backBtn.OnClickEvent += CloseCongifMenu;
    }

    public void CloseCongifMenu() {
        AkSoundEngine.PostEvent("UI_Back_In", gameObject);
        gameObject.SetActive(false);
    }

    public void changeSoundSliderValue(Slider targetMusicSlider, string stringParam) {
        float newVolValue = targetMusicSlider.value * 100;
        if (stringParam == "Vol_SFX") {
            Debug.LogWarning(newVolValue + " skdjksjd " + volSFXValue);
            if (newVolValue > volSFXValue) {
                AkSoundEngine.PostEvent("UI_Vol_Up_In", gameObject);

            } else {
                AkSoundEngine.PostEvent("UI_Vol_Down_In", gameObject);
            }
            volSFXValue = newVolValue;
        } else {
            // Estamos cambiando el bus de Musica
        }
        Debug.LogWarning("Changing " + targetMusicSlider + " with " + stringParam + " for " + newVolValue);
        AkSoundEngine.SetRTPCValue(stringParam, newVolValue);
    }
}
