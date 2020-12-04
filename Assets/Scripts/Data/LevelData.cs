using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    public string levelName;
    public int id;
    public int zone;
    public bool isCompleted;
    public Achievement[] logros;
}
