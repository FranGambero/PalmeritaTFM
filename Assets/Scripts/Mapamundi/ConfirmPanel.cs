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
        fadeOutPanel.BtnTrigger = btnPlay;
        btnPlay.OnClickEvent += PlayLevel;
    }

    private void Update() {
        if (this.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape)) {
            Cancel();
        }
    }

    void Cancel() {
        this.gameObject.SetActive(false);
    }

    public void recargaLogros() {
        logrosPanel.GetLogritos(levelIdToLoad);
    }

    public void PlayLevel() {
        levelStringToLoad = "Level" + levelIdToLoad;

        Debug.LogWarning("Voy a " + SceneManager.GetSceneByName(levelStringToLoad).buildIndex);
        PlayerPrefs.SetInt("CurrentLevel", levelIdToLoad);
        PlayerPrefs.SetInt(Keys.Scenes.LOAD_SCENE_INT, -1);
        PlayerPrefs.SetString(Keys.Scenes.LOAD_SCENE_STRING, levelStringToLoad);

        AudioManager.Instance.setIngameMusic();
        //AudioManager.Instance.unSetUILPF();
        SceneManager.LoadScene("LoadScene");
    }
    public int GetLevelBuildId() {
        levelStringToLoad = "Level" + levelIdToLoad;
        return SceneManager.GetSceneByName(levelStringToLoad).buildIndex;
    }
}
