using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotoAnimatorController : MonoBehaviour {
    public Animator BallAnimator;
    public Animator lotoAnimator;
    public void OnHalfOpen() {
        BallAnimator.Play("BallGo");
    }
    public void OnClose() {
        BallAnimator.Play("BallBack");
    }

    public void LotoOpen() {
        lotoAnimator.SetInteger("Status", 1);
    }

    public void LotoClose() {
        lotoAnimator.SetInteger("Status", -1);
    }
}
