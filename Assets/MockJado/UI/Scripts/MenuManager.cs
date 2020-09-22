using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    public GameObject titleMenu, startMenu, configMenu;
    public MenuButton btnPlay;
    public MenuButton btnConfig;
    public MenuButton btnCredits;

    private void Awake() {
        titleMenu.SetActive(true);
        startMenu.SetActive(false);
        configMenu.SetActive(false);

        InitButtons();
    }
    private void Update() {
        if (Input.anyKey && !startMenu.activeSelf) {
            changeToStartMenu();
        }

        if(Input.GetKeyDown(KeyCode.Escape) && configMenu.activeSelf) {
            toggleConfig();
        }
    }

    public void goToGame() {
        SceneManager.LoadScene(1);
    }

    public void changeToStartMenu() {
        titleMenu.SetActive(false);
        startMenu.SetActive(true);
    }
    public void InitButtons() {
        btnPlay.OnClickEvent = null;
        btnPlay.OnClickEvent = StartGame;
        btnConfig.OnClickEvent = null;
        btnConfig.OnClickEvent = toggleConfig;
        btnCredits.OnClickEvent = null;
        btnCredits.OnClickEvent = StartCredits;
    }

    public void StartGame() {
        FindObjectOfType<MenuManager>().goToGame();
    }

    public void toggleConfig() {
        if (configMenu.activeSelf) {
            configMenu.SetActive(false);
        } else {
            configMenu.SetActive(true);
        }
    }

    public void StartCredits() {
    }
}
