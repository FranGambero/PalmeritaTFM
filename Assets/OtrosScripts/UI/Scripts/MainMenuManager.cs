using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ElJardin {
    public class MainMenuManager : MonoBehaviour {
        public GameObject titleMenu, startMenu;
         public FadeOutPanel fadeOutPanel;
        public MenuButton btnPlay, btnConfig, btnCredits;
        public TMPro.TextMeshProUGUI test;
        private void Awake() {
            titleMenu.SetActive(true);
            startMenu.SetActive(false);
            MenuDirector.Instance.ActivateConfigMenu(false);

            InitButtons();
        }
        private void Start() {
            if(SessionVariables.Instance.sceneData.lastScene == -1)
                AudioManager.Instance.StartSetUILPF(false, 0.1f);
        }
        private void Update() {
            if (Input.anyKey && !startMenu.activeSelf) {
                changeToStartMenu();
            }

            if (Input.GetKeyDown(KeyCode.Escape)) {
                MenuDirector.Instance.ToggleConfigMenu();
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
            btnPlay.OnPreAnimationEvent += triggerFadeOutSound;
            btnPlay.OnPreAnimationEvent += IncreaseUILPF;
            fadeOutPanel.BtnTrigger = btnPlay;
            btnConfig.OnClickEvent = null;
            btnConfig.OnClickEvent = MenuDirector.Instance.ToggleConfigMenu;
            btnConfig.OnPreAnimationEvent += triggerButtonSound;
            btnCredits.OnClickEvent = null;
            btnCredits.OnClickEvent = StartCredits;
            btnCredits.OnPreAnimationEvent += triggerButtonSound;
        }
        public void IncreaseUILPF() {
            AudioManager.Instance.StartSetUILPF(true);
        }

        public void StartGame() {
            PlayerPrefs.SetInt(Keys.Scenes.LOAD_SCENE_INT, 2);
            goToGame();
        }
       
        public void triggerButtonSound() {
            AkSoundEngine.PostEvent("UI_Select_In", gameObject);
        }

        public void triggerFadeOutSound() {
            AkSoundEngine.PostEvent("UI_Trans_1_In", gameObject);
        }

        public void StartCredits() {
            Application.Quit();
        }
    }
}
