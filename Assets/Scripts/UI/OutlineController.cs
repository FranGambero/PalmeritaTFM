using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    public Material outlineMaterial;
    public GameObject outlineBody;
    public Vector3 activeSize, inactiveSize;
    public Color wrong, right;

    public void Activate(bool activate) {
        if (activate) {
            outlineBody.transform.DOScale(activeSize, .2f);
        } else {
            outlineBody.transform.DOScale(inactiveSize, .2f);
        }
    }
}
