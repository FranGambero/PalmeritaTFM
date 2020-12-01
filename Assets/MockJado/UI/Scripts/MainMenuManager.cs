using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ElJardin {
<<<<<<< HEAD:Assets/MockJado/UI/Scripts/MenuManager.cs
    public class MenuManager : MonoBehaviour {
        public GameObject titleMenu, startMenu;
        public FadeOutPanel fadeOutPanel;
=======
    public class MainMenuManager : MonoBehaviour {
        public GameObject titleMenu, startMenu, FadeOutPanel;
>>>>>>> ec1ff6d4a8751bc7d5298923d894234554337eb0:Assets/MockJado/UI/Scripts/MainMenuManager.cs
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
            btnPlay.OnPreAnimationEvent = fadeOutPanel.FadeOut;
            btnPlay.OnPreAnimationEvent += triggerButtonSound;
            fadeOutPanel.BtnTrigger = btnPlay;
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
