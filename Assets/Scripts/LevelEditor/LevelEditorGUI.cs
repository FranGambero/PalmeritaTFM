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

            if(GUILayout.Button("Reset Map"))
            {
                ResetMap(levelEditor);
            }
        }

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

                    levelEditor.nodeMatrixFlattened[FlattenMatrix(i,j,levelEditor.columns)] = newNode;
                }
            }
        }
        
        void ResetMap(LevelEditor levelEditor)
        {
            var parentTransform = levelEditor.nodeRepository.transform;
            while(parentTransform.childCount != 0)
            {
                DestroyImmediate(parentTransform.GetChild(0).gameObject);
            }
        }
        
        #region Support Methods
        int FlattenMatrix(int rowIndex, int columnIndex, int totalColumns)
        {
            return rowIndex + (columnIndex * totalColumns);
        }
        #endregion
    }
}