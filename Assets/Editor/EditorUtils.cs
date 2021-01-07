using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public class EditorUtils : EditorWindow {

    string sceneName = "Level0";
    int index;

    [MenuItem("Window/EditorUtils/Scenes")]
    public static void ShowWindow() {
        EditorWindow.GetWindow<EditorUtils>("Scenes");
    }
    public static void GoStartMenu() {
        EditorSceneManager.OpenScene("Assets/Scenes/StartMenu.unity");
    }
    public static void GoMap() {
        EditorSceneManager.OpenScene("Assets/Scenes/Mapa.unity");
    }
    public static void GoLoadScene() {
        EditorSceneManager.OpenScene("Assets/Scenes/LoadScene.unity");
    }

    void OnGUI() {


        GUILayout.Label("Movimiento Rapido", EditorStyles.boldLabel);

        if (GUILayout.Button("Siguiente: Nivel" + GetNextSceneIndex())) {
            EditorSceneManager.OpenScene("Assets/Scenes/Level" + GetNextSceneIndex() + ".unity");
        }
        if (GUILayout.Button("Anterior: Nivel" + GetPrevSceneIndex())) {

            EditorSceneManager.OpenScene("Assets/Scenes/Level" + GetPrevSceneIndex() + ".unity");
        }

        GUILayout.Label("Principales", EditorStyles.boldLabel);

        if (GUILayout.Button("Main Menu")) {
            GoStartMenu();
        }
        if (GUILayout.Button("Mapa")) {
            GoMap();
        }
        if (GUILayout.Button("Pantalla de carga")) {
            GoLoadScene();
        }
        GUILayout.Label("Buscador", EditorStyles.boldLabel);
        sceneName = EditorGUILayout.TextField("Nombre Escena:", sceneName);
        if (GUILayout.Button("Buscar")) {
            EditorSceneManager.OpenScene("Assets/Scenes/" + sceneName + ".unity");

        }
    }
    private int GetNextSceneIndex() {
        if (index > (EditorSceneManager.sceneCountInBuildSettings - 3)) {
            index = 0;
        }
        int scene = (index++) % (EditorSceneManager.sceneCountInBuildSettings - 3);
        return scene;
    }
    private int GetPrevSceneIndex() {
        if (index < 0) {
            index = (EditorSceneManager.sceneCountInBuildSettings - 3);
        }
        int scene = (index--) % (EditorSceneManager.sceneCountInBuildSettings - 3);
        return (scene);
    }
}


