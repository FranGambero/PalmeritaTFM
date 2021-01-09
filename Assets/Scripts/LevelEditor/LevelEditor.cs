using UnityEngine;

namespace ElJardin
{
    [System.Serializable]
    public class LevelEditor : MonoBehaviour
    {
        [SerializeField]public int rows;
        [SerializeField]public int columns;
        [SerializeField]public GameObject[] nodeMatrixFlattened;
        
        public float tileOffset;

        public GameObject nodeRepository;
        
        public GameObject editorNode;

       
        #region Matrix Support Methods
        public void InitMatrix()
        {
            nodeMatrixFlattened = new GameObject[columns * rows];
        }
        #endregion
    }
}