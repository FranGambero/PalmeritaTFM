using ElJardin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SessionVariables : Singleton<SessionVariables> {
    public SceneData sceneData;
    public LevelSavedData levels;
    private void Awake() {
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
        sceneData = new SceneData();
        levels = new LevelSavedData();
        levels.lastPlayedLevel = PlayerPrefs.GetInt(Keys.Scenes.LAST_PLAYED_LEVEL, -1);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.buildIndex != 1) {
            sceneData.lastScene = sceneData.currentScene;
            sceneData.currentScene = scene.buildIndex;
        }
    }
    [System.Serializable]
    public class SceneData {
        public int lastScene = -1;
        public int currentScene = -1;
    }

    [System.Serializable]
    public class LevelSavedData {
        public int lastPlayedLevel;
    }
}
