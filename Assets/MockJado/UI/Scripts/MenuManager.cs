using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
    public GameObject titleMenu, startMenu;

    private void Awake() {
        titleMenu.SetActive(true);
        startMenu.SetActive(false);
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
}
