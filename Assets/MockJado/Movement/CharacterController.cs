using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElJardin {
    public class CharacterController : MonoBehaviour {
        private Rigidbody myRb;
        private Vector3 destination;
        public float velocity = 2f;
        public float offsetDistance = .2f;

        private void Awake() {
            myRb = GetComponent<Rigidbody>();
        }

        public void MoveToPosition(Node destNode) {
            destination = new Vector3(destNode.transform.position.x, transform.position.y, destNode.transform.position.z);
            StartCoroutine(ReachPosition());
        }

        private IEnumerator ReachPosition() {
            while (Vector3.Distance(transform.position, destination) > .2f) {
                transform.position = Vector3.MoveTowards(transform.position, destination, offsetDistance * velocity);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
