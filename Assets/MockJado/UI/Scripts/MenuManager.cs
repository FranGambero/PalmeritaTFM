using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    public GameObject titleMenu, startMenu, FadeOutPanel;
    public MenuButton btnPlay, btnConfig, btnCredits;
    public ConfigMenuManager configMenuManager;

    private void Awake() {
        titleMenu.SetActive(true);
        startMenu.SetActive(false);
        //configMenuManager = FindObjectOfType<ConfigMenuManager>();
        configMenuManager.gameObject.SetActive(false);

        InitButtons();
    }
    private void Update() {
        if (Input.anyKey && !startMenu.activeSelf) {
            changeToStartMenu();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && configMenuManager.gameObject.activeSelf) {
            toggleConfig();
        }
    }

    public void goToGame() {
        SceneManager.LoadScene("LoadScene");
    }

    public void changeToStartMenu() {
        titleMenu.SetActive(false);
        startMenu.SetActive(true);
    }
    public void InitButtons() {
        btnPlay.OnClickEvent = null;
        btnPlay.OnClickEvent = StartGame;
        btnPlay.OnPreAnimationEvent = FadeOut;
        btnPlay.OnPreAnimationEvent += triggerButtonSound;
        btnConfig.OnClickEvent = null;
        btnConfig.OnClickEvent = toggleConfig;
        btnConfig.OnPreAnimationEvent += triggerButtonSound;
        btnCredits.OnClickEvent = null;
        btnCredits.OnClickEvent = StartCredits;
        btnCredits.OnPreAnimationEvent += triggerButtonSound;
    }

    public void StartGame() {
        goToGame();
    }
    public void FadeOut() {
        FadeOutPanel.GetComponent<Animator>().Play("FadeOut");
    }

    public void toggleConfig() {
        if (configMenuManager.gameObject.activeSelf) {
            configMenuManager.CloseCongifMenu();
            //configMenuManager.gameObject.SetActive(false);
        } else {
            configMenuManager.gameObject.SetActive(true);
        }
    }

    public void triggerButtonSound() {
        AkSoundEngine.PostEvent("UI_Select_In", gameObject);
    }

    public void StartCredits() {
    }
}
