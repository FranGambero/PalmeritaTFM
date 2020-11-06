using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public Sprite[] petals;
    public Sprite lockedSprite;

    public TextMeshProUGUI levelText;
    public int level;
    public int numPetals = 0;
    public ConfirmPanel confirmPanel;

    public bool isActive;

    private void Awake() {
        if(confirmPanel == null) {
            //No va :(((
            confirmPanel = FindObjectOfType<ConfirmPanel>();
        }
    }

    private void Start() {
        ChangeSprite();
    }

    public void ShowConfirmPanel() {
        confirmPanel.gameObject.SetActive(true);
        // No va :(
        confirmPanel.levelName.text = levelText.text;
    }

    public void ChangeSprite() {
        this.gameObject.GetComponent<Image>().sprite = petals[numPetals];
    }
}
