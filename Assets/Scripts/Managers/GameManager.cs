using ElJardin.Characters;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ElJardin {
    public class GameManager : Singleton<GameManager> {
        public SepaloController Sepalo;
        public GameObject instructMenu;
        public GameObject ShovelCrabPrefab;
        public GameObject _positionHover;
        public Vector3 tmpRot;

        public List<TutorialDataWrapper> levelTutos;
        private Node selectedNode;
        
        //public bool DraggingCard;

        public bool draggingCard;
        private bool gameRunning;
        private bool onPause = false;
        //public UnityEvent<bool> OnPause => onPause;
        //UnityEvent onPause = new UnityEvent();

        public bool CanPlay {
            get {
                return gameRunning && !OnPause;
            }
        }
        public bool OnPause { get => onPause; set => onPause = value; }

        //TODO: cambiar nombre cuando se elimine el bool
        public Card selectedCard;

        #region Accessors
        public Node SelectedNode { get => selectedNode; set => selectedNode = value; }
        public GameObject PositionHover {
            get {
                return _positionHover;
            }
            set => _positionHover = value;
        }


        public bool DraggingCard => selectedCard != null;
        #endregion

        public void PosPositionHover(Vector3 newPosition) {
            _positionHover.transform.position = newPosition;
            _positionHover.transform.LookAt(new Vector3(Sepalo.transform.position.x, _positionHover.transform.position.y, Sepalo.transform.position.z));
        }
        private void Awake() {
            _positionHover?.SetActive(false);
            draggingCard = false;
            selectedCard = null;
            Sepalo = FindObjectOfType<SepaloController>();
            MenuDirector.Instance.ActivateConfigMenu(false);
        }

        private void Start() {
            StartGame();
            LaunchTutos();
            // AkSoundEngine.PostEvent("Amb_Base_In", gameObject);
        }
        private void LaunchTutos() {
            if (levelTutos.Count > 0) {
                MenuDirector.Instance.OnAllTutosClosed.AddListener(() => CardManager.Instance.firstDrawCard());
                levelTutos.ForEach(t => MenuDirector.Instance.InitNewTutoPanel(t));
            } else {
                MenuDirector.Instance.ActivateCardCanvas(true);
                CardManager.Instance.firstDrawCard();
            }
        }

        public void StartGame() {
            gameRunning = true;
            AkSoundEngine.PostEvent("Amb_Agua_In", gameObject);
        }
        public void EndGame() {
            gameRunning = false;
            AudioManager.Instance.unSetAmbientMusic();
            MapManager.Instance.CheckLogros();
            MenuDirector.Instance.ActivateEndMenu(true, PlayerPrefs.GetInt("CurrentLevel"));
        }
        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                MenuDirector.Instance.ToggleConfigMenu();
            }

            // if (Input.GetKeyDown(KeyCode.R) && SceneManager.GetActiveScene().buildIndex != 0) {
            //     PlayerPrefs.SetInt(Keys.Scenes.LOAD_SCENE_INT, SceneManager.GetActiveScene().buildIndex);
            //     SceneManager.LoadScene("LoadScene");
            // }
        }

        public void showInstructions(bool showInstruc) {
            if (showInstruc) {
                instructMenu.SetActive(true);
            } else {
                instructMenu.SetActive(false);
            }
        }

        private void OnDestroy() {
            AkSoundEngine.PostEvent("Amb_Agua_Out", gameObject);
        }

        public void goToStartMenu() {
            PlayerPrefs.SetInt("LastScene", SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadScene(0);
        }

        public void goToMapamundi() {
            // Esto quizá hay que cambiarlo....
            AudioManager.Instance.setHappyMusic();
            //----
            SceneManager.LoadScene(2);
        }

        public void goNextLevel() {
            int currentLevel = PlayerPrefs.GetInt(Keys.Scenes.CURRENT_LEVEL) + 1;
            PlayerPrefs.SetInt(Keys.Scenes.CURRENT_LEVEL, currentLevel);

            string levelStringToLoad = "Level" + MapamundiManager.Instance.currentZone + "_" + currentLevel;

            PlayerPrefs.SetInt(Keys.Scenes.LOAD_SCENE_INT, -1);
            PlayerPrefs.SetString(Keys.Scenes.LOAD_SCENE_STRING, levelStringToLoad);
            SceneManager.LoadScene("LoadScene");
        }
    }
}
