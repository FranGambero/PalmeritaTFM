using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElJardin {
    public class CharacterController : MonoBehaviour {
        private Rigidbody myRb;
        private Vector3 firstPos, lastPos;
        public List<Vector3> listPos = new List<Vector3>();
        public float velocity = 6f;
        public float offsetDistance = .5f;

        private void Awake() {
            myRb = GetComponent<Rigidbody>();
        }

        public void MoveToPosition(Node destNode, Node lastNode) {
            listPos.Clear();
            firstPos = new Vector3(destNode.transform.position.x, transform.position.y, destNode.transform.position.z);
            listPos.Add(firstPos);

            if (lastNode != destNode) {
                lastPos = new Vector3(lastNode.transform.position.x, transform.position.y, lastNode.transform.position.z);
                listPos.Add(lastPos);
            }

            StartCoroutine(ReachPosition());
        }

        private IEnumerator ReachPosition() {
            int index = 0;
            Vector3 nextPos;

            while (index < listPos.Count) {
                nextPos = listPos[index];

                if (Vector3.Distance(transform.position, nextPos) > offsetDistance) {
                    transform.position = Vector3.MoveTowards(transform.position, nextPos, Time.deltaTime * velocity);
                    yield return new WaitForEndOfFrame();
                } else {
                    index++;
                }
            }


        }
    }
}
