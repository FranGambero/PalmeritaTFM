using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntWaves : MonoBehaviour {
    public Transform tEast;
    public Transform tWest;
    public Transform tSouth;
    public Transform tNorth;
    public Animator anim;
    public float delay;

    public void Touch() {
        PlayAll();
    }
    public void SuperTouch() {
        VFXDirector.Instance.Play("Impulse", tSouth);

        PlayAll();
    }
    public void Play(int direction) {
        switch (direction) {
            case 0:
                VFXDirector.Instance.Play("Wave", tEast);
                break;
            case 1:
                VFXDirector.Instance.Play("Wave", tWest);
                break;
            case 2:
                VFXDirector.Instance.Play("Wave", tSouth);
                break;
            case 3:
                VFXDirector.Instance.Play("Wave", tNorth);
                break;
            default:
                break;
        }
    }
    [ContextMenu("PlayAll")]
    public void PlayAll() {
        // StartCoroutine(Play());
        for (int i = 0; i < 4; i++) {
            Play(i);
        }

    }
    IEnumerator Play() {
        yield return new WaitForSeconds(.15f);
        for (int i = 0; i < 4; i++) {
            Play(i);
        }
        yield return new WaitForSeconds(.05f);
        for (int i = 0; i < 4; i++) {
            Play(i);
        }
    }
    IEnumerator PlayMultipleTimes(int direction, int times) {
        for (int i = 0; i < times; i++) {
            Play(direction);
            yield return new WaitForSeconds(delay);
        }
    }
}
