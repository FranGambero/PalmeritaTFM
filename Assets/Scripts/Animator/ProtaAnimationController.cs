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
    void Start()
    {
        state = State.Idle;
        rigidbody = transform.parent.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Speeed "+rigidbody.velocity);
        switch (state) {
            case State.Idle:
                if (rigidbody.velocity.magnitude > 0) {

                }
                break;
            case State.Walk:
                break;
            case State.Run:
                break;
            default:
                break;
        }
    }
}
