using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
public class ScreenShotsUtil : EditorWindow {
    [MenuItem("Window/EditorUtils/ScreenShots/ScreenShotsWindow")]
    public static void ShowWindow() {
        EditorWindow.GetWindow<ScreenShotsUtil>("ScreenShots");
    }
    [MenuItem("Window/EditorUtils/ScreenShots/Chas!")]
    public static void TakeScreenShot() {
        string file= "Assets/Screenshots/" + EditorSceneManager.GetActiveScene().name + "_" + System.DateTime.Now.ToFileTime() + ".png";
        ScreenCapture.CaptureScreenshot(file);

    
    }
    void OnGUI() {


        GUILayout.Label("ScreenShots", EditorStyles.boldLabel);

        if (GUILayout.Button("Foto!")) {
            TakeScreenShot();
            // Clipboard.SetImage();
        }

    }
}
