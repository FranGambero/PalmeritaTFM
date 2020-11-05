using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConfirmPanel : MonoBehaviour
{
    public string levelToLoad;
    public TextMeshProUGUI levelName;

    private void Start() {
        if (this.gameObject.activeSelf) {
            this.gameObject.SetActive(false);
        }
    }

    private void Update() {
        if(this.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape)) {
            Cancel();
        }
    }

    void Cancel() {
        this.gameObject.SetActive(false);
    }

    public void PlayLevel() {
        SceneManager.LoadScene(levelToLoad);
    }
}
