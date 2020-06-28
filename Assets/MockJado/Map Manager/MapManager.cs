using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElJardin
{
    public class MapManager : Singleton<MapManager>
    {
        #region Variables

        /**
         * 
         */
        [Header("Matrix Size")]
        public int rows, columns;

        [Header("Map Correction")]
        public float tileOffset;

        /**
         * 
         */
        [Header("Tile Materials")]
        public Material groundMat;

        public Material waterMat;
        //public Material waterMatStr8, waterMatCurve, waterMatFork, waterMatCross, waterMatEnd;

        public Material grooveMat;
        //public Material grooveMatStr8, grooveMatCurve, grooveMatFork, grooveMatCross, grooveMatEnd;


        [Header("Node Prefab")]
        public GameObject nodePrefab;
        /**
         * 
         */
        [Header("Start/End")]
        public Vector2 riverStartPos;
        public Vector2 riverEndPos;
        public Vector2 playerStartPos;

        private GameObject[,] mapMatrix;
        #endregion

        private void Awake()
        {
            mapMatrix = new GameObject[rows,columns];
        }

        private void Start()
        {
            InitializeMap();
        }

        #region Matrix

        public void InitializeMap()
        {
            for(int i = 0; i<rows; i++)
            {
                for(int j = 0; j < columns; j++)
                {
                    mapMatrix[i,j] = Instantiate(nodePrefab, new Vector3(j,0,i)*tileOffset, Quaternion.Euler(-90,0,0), this.transform);
                    mapMatrix[i, j].GetComponent<Node>().ChangeNodeType(NodeType.Ground, groundMat);
                    mapMatrix[i, j].GetComponent<Node>().SetPosition(new Vector2(i,j));
                }
            }
        }

        public Node GetNode(int row, int column)
        {
            return mapMatrix[row, column].GetComponent<Node>();
        }

        #endregion

    }
}
