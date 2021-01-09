using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetManager : MonoBehaviour {
    public bool pusheen;
    public float waitTime = 1.5f;
    private Coroutine checkCor;

    public Image chargingImage;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            StartPusheen();
        }

        if (Input.GetKeyUp(KeyCode.R)) {
            StopPusheen();
        }
    }

    private void OnMouseDown() {
        StartPusheen();
    }

    private void OnMouseUp() {
        StopPusheen();
    }

    private void CheckCorotuine() {
        if (checkCor != null) {
            StopCoroutine(checkCor);
            checkCor = null;
        }
    }

    public void StartPusheen() {
        pusheen = true;
        CheckCorotuine();
        StartCoroutine(chargeImage());
        checkCor = StartCoroutine(checkRestartTime());
    }

    public void StopPusheen() {
        pusheen = false;
        CheckCorotuine();

    }

    private IEnumerator checkRestartTime() {


        yield return new WaitForSeconds(waitTime);
        if (pusheen) {
            ResetScene();
        }
    }

    private IEnumerator chargeImage() {
        while (pusheen) {
            yield return new WaitForEndOfFrame();
            if (pusheen) {
                chargingImage.fillAmount += Time.deltaTime / waitTime;
            }
        }
        chargingImage.fillAmount = 0;
    }

    private void ResetScene() {
        PlayerPrefs.SetInt(Keys.Scenes.LOAD_SCENE_INT, SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("LoadScene");
    }
}
