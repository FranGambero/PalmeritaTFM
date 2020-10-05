using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfigMenuManager : MonoBehaviour {
    public MenuButton backBtn;
    public Slider musicSlider, sfxSlider;
    public TextMeshProUGUI volMusicTextValue, volSFXTextValue;
    private float volMusicValue, volSFXValue;

    private void Awake() {
        //To be changed when we have both buses working, currently only SFX does -------------------
        musicSlider.value = 5f;
        sfxSlider.value = 5f;
        volMusicValue = volSFXValue = 100;
        refreshTextValues();
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
        float newVolValue = targetMusicSlider.value * 20;
        if (stringParam == "Vol_SFX") {
            if (newVolValue > volSFXValue) {
                AkSoundEngine.PostEvent("UI_Vol_Up_In", gameObject);

            } else {
                AkSoundEngine.PostEvent("UI_Vol_Down_In", gameObject);
            }
            volSFXValue = newVolValue;
        } else {
            // Estamos cambiando el bus de Musica
            if (newVolValue > volMusicValue) {
                AkSoundEngine.PostEvent("UI_Vol_Up_In", gameObject);

            } else {
                AkSoundEngine.PostEvent("UI_Vol_Down_In", gameObject);
            }
            volMusicValue = newVolValue;
        }
        refreshTextValues();
        AkSoundEngine.SetRTPCValue(stringParam, newVolValue);
    }

    private void refreshTextValues() {
        volMusicTextValue.text = volMusicValue.ToString();
        volSFXTextValue.text = volSFXValue.ToString();
    }
}
