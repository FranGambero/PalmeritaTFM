using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialPanel : MonoBehaviour
{
    public TutorialDataWrapper tutorialData;
    public TextMeshProUGUI title, body;
    public Image tutorialImage; 

    private void Awake() {
        this.gameObject.SetActive(true);
    }

    private void Start() {
        title.text = tutorialData.Data.title;
        body.text = tutorialData.Data.body;
        tutorialImage.sprite = tutorialData.Data.sprite;
    }

    private void Update() {
        if (Input.anyKeyDown && gameObject.activeSelf) {
            gameObject.SetActive(false);
        }
    }
}
