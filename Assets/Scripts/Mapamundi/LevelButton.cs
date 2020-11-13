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

}
