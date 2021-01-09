using ElJardin;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuDirector : Singleton<MenuDirector> {
    public ConfigMenuManager configMenu;
    public GameObject finalPanel;
    public Canvas baseCanvas;
    public Canvas victoryCanvas;
    public Canvas cardCanvas;

    public GameObject BGBlur;

    [Header("Prefabs")]
    public TutorialPanel tutoPanelPrefab;

    [Header("Lists")]
    public List<TutorialDataWrapper> openedTutoPanels;

    public UnityEvent OnAllTutosClosed => onAllTutosClosed;
    UnityEvent onAllTutosClosed = new UnityEvent();

    public void InitNewTutoPanel(TutorialDataWrapper tutorialData) {
        if (tutoPanelPrefab) {
            if (BGBlur) BGBlur.SetActive(true);
            if (GameManager.CheckInstance()) GameManager.Instance.OnPause = true;
            ActivateCardCanvas(false);
            TutorialPanel tuto = Instantiate(tutoPanelPrefab, baseCanvas.transform);
            tuto.Init(tutorialData);
            openedTutoPanels.Add(tuto.tutorialData);
        } else {
            Debug.LogError("No TutoPanelPrefab");
        }
    }
    public void CloseTuto(TutorialPanel tutorialPanel) {
        openedTutoPanels.Remove(tutorialPanel.tutorialData);
        AkSoundEngine.PostEvent("UI_Select_In", gameObject);
        Destroy(tutorialPanel.gameObject);
        if (openedTutoPanels.Count == 0) {
            if (BGBlur) BGBlur.SetActive(false);
            if (GameManager.CheckInstance()) GameManager.Instance.OnPause = false;
            ActivateCardCanvas(true);
            OnAllTutosClosed?.Invoke();
        }
    }
    public void ActivateConfigMenu(bool activate) {
        if (openedTutoPanels.Count > 0)
            activate = false;
        else {
            if (BGBlur)
                BGBlur.SetActive(activate);
            if (GameManager.CheckInstance())
                GameManager.Instance.OnPause = activate;
        }
        if (activate) {
            ActivateCardCanvas(false);
            configMenu.gameObject.SetActive(activate);
        } else {
            ActivateCardCanvas(true);
            AudioManager.Instance.toggleMusicIngameState(true);
            if(configMenu && configMenu.gameObject.activeInHierarchy)
            AkSoundEngine.PostEvent("UI_Back_In", gameObject);
            configMenu.gameObject.SetActive(false);
        }
    }

    public void ToggleConfigMenu() {
        ActivateConfigMenu(!configMenu.gameObject.activeSelf);
    }


    public void ActivateEndMenu(bool activate, int currentLevel) {
        if (GameManager.CheckInstance()) GameManager.Instance.OnPause = activate;
        if (BGBlur) BGBlur.SetActive(false);
        if (activate) {
            AkSoundEngine.PostEvent("Victoria_In", gameObject);
            ActivateCardCanvas(false);
            victoryCanvas.gameObject.SetActive(true);
            ActivateLogrosPanel(currentLevel);
        } else {
            ActivateCardCanvas(true);
            victoryCanvas.gameObject.SetActive(false);

        }
    }
    private void ActivateLogrosPanel(int currentLevel) {
        LogrosPanel logrosPanel = FindObjectOfType<LogrosPanel>();
        // Quiza puede ser directamente el activar los ticks pasando el leveldata
        logrosPanel.GetLogritos(currentLevel);
    }

    public void ActivateCardCanvas(bool activate) {
        if (cardCanvas)
            cardCanvas.gameObject.SetActive(activate);
    }
}
