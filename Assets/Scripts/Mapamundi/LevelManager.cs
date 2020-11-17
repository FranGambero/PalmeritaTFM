using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<LevelButton> levelTList;

    private void Start() {
        findLevels();
    }

    private void findLevels() {
        levelTList = new List<LevelButton>(transform.GetComponentsInChildren<LevelButton>());
    }
}
