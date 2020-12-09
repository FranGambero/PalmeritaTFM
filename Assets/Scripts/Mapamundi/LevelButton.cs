using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {
    public List<ScorePetalController> petals;
    public Transform parentElementsToAppears;
    private List<DoGrow> elementsToAppears;
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
            confirmPanel = FindObjectOfType<ConfirmPanel>();
        }
        elementsToAppears = new List<DoGrow>(parentElementsToAppears.GetComponentsInChildren<DoGrow>());
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
        mapMoveController.focusMove(this.transform, levelId);
        StartCoroutine(nameof(MoveCoroutine));
    }

    public IEnumerator MoveCoroutine() {
        yield return new WaitUntil(() => mapMoveController.moveFinished == true);
        confirmPanel.gameObject.SetActive(true);
        AssignDataToPanel();
    }

    private void AssignDataToPanel() {
        confirmPanel.levelName.text = levelText.text;
        confirmPanel.levelIdToLoad = levelId;
        confirmPanel.logrosPanel.GetLogritos(levelId);
    }
    [ContextMenu("ChangeSprite")]
    public void ChangeSprite() {
        for (int i = 0; i < numPetals; i++) {
            petals[i].SetFase(ScorePetalController.Fase.Petal, true);
        }

        if (elementsToAppears.Count > 0) {
            if (numPetals > 0) {
                if (SessionVariables.Instance.sceneData.lastScene != confirmPanel.GetLevelBuildId()) {
                    elementsToAppears.ForEach(e => e.QuickGrow());
                } else {
                    elementsToAppears[0].Grow();
                    elementsToAppears.ForEach(e => e.RandomGrow());
                }
            } else {
                elementsToAppears.ForEach(e => e.QuickShrink());
            }
        }
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
