using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoGrow : MonoBehaviour {
    public float size = 1;
    public float time = 1;
    public bool growed = false;

    private void Start() {
        if (growed) {
            QuickGrow();
        } else {
            transform.DOScale(0, 0.1f);
        }
    }

    [ContextMenu("Grow")]
    public void Grow() {
        if (!growed) {
            transform.DOScale(size, time).SetEase(Ease.OutBack);
            growed = true;
        }
    }
    public void QuickGrow() {
        float tmpTime = time;
        time = 0.1f;
        Grow();
        time = tmpTime;
    }
    public void RandomGrow() {
        Invoke(nameof(Grow), Random.Range(1, 3f));
    }

    [ContextMenu("Encoje")]
    public void Shrink() {
        transform.DOScale(0, time);
    }
}
