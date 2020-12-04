using System.Collections;
using System.Collections.Generic;
using ElJardin.Util;
using UnityEngine;

namespace ElJardin {
    public class MapManager : Singleton<MapManager> {
        #region Variables

        public GameObject victoryCanvas;
        public GameObject cardCanvas;
        public GameObject confety;
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
        #endregion

        private void Awake() {
            mapMatrix = new GameObject[rows, columns];
            levelEnded = false;
        }

        private void Start() {
            InitializeMap();
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
            CreateStartEndPoints();
        }

        private void CreateStartEndPoints() {
            // Start point
            if (riverStartPos != riverEndPos) {
                mapMatrix[(int)riverStartPos.x, (int)riverStartPos.y].GetComponent<Node>().ChangeNodeType(NodeType.Water, BuildManager.Instance.pond_m);
                startingNode = mapMatrix[(int)riverStartPos.x, (int)riverStartPos.y].GetComponent<Node>();
                startingNode.SetColor(Color.blue);
                mapMatrix[(int)riverEndPos.x, (int)riverEndPos.y].GetComponent<Node>().ChangeNodeType(NodeType.Groove, BuildManager.Instance.pond_m);
                endingNode = mapMatrix[(int)riverEndPos.x, (int)riverEndPos.y].GetComponent<Node>();
                endingNode.SetColor(Color.red);
            } else {
                Debug.LogError("Start and end positions cant be the same");
            }
        }

        public Node GetNode(int row, int column) {
            return mapMatrix[row, column].GetComponent<Node>();
        }

        #region Victory
        private void CheckWin(Node node) {
            winCheckedNodes.Add(node);
            Debug.Log("Node: " + node.name + " // Ending node: " + endingNode.name);
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
            if (levelEnded)
            {
                // AkSoundEngine.PostEvent("Amb_Base_Out", gameObject);
                AudioManager.Instance.unSetAmbientMusic();
                CheckLogros();
                // Quizá hay que desactivar antes los otros canvas (cartas y mazo)???
                //victoryCanvas.SetActive(true);
                //confety.SetActive(true);
            }
        }

        [ContextMenu("FUERZA LOGROS CARLA")]
        private void CheckLogros() {
            //El logro de victoria lo seteamos a true si o si al ganar
            int currentZone = MapamundiManager.Instance.currentZone;
            int currentLevel = PlayerPrefs.GetInt("CurrentLevel");
            LevelData levelData = MapamundiManager.Instance.GetCurrentLevel(currentZone, currentLevel);

            // Logritos
            levelData.logros[0].done = true;   // El logro completado basico siempre se cumple por terminar el nivel
            levelData.logros[1].done = CheckLogroPetalos();
            levelData.logros[2].done = CheckLogroMovimientos();

            MapamundiManager.Instance.SaveLevel(levelData);
            cardCanvas.SetActive(false);
            victoryCanvas.SetActive(true);
            ActivateLogrosPanel(currentLevel);

        }

        private bool CheckLogroPetalos() {
            // Comprobamos que todas las flores de ese nivel están siendo regafas
            return false;
        }

        private bool CheckLogroMovimientos() {
            // Comprobamos que se ha completado en menos de X movimientos
            return false;
        }

        private void ActivateLogrosPanel(int currentLevel) {
            LogrosPanel logrosPanel = FindObjectOfType<LogrosPanel>();
            // Quiza puede ser directamente el activar los ticks pasando el leveldata
            logrosPanel.GetLogritos(currentLevel);
        }

        #endregion

        #endregion

    }
}
