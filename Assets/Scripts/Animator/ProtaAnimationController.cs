using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace ElJardin {

    public class ProtaAnimationController : MonoBehaviour {
        public List<Transform> footTransforms;
        public List<GameObject> footPS;
        Rigidbody rigidbody;
        Animator anim;
        Coroutine walkCor;
        public float acelerationSpeed = .01f;
        enum State {
            Idle, Walk, Run
        }
        State state;
        private void Start() {
            anim = GetComponent<Animator>();
        }
        public void OnStep(int foot) {
            AkSoundEngine.PostEvent("Sepalo_Steps_In", gameObject);
            if (GameManager.Instance.Sepalo.CurrentNode.GetNodeType() == NodeType.Water)
                VFXDirector.Instance.Play("Step", footTransforms[foot]);
        }
        public void ActivateFloatingPS(bool active) {
            if (active) {
                if (GameManager.Instance.Sepalo.CurrentNode.GetNodeType() == NodeType.Water) {
                    footPS.ForEach(f => f.SetActive(true));
                    Debug.Log("Sobre agua");
                }
            } else {
            footPS.ForEach(f => f.SetActive(false));

            }
        }
        public void StartWalkAnimation(Node targetNode) {
            if (walkCor != null) {
                StopCoroutine(walkCor);
                walkCor = null;
            }
            walkCor = StartCoroutine(CalculeWalkDistance(targetNode.GetPosition()));
        }
        private IEnumerator CalculeWalkDistance(Vector3 targetDistance) {
            targetDistance.y = transform.position.y;
            while (GameManager.Instance.Sepalo.isMoving) {
                if (GameManager.Instance.Sepalo.Movement.globalStartingNode == GameManager.Instance.Sepalo.CurrentNode) {
                    anim.SetFloat("speed", Mathf.Clamp(anim.GetFloat("speed") + acelerationSpeed, 0f, 1f));
                }
                if (GameManager.Instance.Sepalo.Movement.globalTargetNode == GameManager.Instance.Sepalo.CurrentNode) {
                    anim.SetFloat("speed", Mathf.Clamp(anim.GetFloat("speed") - .005f, 0.1f, 1f));
                }
                yield return new WaitForEndOfFrame();
            }
            if (anim.GetFloat("speed") != 0) {
                while (anim.GetFloat("speed") != 0) {
                    anim.SetFloat("speed", Mathf.Clamp(anim.GetFloat("speed") - acelerationSpeed, 0f, 1f));
                    yield return new WaitForEndOfFrame();
                }

            }
        }
    }
}
