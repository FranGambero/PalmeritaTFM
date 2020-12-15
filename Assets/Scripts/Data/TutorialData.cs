using System;
using UnityEngine;

[Serializable]
public class TutorialData
{
    public string title;
    [TextArea]
    public string body;
    public Sprite sprite;
}
