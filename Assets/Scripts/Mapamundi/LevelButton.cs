using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {
    public GameObject[] petals;
    public List<DoGrow> elementsToAppears;
    public Sprite lockedSprite;

    public TextMeshProUGUI levelText;
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
    }

    private void Start() {
        ChangeSprite();
        CheckAvailableLevel();
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
        new List<GameObject>(petals).ForEach(p => p.SetActive(false));
        if (numPetals > 0) {

            for (int i = 0; i < numPetals; i++) {
                petals[i].SetActive(true);
            }
            if (elementsToAppears.Count > 0) {
                if (SessionVariables.Instance.sceneData.lastScene != confirmPanel.GetLevelBuildId()) {
                    elementsToAppears.ForEach(e => e.QuickGrow());
                } else {
                    elementsToAppears[0].Grow();
                    elementsToAppears.ForEach(e => e.RandomGrow());
                }
            }
        }
    }

    private void CheckAvailableLevel() {
        // Para poder empezar en el 1º nivel empezamos a checkear a partir del 2º
        if (levelId > 0) {
            LevelData levelData = MapamundiManager.Instance.GetCurrentLevel(levelId - 1);
            isActive = levelData.isCompleted;

            if (!isActive) {
                GetComponent<Button>().interactable = false;
            }
        }

    }
}
