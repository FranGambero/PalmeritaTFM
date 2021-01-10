using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlautibelulaAnimController : MonoBehaviour {
    public float fireTime;
    public Animator anim;
    public ParticleSystem FirePS;

    [ContextMenu("Attack")]
    public void Attack() {
        transform.parent.gameObject.SetActive(true);

        FirePS.Stop();
        anim.SetBool("firing", true);
        anim.Play("Enter");
        VFXDirector.Instance.Play("OnDestroyGround", transform.GetChild(0));
    }
    public void OnPreFire() {
        FirePS.Play();
    }
    [ContextMenu("Out")]
    public void Out() {
        anim.Play("Out");
    }
    public void OnFireStarts() {

        Invoke(nameof(StopFire), fireTime);
    }
    public void StopFire() {
        FirePS.Stop();
        anim.SetBool("firing", false);
    }
    [ContextMenu("Idle")]
    public void Idle() {
        anim.Play("Idle");
    }
    public void OnAnimationEnds() {
        VFXDirector.Instance.Play("OnDestroyGround", transform.GetChild(0));
        //transform.parent.gameObject.SetActive(false);
    }
}
