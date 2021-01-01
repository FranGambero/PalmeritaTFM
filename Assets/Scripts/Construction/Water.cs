using DG.Tweening;
using ElJardin;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {

    public float maxValue = 1f;
    public float minValue = 0f;
    public bool growing = false;
    public bool isGonnaHaveDaWote;
    public bool hasWater = false;
    public GameObject waterN, waterW, waterE, waterS, waterStatic;
    Node thisNode;
    #region PreParams
    bool grow; System.Action middleCallback; Node initNode;
    #endregion
    [ContextMenu("Grow")]
    public void Grow(bool grow, System.Action middleCallback, Node initNode) {
        if (!growing) {
            if (initNode == null) {
                initNode = GetComponentInParent<Node>();
            }
            thisNode = GetComponentInParent<Node>();
            Rotate(thisNode);
            if (grow) {
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
                    StartCoroutine(CorGrow(grow, middleCallback, waterStatic));
                }
                thisNode.dryController.StopDry(false);
            } else {
                StartCoroutine(Dry(grow, middleCallback));
            }
        }
    }

    public void Reset() {
        growing = false;
        isGonnaHaveDaWote = false;
        hasWater = false;
        waterStatic.SetActive(false);
        waterS.SetActive(false);
        waterE.SetActive(false);
        waterN.SetActive(false);
        waterW.SetActive(false);
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
        float mayor, menor, incremento, valueZ, callTriger = .7f;
        bool invoked = false;
        if (!growing) {
            growing = true;
            if (!hasWater) {
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
                        }
                    }
                }

                yield return new WaitForSeconds(.5f);
                water.SetActive(false);
                hasWater = true;
                waterStatic.SetActive(true);

            }
            growing = false;
        }
    }
    private IEnumerator Dry(bool grow, System.Action middleCallback) {
        float valueY;
        float animT = 1.5f;
        if (!growing) {
            if (!grow) {
                growing = true;

                valueY = waterStatic.transform.position.y;
                waterStatic.transform.DOMoveY(-1, animT).OnComplete(() => {
                    waterStatic.SetActive(false);
                    waterStatic.transform.position = new Vector3(waterStatic.transform.position.x, valueY, waterStatic.transform.position.z);
                    int dryIndex = Semaphore.Instance.GetNewIndex();
                    if (initNode && initNode.dryController.active)//Si ya se está secando uso su indice
                        dryIndex = initNode.dryController.turnIndex;
                   // thisNode.AdminDryScript(true, dryIndex);
                    growing = false;
                    hasWater = false;
                    if (thisNode.GetComponent<NodeDataModel>().isRiverStart) {
                        thisNode.Water();
                    }
                });
                yield return new WaitForSeconds(.01f);

                middleCallback?.Invoke();

                yield return new WaitForSeconds(animT);

            }
            isGonnaHaveDaWote = false;
        }
    }
    public bool IsActive() {
        return waterStatic.activeSelf || isGonnaHaveDaWote;
    }
}