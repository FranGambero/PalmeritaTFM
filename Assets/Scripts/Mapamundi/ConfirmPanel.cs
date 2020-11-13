using ElJardin;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConfirmPanel : MonoBehaviour
{
    public int levelIdToLoad;
    public string levelStringToLoad;
    public TextMeshProUGUI levelName;
    public LogrosPanel logrosPanel;

    private void Awake() {
        logrosPanel = GetComponentInChildren<LogrosPanel>();
    }

    private void Start() {
        if (this.gameObject.activeSelf) {
            this.gameObject.SetActive(false);
        }
    }

    private void Update() {
        if(this.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape)) {
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
        Debug.LogWarning("Voy a " + levelStringToLoad);
        PlayerPrefs.SetInt("CurrentLevel", levelIdToLoad);
        AudioManager.Instance.setIngameMusic();
        SceneManager.LoadScene(levelStringToLoad);
    }
}
