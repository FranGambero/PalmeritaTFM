using ElJardin;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDirector : Singleton<MenuDirector>
{
    public ConfigMenuManager configMenu;
    public GameObject finalPanel;
    public Canvas baseCanvas;
    public Canvas victoryCanvas;
    public Canvas cardCanvas;

    [Header("Prefabs")]
    public TutorialPanel tutoPanelPrefab;

   
    public void InitNewTutoPanel(TutorialDataWrapper tutorialData) {
        if (tutoPanelPrefab) {
            Instantiate(tutoPanelPrefab, baseCanvas.transform).Init(tutorialData);
        } else {
            Debug.LogError("No TutoPanelPrefab");
        }
    }
    public void ActivateConfigMenu(bool activate) {
        configMenu.gameObject.SetActive(activate);
    }

    public void ToggleConfigMenu() {
        if (configMenu.gameObject.activeSelf)
            configMenu.CloseCongifMenu();
        else
            configMenu.gameObject.SetActive(true);
    }

    public void ActivateEndMenu(bool activate, int currentLevel) {
        if (activate) {
        cardCanvas.gameObject.SetActive(false);
        victoryCanvas.gameObject.SetActive(true);
        ActivateLogrosPanel(currentLevel);
        } else {
            cardCanvas.gameObject.SetActive(true);
            victoryCanvas.gameObject.SetActive(false);

        }
    }
    private void ActivateLogrosPanel(int currentLevel) {
        LogrosPanel logrosPanel = FindObjectOfType<LogrosPanel>();
        // Quiza puede ser directamente el activar los ticks pasando el leveldata
        logrosPanel.GetLogritos(currentLevel);
    }
}
