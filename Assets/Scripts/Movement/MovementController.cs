using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ElJardin.Characters;
using UnityEngine;

namespace ElJardin.Movement {
    public class MovementController : MonoBehaviour {
        #region SerializedFields
        [SerializeField] float speed;
        [SerializeField] float lookAtSpeed;
        [SerializeField] float YOffset;
        public Node globalTargetNode;
        public Node globalStartingNode;
        #endregion

        IEnumerable<Node> CalculatePath(Node startingNode, Node destinyNode) {
            return MapManager.Instance.Pathfinding.FindPath(startingNode.row, startingNode.column, destinyNode.row, destinyNode.column);
        }

        IEnumerator MoveToNode(Node node) {
            var nodePosition = node.gameObject.transform.position + new Vector3(0, YOffset, 0);
            Vector3 relativePos = nodePosition - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            //While distance to target is larger than speed, move to target
            while (Vector3.Distance(transform.position, nodePosition) > speed * Time.deltaTime) {
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, lookAtSpeed * Time.deltaTime);
                //transform.LookAt(nodePosition);
                GameManager.Instance.Sepalo.CheckGrownd();
                transform.position = Vector3.MoveTowards(transform.position, nodePosition, speed * Time.deltaTime);
                yield return 0;
            }
            //Close enough to target, teleport there and stop coroutine
            transform.position = nodePosition;
        }


        IEnumerator MovePartialPath(IEnumerable<Node> path, int times, System.Action<Node> callback) {
            var pathList = path.ToList();
            for (var i = 0; i < times && i < pathList.Count; i++) {
                yield return StartCoroutine(MoveToNode(pathList[i]));
                callback(pathList[i]);
            }
        }

        public IEnumerator Move(Node startingNode, Node targetNode, SepaloController sepalo) {
            this.globalTargetNode = targetNode;
            this.globalStartingNode = startingNode;
            var path = CalculatePath(startingNode, targetNode);
            foreach (var node in path) {
                if (targetNode == this.globalTargetNode) {
                    yield return StartCoroutine(MoveToNode(node));
                    sepalo.CurrentNode = node;
                } else {
                    break;
                }
            }
        }

        public IEnumerator MovePartial(Node startingNode, Node targetNode, int timesToMove, System.Action<Node> callback) {
            var path = CalculatePath(startingNode, targetNode);
            yield return StartCoroutine(MovePartialPath(path, timesToMove, callback));
        }
    }
}