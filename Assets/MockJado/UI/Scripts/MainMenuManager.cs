using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ElJardin {
    public class MainMenuManager : MonoBehaviour {
        public GameObject titleMenu, startMenu, FadeOutPanel;
        public MenuButton btnPlay, btnConfig, btnCredits;
        public ConfigMenuManager configMenuManager;

        private void Awake() {
            titleMenu.SetActive(true);
            startMenu.SetActive(false);
            configMenuManager.gameObject.SetActive(false);

            InitButtons();
        }
        private void Update() {
            if (Input.anyKey && !startMenu.activeSelf) {
                changeToStartMenu();
            }

            if (Input.GetKeyDown(KeyCode.Escape) && configMenuManager.gameObject.activeSelf) {
                toggleConfig();
            }
        }

        public void goToGame() {
            AudioManager.Instance.setAmbientMusic();
            SceneManager.LoadScene("LoadScene");
        }

        public void changeToStartMenu() {
            triggerButtonSound();
            titleMenu.SetActive(false);
            startMenu.SetActive(true);
        }
        public void InitButtons() {
            btnPlay.OnClickEvent = null;
            btnPlay.OnClickEvent = StartGame;
            btnPlay.OnPreAnimationEvent = FadeOut;
            btnPlay.OnPreAnimationEvent += triggerButtonSound;
            btnConfig.OnClickEvent = null;
            btnConfig.OnClickEvent = toggleConfig;
            btnConfig.OnPreAnimationEvent += triggerButtonSound;
            btnCredits.OnClickEvent = null;
            btnCredits.OnClickEvent = StartCredits;
            btnCredits.OnPreAnimationEvent += triggerButtonSound;
        }

        public void StartGame() {
            PlayerPrefs.SetInt("NextLevel", 2);
            goToGame();
        }
        public void FadeOut() {
            float time = .2f;
            if (FadeOutPanel.GetComponent<Animator>().runtimeAnimatorController.animationClips.Any(x => x.name == "FadeOut"))
                time = FadeOutPanel.GetComponent<Animator>().runtimeAnimatorController.animationClips.First(x => x.name == "FadeOut").length;
            Debug.Log("Animacion: " + FadeOutPanel + " " + time);
            btnPlay.animationLength = time;
            FadeOutPanel.GetComponent<Animator>().Play("FadeOut");
        }

        public void toggleConfig() {
            if (configMenuManager.gameObject.activeSelf) {
                configMenuManager.CloseCongifMenu();
            } else {
                configMenuManager.gameObject.SetActive(true);
            }
        }

        public void triggerButtonSound() {
            AkSoundEngine.PostEvent("UI_Select_In", gameObject);
        }

        public void StartCredits() {
            Application.Quit();
        }
    }
}
