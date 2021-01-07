using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePetalController : MonoBehaviour {
    public GameObject Petal;
    public GameObject Cocoon;
    public GameObject Pod;
    public ParticleSystem psComplete;
    public Fase fase = Fase.Pod;
    public float animTime = 1f;
    public float delay;
    private List<DoGrow> elementsToAppears;
    public Transform parentElementsToAppears;

    public enum Fase {
        Pod, Cocoon, Petal
    }
    private void Start() {
    }
    public void SetFase(Fase fase, bool anim) {
        elementsToAppears = new List<DoGrow>(parentElementsToAppears.GetComponentsInChildren<DoGrow>());
        elementsToAppears.ForEach(e => e.QuickShrink());
        GameObject pre = GetFaseObject(this.fase);
        this.fase = fase;
        GameObject post = GetFaseObject(this.fase);
        if (anim) {
            StartCoroutine(ChangeFaseWAnim(pre, post));
        } else {
            post.SetActive(true);
        }

        if (fase == Fase.Pod || fase == Fase.Cocoon) {
            elementsToAppears.ForEach(e => e.QuickShrink());
        } else {
            if (anim) {
                elementsToAppears.ForEach(e => e.RandomGrow());
            } else {
                elementsToAppears.ForEach(e => e.QuickGrow());
            }
        }

    }
    private IEnumerator ChangeFaseWAnim(GameObject pre, GameObject post) {
        yield return new WaitForSeconds(delay);
        float value = 0;
        post.SetActive(false);
        Material mat = Instantiate(pre.GetComponent<Image>().material);
        pre.GetComponent<Image>().material = mat;
        DOTween.To(() => value, x => value = x, 1f, animTime).OnUpdate(() => mat.SetFloat("_FlashAmount", value)).OnComplete(() => {
            post.SetActive(true);
            mat.SetFloat("_FlashAmount", 0);
            AkSoundEngine.PostEvent("UI_Hit_Flor_In", gameObject);
            psComplete.Play();
        });
    }
    private GameObject GetFaseObject(Fase fase) {
        GameObject faseG;
        switch (fase) {
            case Fase.Pod:
                faseG = Pod;
                break;
            case Fase.Cocoon:
                faseG = Cocoon;
                break;
            case Fase.Petal:
                faseG = Petal;
                break;
            default:
                faseG = Petal;
                break;
        }
        return faseG;
    }
    public void Reset() {
        Petal.SetActive(false);
        Cocoon.SetActive(false);
        Pod.SetActive(true);
    }
}
