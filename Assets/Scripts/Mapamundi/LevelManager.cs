using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public List<LevelButton> levelTList;
    public int zone;
    public Button playButton;

    private void Start() {
        findLevels();
    }

    private void findLevels() {
        levelTList = new List<LevelButton>(transform.GetComponentsInChildren<LevelButton>());
    }

    public void OnStartWalking() {
        playButton.interactable = false;
    }

    public void OnStopWalking(ConfirmPanel confirmPanel) {
        playButton.interactable = true;
        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(()=>confirmPanel.Activate(true));
    }
}
