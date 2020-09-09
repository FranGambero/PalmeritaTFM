using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    public GameObject titleMenu, startMenu;
    public MenuButton btnPlay;
    public MenuButton btnConfig;
    public MenuButton btnCredits;
    private void Awake() {
        titleMenu.SetActive(true);
        startMenu.SetActive(false);

        InitButtons();
    }

    public void goToGame() {
        SceneManager.LoadScene(1);
    }

    private void Update() {
        if (Input.anyKey && !startMenu.activeSelf) {
            changeToStartMenu();
        }
    }

    public void changeToStartMenu() {
        titleMenu.SetActive(false);
        startMenu.SetActive(true);
    }
    public void InitButtons() {
        btnPlay.OnClickEvent = null;
        btnPlay.OnClickEvent = StartGame;
        btnConfig.OnClickEvent = null;
        btnConfig.OnClickEvent = OpenConfig;
        btnCredits.OnClickEvent = null;
        btnCredits.OnClickEvent = StartCredits;
    }

    public void StartGame() {
        FindObjectOfType<MenuManager>().goToGame();
    }
    public void OpenConfig() {
    }
    public void StartCredits() {
    }
}
