using ElJardin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {

    public float maxValue = 1f;
    public float minValue = 0f;
    public bool growing = false;
    public bool isGonnaHaveDaWote;
    public GameObject waterN, waterW, waterE, waterS, waterStatic;

    #region PreParams
    bool grow; System.Action middleCallback; Node initNode;
    #endregion
    [ContextMenu("Grow")]
    public void Grow(bool grow, System.Action middleCallback, Node initNode) {
        if (!growing) {
            if (initNode == null) {
                initNode = GetComponentInParent<Node>();
            }
            Node thisNode = GetComponentInParent<Node>();
            Rotate(thisNode);

            if (initNode.column == thisNode.column && initNode.row > thisNode.row) {
                StartCoroutine(CorGrow(grow, middleCallback, waterN));
            } else
            if (initNode.column > thisNode.column && initNode.row == thisNode.row) {
                StartCoroutine(CorGrow(grow, middleCallback, waterE));
            } else
            if (initNode.column < thisNode.column && initNode.row == thisNode.row) {
                StartCoroutine(CorGrow(grow, middleCallback, waterW));
            } else
            if (initNode.column == thisNode.column && initNode.row < thisNode.row) {
                StartCoroutine(CorGrow(grow, middleCallback, waterS));
            } else {
                StartCoroutine(CorGrow(grow, middleCallback, waterS));
            }

            if (grow) {
                thisNode.RemoveDryComponent();
            }
        }
    }
    public void PrepareGrow(bool grow, System.Action middleCallback, Node initNode) {
        isGonnaHaveDaWote = grow;
        this.grow = grow; this.middleCallback = middleCallback; this.initNode = initNode;
    }
    public void DoPreparatedGrow() {
        Grow(grow, middleCallback, initNode);
    }
    public void Rotate(Node thisNode) {
        transform.SetParent(null);
        transform.rotation = Quaternion.identity;
        transform.SetParent(thisNode.transform);
    }

    private IEnumerator CorGrow(bool grow, System.Action middleCallback, GameObject water) {
        float mayor, menor, incremento, valueZ, callTriger = .8f;
        bool invoked = false;
        growing = true;
        if (!waterStatic.activeSelf) {
            if (grow) {
                water.SetActive(true);
                valueZ = 0;
                mayor = maxValue;
                menor = valueZ;
                incremento = .02f;
                while (mayor >= menor) {
                    valueZ += incremento;
                    if (!invoked && valueZ >= callTriger) {
                        invoked = true;
                        middleCallback?.Invoke();
                    }
                    water.transform.localScale = new Vector3(1, 1f, valueZ);
                    yield return new WaitForEndOfFrame();
                    if (grow) {
                        menor = valueZ;
                    } else {
                        mayor = valueZ;
                    }
                }
            } else {
                valueZ = 1;
                mayor = valueZ;
                menor = minValue;
                incremento = -.1f;

                water.transform.localScale = new Vector3(1, 1f, valueZ);
            }

            yield return new WaitForSeconds(.5f);
            waterStatic.SetActive(grow);
            water.SetActive(false);
            growing = false;


        } else {
            yield return new WaitForSeconds(.1f);

            middleCallback?.Invoke();

        }
        growing = false;
        isGonnaHaveDaWote = false;
    }

    public bool IsActive() {
        return waterStatic.activeSelf;
    }
}