using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChildrens : MonoBehaviour {
    public int childStart;
    public Gradient color;
    private int nChilds;
    void Start() {
        SetColor();
    }
    [ContextMenu("SetColor")]
    public void SetColor() {
        if (childStart < transform.childCount) {
            nChilds = transform.childCount - childStart;
            for (int i = childStart; i < transform.childCount; i++) {
                //  Debug.Log("Color: " + nChilds + ";" + i + " - " + childStart + " :: " + ((i - childStart) + .1f) / nChilds + "  " + (childStart - i));
                transform.GetChild(i).GetComponent<Renderer>().material.color = color.Evaluate(((i - childStart) + .1f) / nChilds);
            }
        }
    }
}
