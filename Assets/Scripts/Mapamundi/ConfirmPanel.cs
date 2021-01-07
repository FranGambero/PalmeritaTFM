using ElJardin;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConfirmPanel : MonoBehaviour {
    public int levelIdToLoad;
    public string levelStringToLoad;
    public TextMeshProUGUI levelName;
    public LogrosPanel logrosPanel;
    public MenuButton btnPlay;
    public FadeOutPanel fadeOutPanel;

    private void Awake() {
        logrosPanel = GetComponentInChildren<LogrosPanel>();
    }

    private void Start() {
        if (this.gameObject.activeSelf) {
            this.gameObject.SetActive(false);
        }
        btnPlay.OnPreAnimationEvent += fadeOutPanel.FadeOut;
        btnPlay.OnPreAnimationEvent += triggerButtonSound;
        btnPlay.OnPreAnimationEvent += triggerFadeOutSound;
        fadeOutPanel.BtnTrigger = btnPlay;
        btnPlay.OnClickEvent += PlayLevel;
    }

    private void Update() {
        if (this.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape)) {
            Cancel();
        }
    }

    void Cancel() {
        AkSoundEngine.PostEvent("UI_Back_In", gameObject);
        this.gameObject.SetActive(false);
    }

    public void recargaLogros() {
        logrosPanel.GetLogritos(levelIdToLoad);
    }

    public void PlayLevel() {
        levelStringToLoad = "Level" + MapamundiManager.Instance.currentZone + "_"+ levelIdToLoad;
        PlayerPrefs.SetInt(Keys.Scenes.LAST_PLAYED_LEVEL, levelIdToLoad);
        SessionVariables.Instance.levels.lastPlayedLevel = levelIdToLoad;

        PlayerPrefs.SetInt("CurrentLevel", levelIdToLoad);
        PlayerPrefs.SetInt(Keys.Scenes.LOAD_SCENE_INT, -1);
        PlayerPrefs.SetString(Keys.Scenes.LOAD_SCENE_STRING, levelStringToLoad);

        AudioManager.Instance.setIngameMusic();
        MapamundiManager.Instance.SaveZoneData();
        //AudioManager.Instance.unSetUILPF(); 
        SceneManager.LoadScene("LoadScene");
    }
   
    public void Activate(bool activate) {
        if (activate) {
            gameObject.SetActive(true);
            AkSoundEngine.PostEvent("UI_Pantalla_Logro_In", gameObject);
        } else {
            gameObject.SetActive(false);
        }
    }
    public void triggerButtonSound() {
        AkSoundEngine.PostEvent("UI_Select_In", gameObject);
    }

    public void triggerFadeOutSound() {
        AkSoundEngine.PostEvent("UI_Trans_1_In", gameObject);
    }
}
