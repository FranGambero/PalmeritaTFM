﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace ElJardin {
    public class LoadScreenManager : MonoBehaviour {
        public GameObject loadingScreenPanel;
        public Image loadedImg;
        public Text loadedText;
        public string sceneName;
        public float minLoadTime;
        private bool minTimeElapsed;
        private AsyncOperation async;

        void Awake() {
            loadedImg.fillAmount = 0f;
            loadedText.text = "0%";
        }
        private void Start() {
            LoadScene();

        }

        public void LoadScene() {
            minTimeElapsed = false;
            StartCoroutine(LoadScreenCoroutine());
            Invoke(nameof(TimeElapsed), minLoadTime);
        }

        private IEnumerator LoadScreenCoroutine() {
            loadingScreenPanel.SetActive(true);
            if (PlayerPrefs.GetInt(Keys.Scenes.LOAD_SCENE_INT) != -1)
                async = SceneManager.LoadSceneAsync(PlayerPrefs.GetInt(Keys.Scenes.LOAD_SCENE_INT));
            else
                async = SceneManager.LoadSceneAsync(PlayerPrefs.GetString(Keys.Scenes.LOAD_SCENE_STRING));

            async.allowSceneActivation = false;


            while (!async.isDone && !minTimeElapsed) {
                loadedImg.fillAmount = async.progress / 0.9f; // Trabajamos en 0 -> 0.9 porque 'progress' llega como máximo a 0.9f
                loadedText.text = $"{(int)(loadedImg.fillAmount * 100f)}%";
                if (async.progress >= 0.9f || minTimeElapsed) {
                    loadedImg.fillAmount = 1f;
                    loadedText.text = "100%";
                    async.allowSceneActivation = true;
                }
                yield return null;
            }

        }

        private void TimeElapsed() {
            minTimeElapsed = true;
        }
    }
}
