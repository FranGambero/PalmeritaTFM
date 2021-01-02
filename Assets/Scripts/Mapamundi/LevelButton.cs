﻿using ElJardin;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {
    public List<ScorePetalController> petals;
    public GameObject lockedSprite;

    public LevelData levelData;
    public TextMeshProUGUI levelText;
    public int zoneId;
    public int levelId;
    public int numPetals = 0;
    public ConfirmPanel confirmPanel;
    private MapMove mapMoveController;

    public bool isActive;

    private void Awake() {
        mapMoveController = FindObjectOfType<MapMove>();
        if (confirmPanel == null) {
            //No va :(((
            confirmPanel = MenuManager.Instance.confirmPanel;
        }
    }

    private void Start() {
        GetLevelData();
        CheckAvailableLevel();
        ChangeSprite();
    }

    private void GetLevelData() {
        int currentZone = PlayerPrefs.GetInt("CurrentZone");
        this.levelData = MapamundiManager.Instance.GetCurrentLevel(zoneId, levelId);

        numPetals = Array.FindAll(levelData.logros, l => l.done).Length;
        levelText.text = levelData.levelName;
    }

    public void ShowConfirmPanel() {
        Debug.Log("Hay que quemo");
        mapMoveController.focusMove(levelId);
        StartCoroutine(nameof(MoveCoroutine));
    }

    public IEnumerator MoveCoroutine() {
        mapMoveController.levelManager.OnStartWalking();
        yield return new WaitUntil(() => mapMoveController.moveFinished == true);
        confirmPanel.Activate(true);
        AssignDataToPanel();
        if (mapMoveController.levelManager.playButton && mapMoveController.levelManager.playButton.gameObject.activeSelf)
            mapMoveController.levelManager.OnStopWalking(confirmPanel);
        else
            confirmPanel.Activate(true);
    }

    private void AssignDataToPanel() {
        confirmPanel.levelName.text = levelText.text;
        confirmPanel.levelIdToLoad = levelId;
        confirmPanel.logrosPanel.GetLogritos(levelId);
    }
    [ContextMenu("ChangeSprite")]
    public void ChangeSprite() {
        bool changed = false;
        for (int i = 0; i < numPetals; i++) {
            petals[i].SetFase(ScorePetalController.Fase.Petal, !levelData.logros[i].animationDone);
            if (!levelData.logros[i].animationDone)
                changed = true;
            levelData.logros[i].animationDone = true;
        }
        if (changed)
            MapamundiManager.Instance.SaveLevel(levelData);
    }

    private void CheckAvailableLevel() {
        //  Para poder empezar en el 1º nivel empezamos a checkear a partir del 2º
        isActive = true;
        if (levelId > 0) {
            LevelData previousLevelData = MapamundiManager.Instance.GetCurrentLevel(levelId - 1);
            isActive = previousLevelData.isCompleted;
        }
        petals.ForEach(p => p.Reset());


        if (!isActive) {
            petals.ForEach(p => p.Reset());
            //pods.ForEach(p => p.SetActive(!isActive));
            //cocoons.ForEach(c => c.SetActive(isActive));
        } else {
            petals.ForEach(p => p.SetFase(ScorePetalController.Fase.Cocoon, false));
        }
        GetComponent<Button>().interactable = isActive;
        lockedSprite.SetActive(!isActive);
    }
}
