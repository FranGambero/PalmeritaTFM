using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoGrow : MonoBehaviour {
    public Vector3 size;
    public float time = 1;
    public float delay = 0;
    public bool growed = false;

    public List<GrowChild> childs;

    private void Start() {
        if (growed) {
            QuickGrow();
        } else {
            QuickShrink();
            if (childs != null)
                foreach (GrowChild item in childs) {
                    item.body.AddComponent(typeof(DoGrow));
                    item.Init();
                }
        }
    }

    [ContextMenu("Grow")]
    public void Grow() {
        if (!growed) {
            Invoke(nameof(TimeredGrow), delay);
        }
    }
    private void TimeredGrow() {
        transform.DOScale(size, time);
        growed = true;
        childs?.ForEach(c => c.doGrow.Grow());
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
        growed = false;
    }
    public void QuickShrink() {
        growed = false;
        float tmpTime = time;
        time = 0.1f;
        transform.localScale = Vector3.zero;
        time = tmpTime;
    }
    [System.Serializable]
    public class GrowChild {
        public GameObject body;
        public float delay;
        public float time;
        public Vector3 size;
        public DoGrow doGrow { get { return body.GetComponent<DoGrow>(); } }
        public void Init() {
            DoGrow dg = doGrow;
            dg.delay = this.delay;
            dg.time = this.time;
            dg.size = this.size;
        }
    }
}

