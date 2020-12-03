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
            ResetMap(levelEditor);
            
            for(var i = 0; i < levelEditor.rows; i++)
            {
                for(var j = 0; j < levelEditor.columns; j++)
                {
                    var node = Instantiate(levelEditor.editorNode, new Vector3(j, 0, i) * levelEditor.tileOffset, Quaternion.identity, levelEditor.gameObject.transform);
                    node.name = $"Node{i}_{j}";
                    node.transform.parent = levelEditor.nodeRepository.transform;
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
    }
}