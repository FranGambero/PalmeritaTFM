using ElJardin;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfigMenuManager : MonoBehaviour {
    public MenuButton backBtn, backMapBtn, instructionsBtn;
    public Slider musicSlider, sfxSlider;
    public TextMeshProUGUI volMusicTextValue, volSFXTextValue;
    private float volMusicValue, volSFXValue;
    public FadeOutPanel fadeOutPanel;
    public bool onMapamundi;

    private void Awake() {

        musicSlider.onValueChanged.AddListener(delegate {
            changeSoundSliderValue(musicSlider, "Vol_Musica");
        });

        sfxSlider.onValueChanged.AddListener(delegate {
            changeSoundSliderValue(sfxSlider, "Vol_SFX");
        });

        backBtn.OnClickEvent += CloseCongifMenu;
        if (backMapBtn) {
            if (onMapamundi) {
                backMapBtn.OnPreAnimationEvent += () => { fadeOutPanel.BtnTrigger = backMapBtn; fadeOutPanel.FadeOut(); };
                backMapBtn.OnPreAnimationEvent += () => AkSoundEngine.PostEvent("UI_Select_In", gameObject);
                backMapBtn.OnPreAnimationEvent += () => AkSoundEngine.PostEvent("UI_Trans_1_In", gameObject);
                backMapBtn.OnPreAnimationEvent += () => AudioManager.Instance.StartSetUILPF(false, 5f);

                backMapBtn.OnClickEvent += () => MenuManager.Instance.goToStartMenu();
            } else {
                backMapBtn.OnClickEvent += () => GameManager.Instance.goToMapamundi();
            }
        }
        if (instructionsBtn)
            instructionsBtn.OnClickEvent += () => GameManager.Instance.showInstructions(true);
    }

    private void OnEnable() {
       
        AudioManager.Instance.toggleMusicIngameState(false);
        reChargePlayerPrefs();
    }

    public void CloseCongifMenu() {
        MenuDirector.Instance.ActivateConfigMenu(false);
        
        gameObject.SetActive(false);
    }

    public void changeSoundSliderValue(Slider targetMusicSlider, string stringParam) {
        float newVolValue = targetMusicSlider.value * 20;
        if (stringParam == "Vol_SFX") {
            if (newVolValue > volSFXValue) {
                AkSoundEngine.PostEvent("UI_Vol_Up_In", gameObject);

            } else if (newVolValue != volSFXValue) {
                AkSoundEngine.PostEvent("UI_Vol_Down_In", gameObject);
            }
            volSFXValue = newVolValue;
        } else {
            // Estamos cambiando el bus de Musica
            if (newVolValue > volMusicValue) {
                AkSoundEngine.PostEvent("UI_Vol_Up_In", gameObject);

            } else if (newVolValue != volMusicValue) {
                AkSoundEngine.PostEvent("UI_Vol_Down_In", gameObject);
            }
            volMusicValue = newVolValue;
        }

        SetSettingValues();
        UpdateSliderValues();

        AkSoundEngine.SetRTPCValue(stringParam, newVolValue);
    }

    private void SetSettingValues() {
        PlayerPrefs.SetInt(Keys.Volume.PREF_VOL_SFX, (int)volSFXValue);
        PlayerPrefs.SetInt(Keys.Volume.PREF_VOL_MUSIC, (int)volMusicValue);
    }

    public void reChargePlayerPrefs() {
        if (PlayerPrefs.HasKey(Keys.Volume.PREF_VOL_SFX)) {

            volSFXValue = PlayerPrefs.GetInt(Keys.Volume.PREF_VOL_SFX);
        } else {
            volSFXValue = 100;
        }

        if (PlayerPrefs.HasKey(Keys.Volume.PREF_VOL_MUSIC)) {

            volMusicValue = PlayerPrefs.GetInt(Keys.Volume.PREF_VOL_MUSIC);
        } else {
            volMusicValue = 100;
        }

        UpdateSliderValues();
    }

    private void UpdateSliderValues() {
        sfxSlider.value = volSFXValue / 20;
        musicSlider.value = volMusicValue / 20;

        volMusicTextValue.text = volMusicValue.ToString();
        volSFXTextValue.text = volSFXValue.ToString();
    }


}
