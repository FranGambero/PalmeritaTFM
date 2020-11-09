using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProtaAnimationController : MonoBehaviour
{
    Rigidbody rigidbody;
    enum State {
        Idle,Walk,Run
    }
    State state;
  
    public void OnStep() {
        AkSoundEngine.PostEvent("Sepalo_Steps_In", gameObject);
    }

}
