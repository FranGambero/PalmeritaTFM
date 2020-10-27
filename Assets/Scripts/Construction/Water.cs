using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {
    public float valuey = 0.4f;
    public float valuex = 0;
    public float maxValue = 1.5f;
    public float minValue = 0f;
    public bool growing = false;
    private void Update() {
        transform.localScale = new Vector3(valuex, valuey, 1.5f);
    }
    [ContextMenu("Grow")]
    public void Grow(bool grow, System.Action middleCallback) {
        if (!growing)
            StartCoroutine(CorGrow(grow, middleCallback));
    }
    private IEnumerator CorGrow(bool grow, System.Action middleCallback) {
        float targetVal = 0;
        bool invoked = false;
        growing = true;
        float call = .9f;
        if (!grow) {
            if (valuex > minValue) {
                targetVal = minValue;
                while (minValue < valuex) {
                    valuex -= .1f;
                    if (!invoked && valuex >= call) {
                        invoked = true;
                        middleCallback?.Invoke();
                    }

                    yield return new WaitForEndOfFrame();
                }
                // valuey = 0;
                valuex = 0;
            }

        } else {
            if (valuex < maxValue) {
                targetVal = maxValue;
                valuex = 0;
                while (maxValue > valuex) {
                    valuex += .01f;
                    if (!invoked && valuex >= call) {
                        Debug.Log(valuex);
                        invoked = true;
                        middleCallback?.Invoke();
                    }
                    yield return new WaitForEndOfFrame();
                }
            }
        }
        growing = false;
    }
}