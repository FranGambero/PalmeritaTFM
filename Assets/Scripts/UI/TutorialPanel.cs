using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialPanel : MonoBehaviour
{
    public TutorialDataWrapper tutorialData;
    public TextMeshProUGUI title, body;
    public Image tutorialImage; 

    private void Awake() {
        Activate(true);
    }

    private void Start() {
        Init(tutorialData);
    }
    public void Activate(bool activate) {
        this.gameObject.SetActive(activate);
    }
    public void Init(TutorialDataWrapper tutorialData) {
        title.text = tutorialData.Data.title;
        body.text = tutorialData.Data.body;
        tutorialImage.sprite = tutorialData.Data.sprite;
        this.tutorialData = tutorialData;
    }

    private void Update() {
        if (Input.anyKeyDown && gameObject.activeSelf) {
            gameObject.SetActive(false);
        }
    }
}
