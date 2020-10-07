using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ElJardin;
using UnityEngine;

namespace ElJardin.Movement
{
    public class MovementController : MonoBehaviour
    {
        #region SerializedFields
        [SerializeField] float speed;
        [SerializeField] float YOffset;
        #endregion

        IEnumerable<Node> CalculatePath(Node startingNode, Node destinyNode)
        {
            return MapManager.Instance.Pathfinding.FindPath(startingNode.row, startingNode.column, destinyNode.row, destinyNode.column);
        }

        IEnumerator MoveToNode(Node node)
        {
            var nodePosition = node.gameObject.transform.position+new Vector3(0,YOffset,0);
            //While distance to target is larger than speed, move to target
            while(Vector3.Distance(transform.position, nodePosition) > speed * Time.deltaTime)
            {
                transform.LookAt(nodePosition);
                transform.position = Vector3.MoveTowards(transform.position, nodePosition, speed * Time.deltaTime);
                yield return 0;
            }
            //Close enough to target, teleport there and stop coroutine
            transform.position = nodePosition;
        }

        IEnumerator MoveFullPath(IEnumerable<Node> path)
        {
            foreach(var node in path)
            {
                yield return StartCoroutine(MoveToNode(node));
            }
        }

        IEnumerator MovePartialPath(IEnumerable<Node> path, int times, System.Action<Node> callback)
        {
            var pathList = path.ToList();
            for(var i = 0; i < times && i<pathList.Count; i++)
            {
                yield return StartCoroutine(MoveToNode(pathList[i]));
                callback(pathList[i]);
            }
        }

        public IEnumerator Move(Node startingNode, Node targetNode)
        {
            var path = CalculatePath(startingNode, targetNode);
            yield return StartCoroutine(MoveFullPath(path));
        }

        public IEnumerator MovePartial(Node startingNode, Node targetNode, int timesToMove, System.Action<Node> callback)
        {
            var path = CalculatePath(startingNode, targetNode);
            yield return StartCoroutine(MovePartialPath(path, timesToMove, callback));
        }
    }
}