using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    public GameObject titleMenu, startMenu;

    private void Awake() {
        titleMenu.SetActive(true);
        startMenu.SetActive(false);
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
