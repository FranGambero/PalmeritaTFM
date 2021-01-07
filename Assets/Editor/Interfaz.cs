using UnityEditor;
using UnityEditor.SceneManagement;

public class Interfaz : Editor {

    [MenuItem("Escenitas/StartMenu")]
    static void LoadScene1() {
        EditorSceneManager.OpenScene("Assets/Scenes/StartMenu.unity", OpenSceneMode.Single);
    }

    [MenuItem("Escenitas/Mapamundi")]
    static void LoadScene2() {
        EditorSceneManager.OpenScene("Assets/Scenes/Mapa.unity", OpenSceneMode.Single);
    }

    [MenuItem("Escenitas/Level 0")]
    static void LoadScene3() {
        EditorSceneManager.OpenScene("Assets/Scenes/Level0.unity", OpenSceneMode.Single);
    }
}
