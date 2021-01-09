using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ElJardin.Util;
using UnityEngine;

namespace ElJardin {
    public class MapManager : Singleton<MapManager> {
        #region Variables
        [Header("Injection")] public LevelEditor levelEditor;

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
        public Material groundMatOscurecio;
        public Material peligro1, peligro2, peligro3;

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
        private Node startingNode;
        private Node endingNode;

        private bool levelEnded;
        private List<Node> winCheckedNodes;


        [Header("Injection")]
        [HideInInspector]
        public Pathfinding Pathfinding;

        public FlowerManager flowerManager;
        #endregion

        private void Awake() {
            mapMatrix = new GameObject[rows, columns];
            levelEnded = false;
        }

        private void Start() {
            if (!levelEditor) {
                InitializeMap();
            } else {
                InitializeMapFromEditor();
            }
            Pathfinding = new Pathfinding(this);
        }

        #region Matrix

        public void InitializeMap() {
            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < columns; j++) {
                    mapMatrix[i, j] = Instantiate(nodePrefab, new Vector3(j, 0, i) * tileOffset, Quaternion.identity, this.transform);
                    mapMatrix[i, j].GetComponent<Node>().ChangeNodeType(NodeType.Ground, (i + j) % 2 == 0 ? groundMat : groundMatOscurecio);
                    mapMatrix[i, j].GetComponent<Node>().SetPosition(new Vector2(i, j));
                    mapMatrix[i, j].gameObject.name = "Node" + i + "_" + j;
                }
            }
            CreateStartEndPoints(riverStartPos, riverEndPos);
        }

        void InitializeMapFromEditor() {
            var startPos = new List<Vector2>();
            var endPos = new List<Vector2>();

            for (var i = 0; i < levelEditor.rows; i++) {
                for (var j = 0; j < levelEditor.columns; j++) {
                    var nodeDataModel = levelEditor.nodeMatrixFlattened[FlattenMatrix(i, j, levelEditor.columns)].GetComponent<NodeDataModel>();
                    mapMatrix[i, j] = nodeDataModel.gameObject;
                    mapMatrix[i, j].GetComponent<Node>().ChangeNodeType(NodeType.Ground, (i + j) % 2 == 0 ? groundMat : groundMatOscurecio);
                    mapMatrix[i, j].GetComponent<Node>().SetPosition(new Vector2(nodeDataModel.Row, nodeDataModel.Column));

                    if (nodeDataModel.obstacle != NodeDataModel.ObstacleType.None)
                        mapMatrix[i, j].GetComponent<Node>().obstacle = nodeDataModel.obstacleGameObject;

                    if (nodeDataModel.isRiverStart) {
                        startPos.Add(new Vector2(nodeDataModel.Row, nodeDataModel.Column));
                    } else if (nodeDataModel.isRiverEnd) {
                        endPos.Add(new Vector2(nodeDataModel.Row, nodeDataModel.Column));
                    }
                }
            }

            TryCreateStartEndPoints(startPos, endPos);
        }

        int FlattenMatrix(int rowIndex, int columnIndex, int totalColumns) {
            return rowIndex + (columnIndex * totalColumns);
        }

        void TryCreateStartEndPoints(List<Vector2> listStarting, List<Vector2> listEnding) {
            if (listStarting.Count() == 1 && listEnding.Count == 1)
                CreateStartEndPoints(listStarting[0], listEnding[0]);
            else
                Debug.LogError($"Start/End points Error || Make sure there is just one of each");
        }

        private void CreateStartEndPoints(Vector2 startPos, Vector2 endPos) {
            // Start point
            if (startPos != endPos) {
                mapMatrix[(int)startPos.x, (int)startPos.y].GetComponent<Node>().ChangeNodeType(NodeType.Water, BuildManager.Instance.pond_m);
                startingNode = mapMatrix[(int)startPos.x, (int)startPos.y].GetComponent<Node>();
                startingNode.Water(true);
                startingNode.MakeStatic();
                startingNode.SetColor(Color.blue);
                mapMatrix[(int)endPos.x, (int)endPos.y].GetComponent<Node>().ChangeNodeType(NodeType.Groove, BuildManager.Instance.pond_m);
                endingNode = mapMatrix[(int)endPos.x, (int)endPos.y].GetComponent<Node>();
                endingNode.ChangeNodeType(NodeType.Water);
                endingNode.MakeStatic();
                endingNode.SetColor(Color.red);
            } else {
                Debug.LogError("Start and end positions cant be the same");
            }
        }

        public Node GetNode(int row, int column)
        {
            if(rows > row && row >= 0 && columns > column && column >= 0)
                return mapMatrix[row, column]?.GetComponent<Node>(); 
            
            return null;
        }

        #region Victory
        private void CheckWin(Node node) {
            winCheckedNodes.Add(node);

            if (node == endingNode) {
                levelEnded = true;
            } else {
                if (node.neighbors.Count > 0) {
                    foreach (Node neighbor in node.neighbors) {
                        if (!winCheckedNodes.Contains(neighbor))
                            CheckWin(neighbor);
                    }
                }
            }
        }

        public void CheckFullRiver() {
            winCheckedNodes = new List<Node>();
            CheckWin(startingNode);
            if (levelEnded) {
                StartCoroutine(WaitWaterToEnd());
            }
        }
        /// <summary>
        /// Waits til the ending node has water to end the level
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitWaterToEnd() {
            yield return new WaitUntil(() => endingNode.water.hasWater);
            GameManager.Instance.EndGame();
        }

        [ContextMenu("FUERZA LOGROS CARLA")]
        public void CheckLogros() {
            //El logro de victoria lo seteamos a true si o si al ganar
            int currentZone = MapamundiManager.Instance.currentZone;
            int currentLevel = PlayerPrefs.GetInt("CurrentLevel");
            LevelData levelData = MapamundiManager.Instance.GetCurrentLevel(currentZone, currentLevel);

            // Logritos
            levelData.logros[0].done = true;   // El logro completado basico siempre se cumple por terminar el nivel
            if(levelData.logros.Length >= 2)
                levelData.logros[1].done = levelData.logros[1].done ? true : CheckLogroPetalos();
            if (levelData.logros.Length >= 3)
                levelData.logros[2].done = levelData.logros[2].done ? true : CheckLogroMovimientos();

            // Ponemos a completado si tiene un logro al menos
            levelData.isCompleted = true;

            MapamundiManager.Instance.SaveLevel(levelData);

        }

        private bool CheckLogroPetalos() {
            // Comprobamos que todas las flores de ese nivel están siendo regadas
            if (!flowerManager)
                flowerManager = FindObjectOfType<FlowerManager>();
            return flowerManager.CountFlowerStatus();
        }

        private bool CheckLogroMovimientos() {
            return TurnsCounter.Instance.CheckResults();
        }



        #endregion

        #endregion

    }
}
