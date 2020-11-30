using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace ElJardin {

    public class ProtaAnimationController : MonoBehaviour {
        Rigidbody rigidbody;
        Animator anim;
        public float acelerationSpeed = .01f;
        enum State {
            Idle, Walk, Run
        }
        State state;
        private void Start() {
            anim = GetComponent<Animator>();
        }
        public void OnStep() {
            AkSoundEngine.PostEvent("Sepalo_Steps_In", gameObject);
        }

        public void StartWalkAnimation(Node targetNode) {
            StartCoroutine(CalculeWalkDistance(targetNode.GetPosition()));
        }
        private IEnumerator CalculeWalkDistance(Vector3 targetDistance) {
            targetDistance.y = transform.position.y;
            while (GameManager.Instance.Sepalo.isMoving) {
                if (GameManager.Instance.Sepalo.Movement.globalStartingNode == GameManager.Instance.Sepalo.CurrentNode) {
                    anim.SetFloat("speed", Mathf.Clamp(anim.GetFloat("speed") + acelerationSpeed, 0f, 1f));
                    Debug.Log("Anim+EN NODO INICIAL");
                }
                if (GameManager.Instance.Sepalo.Movement.globalTargetNode == GameManager.Instance.Sepalo.CurrentNode) {
                    anim.SetFloat("speed", Mathf.Clamp(anim.GetFloat("speed") - acelerationSpeed, 0f, 1f));
                    Debug.Log("Anim+EN NODO FINAL");
                }
                yield return new WaitForEndOfFrame();
            }
            if (anim.GetFloat("speed") != 0) {
                while (anim.GetFloat("speed") != 0) {
                    anim.SetFloat("speed", Mathf.Clamp(anim.GetFloat("speed") - acelerationSpeed * 2, 0f, 1f));
                    yield return new WaitForEndOfFrame();
                }

            }
        }
    }
}
