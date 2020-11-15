using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {
    public Sprite[] petals;
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

    public void ChangeSprite() {
        this.gameObject.GetComponent<Image>().sprite = petals[numPetals];
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
