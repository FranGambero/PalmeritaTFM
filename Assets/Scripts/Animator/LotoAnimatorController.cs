using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotoAnimatorController : MonoBehaviour {
    public Animator BallAnimator;
    public void OnHalfOpen() {
        BallAnimator.Play("BallGo");
    }   public void OnClose() {
        BallAnimator.Play("BallBack");
    }
}
