using System;
using UnityEditor;
using UnityEngine;

namespace ElJardin
{
    [CustomEditor(typeof(NodeDataModel))]
    public class NodeEditorGUI : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var node = (NodeDataModel) target;

            if(!node.isRiverStart)
            {
                if(GUILayout.Button("Update"))
                {
                    switch(node.obstacle)
                    {
                        case NodeDataModel.ObstacleType.None:
                            DestroyChildWithTag(node.gameObject, "obstacle");
                            break;
                        case NodeDataModel.ObstacleType.Rock:
                            GenerateObstacle(node.rockPrefab, node.transform, node);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }

        void DestroyChildWithTag(GameObject parent, string tag)
        {
            var childCount = parent.transform.childCount;
            var index = 0;
            var safeLock = 100;
            while(index < childCount && safeLock > 0)
            {
                safeLock -= 1;
                var child = parent.transform.GetChild(index);
                if(child.CompareTag(tag))
                {
                    DestroyImmediate(child.gameObject);
                    childCount = parent.transform.childCount;
                }
                else
                    index += 1;
            }
        }

        void GenerateObstacle(GameObject obstacle, Transform parent, NodeDataModel nodeDM)
        {
            var tag = "obstacle";
            DestroyChildWithTag(parent.gameObject, tag);
            
            var newObstacle = Instantiate(obstacle, parent);
            nodeDM.obstacleGameObject = newObstacle;
            newObstacle.tag = tag;
        }
    }
}