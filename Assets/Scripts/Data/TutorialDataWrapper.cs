using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTutorialData", menuName = "Tutorial/TutorialData")]
public class TutorialDataWrapper : ScriptableObject {
    [SerializeField]
    private TutorialData data;

    [SerializeField]
    public TutorialData Data => data;
}
