using System;
using UnityEditor;
using UnityEngine;

namespace ElJardin
{
    [CustomEditor(typeof(LevelEditor))]
    public class LevelEditorGUI : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var levelEditor = (LevelEditor) target;

            if(GUILayout.Button("Generate Base Map"))
            {
                GenerateBaseMap(levelEditor);
            }

            if(GUILayout.Button("Update Map"))
            {
                UpdateAllNodesInMap(levelEditor);
                Debug.Log("Map Updated");
            }

            if(GUILayout.Button("Reset Map"))
            {
                ResetMap(levelEditor);
            }
        }

        #region Map Generation
        void GenerateBaseMap(LevelEditor levelEditor)
        {
            levelEditor.InitMatrix();

            ResetMap(levelEditor);

            for(var i = 0; i < levelEditor.rows; i++)
            {
                for(var j = 0; j < levelEditor.columns; j++)
                {
                    var newNode = Instantiate(levelEditor.editorNode, new Vector3(j, 0, i) * levelEditor.tileOffset, Quaternion.identity, levelEditor.gameObject.transform);
                    newNode.name = $"Node{i}_{j}";
                    newNode.transform.parent = levelEditor.nodeRepository.transform;
                    newNode.gameObject.GetComponent<NodeDataModel>().SetPosition(i, j);

                    levelEditor.nodeMatrixFlattened[FlattenMatrix(i, j, levelEditor.columns)] = newNode;
                }
            }
        }

        void UpdateAllNodesInMap(LevelEditor levelEditor)
        {
            var nodeEditorsParent = levelEditor.gameObject.transform.GetChild(0);
            foreach(Transform node in nodeEditorsParent)
            {
                var nodeDataModel = node.gameObject.GetComponent<NodeDataModel>();
                UpdateNode(nodeDataModel);
            }
        }

        void UpdateNode(NodeDataModel node)
        {
            if(!node.isRiverStart)
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
        #endregion

        #region Map Reset
        void ResetMap(LevelEditor levelEditor)
        {
            var parentTransform = levelEditor.nodeRepository.transform;
            while(parentTransform.childCount != 0)
            {
                DestroyImmediate(parentTransform.GetChild(0).gameObject);
            }
        }
        #endregion

        #region Support Methods
        int FlattenMatrix(int rowIndex, int columnIndex, int totalColumns)
        {
            return rowIndex + (columnIndex * totalColumns);
        }
        #endregion
    }
}