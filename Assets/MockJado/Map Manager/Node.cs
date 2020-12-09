using System.Collections.Generic;
using System.Linq;
using ElJardin.Movement;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ElJardin {
    public class Node : MonoBehaviour, IPathable {
        #region Variables
        [Header("Hover")] public Color hoverColor;

        [Header("Position")] public int row, column;

        //[HideInInspector]
        public List<Node> neighbors;

        Material currentMaterial;
        NodeType nodeType;

        MeshRenderer _mr;
        MeshFilter _mf;
        Color baseColor;

        bool hovering, canBuild;
        public Water water;

        public int GCost { get; set; }
        public int HCost { get; set; }
        public int FCost { get; set; }
        public Node CameFromNode { get; set; }

        public bool CanBuild => canBuild;

        public DirectionType directionInHover;

        public GameObject obstacle;
        #endregion

        #region Accessors
        public bool HasObstacle => obstacle != null;
        public bool IsWalkable => obstacle == null && nodeType != NodeType.Water;
        #endregion

        private void Awake() {
            nodeType = NodeType.Undefined;
            directionInHover = DirectionType.Undefined;

            _mr = GetComponentInChildren<MeshRenderer>();
            _mf = GetComponentInChildren<MeshFilter>();

            hovering = false;

            canBuild = true;
        }

        private void Start() {
            water = GetComponentInChildren<Water>();
            baseColor = _mr.material.color;
        }

        public void ChangeNodeType(NodeType newType, Material newMaterial) {
            nodeType = newType;
            _mr.material = newMaterial;
        }

        public void ChangeNodeType(NodeType newType, Mesh newMesh) {
            nodeType = newType;
            _mf.mesh = newMesh;
            CalculateNeighbors();
        }

        #region Setters
        public void SetPosition(Vector2 pos) {
            row = (int)pos.x;
            column = (int)pos.y;
        }

        public void SetColor(Color cl) {
            _mr.material.color = cl;
        }
        #endregion

        #region Getters
        public Vector2 GetPosition() {
            return new Vector2(row, column);
        }

        public NodeType GetNodeType() {
            return nodeType;
        }

        public Mesh GetMesh() {
            return _mf.mesh;
        }
        #endregion

        #region Hover
        public void HoverOn(DirectionType direction) {
            //baseColor = _mr.material.color;
            _mr.material.color = hoverColor;
            hovering = true;
            directionInHover = direction;
        }

        public void HoverOff() {
            _mr.material.color = baseColor;
            hovering = false;
            directionInHover = DirectionType.Undefined;
        }

        /**
         * 
         */
        public void ShowPreview(bool show) {
            if (show) {
                _mr.material.color = Color.black;
            } else if (hovering) {
                HoverOn(directionInHover);
            } else {
                HoverOff();
            }
        }

        /**
         * Si el nodo está en hove (por una carta), preview (de momento negra)
         * Si no carta y se puede construir, hover de pasitos
         */
        private void OnMouseEnter() {
            if (hovering) {
                GameManager.Instance.SelectedNode = this;
                var nodesToShowPreview = BuildManager.Instance.dictionaryNodesAround[directionInHover];
                if (nodesToShowPreview.All(node => !node.HasObstacle))
                    BuildManager.Instance.dictionaryNodesAround[directionInHover].ForEach(n => n.ShowPreview(true));

                //TODO: ShowCantBuildPreview
            } else if (!GameManager.Instance.draggingCard && this.CanBuild) {
                if (GameManager.Instance.Sepalo.CurrentNode != this)
                    PositionMoveHover();
            }
        }

        /**
         * Hover de pasitos que aparece en el nodo en el que se encuentra el raton
         * Se oculta si raton fuera de nodo && en casilla no walkable ( por ahora la de sepalo)
         */
        private void PositionMoveHover() {
            GameManager.Instance.PositionHover.SetActive(true);
            GameManager.Instance.PosPositionHover(new Vector3(
                this.transform.position.x,
                GameManager.Instance.PositionHover.transform.position.y,
                this.transform.position.z
            ));
        }

        /**
         * Si esta en hover (por carta) quita la preview (ahora en negro)
         * Si no, quita el hover de pasitos
         */
        private void OnMouseExit() {
            if (hovering) {
                GameManager.Instance.SelectedNode = null;
                BuildManager.Instance.dictionaryNodesAround[directionInHover].ForEach(n => n.ShowPreview(false));
            } else {
                ShowPreview(false);
                GameManager.Instance.PositionHover.SetActive(false);
            }
        }
        #endregion

        #region Builder
        private void OnMouseUp() {
            GameManager.Instance.Sepalo.DoTheMove(this);
            //TODO: preguntar, esto no hace nada
            if (EventSystem.current.IsPointerOverGameObject())
                return;
        }
        #endregion

        #region Neighbors
        public bool IsGround() {
            return nodeType == NodeType.Ground ? true : false;
        }

        public void CalculateNeighbors() {
            this.neighbors = new List<Node>();

            List<Vector2> positionList = new List<Vector2>();
            List<Node> neighbors = new List<Node>();
            //Sacamos lista de posibles vecinos en cruz (i+1,j // i-1,j // i,j+1 // i,j-1)

            positionList.Add(new Vector2(row + 1, column));
            positionList.Add(new Vector2(row - 1, column));
            positionList.Add(new Vector2(row, column + 1));
            positionList.Add(new Vector2(row, column - 1));


            //Comprobamos que los vecinos sean validos
            foreach (Vector2 pos in positionList) {
                if (BuildManager.Instance.CheckValidNode((int)pos.x, (int)pos.y)) {
                    neighbors.Add(MapManager.Instance.GetNode((int)pos.x, (int)pos.y));
                    this.neighbors.Add(MapManager.Instance.GetNode((int)pos.x, (int)pos.y));
                    foreach (Node neighbor in neighbors) {
                        if (!neighbor.neighbors.Contains(this))
                            neighbor.neighbors.Add(this);
                    }
                }
            }
        }
        #endregion

        #region Pathfinding
        public void CalculateFCost() {
            FCost = GCost + HCost;
        }
        #endregion

        #region Water
        [ContextMenu("Water")]
        public void Water() {
            water.Grow(true, () => neighbors.ForEach(n => n.Water(this)), null);
        }

        public void Water(Node last) {
            water.Grow(true, () => neighbors.ForEach(n => n.Water(this)), last);
        }
        public void PrepareWater(Node last) {
            water.PrepareGrow(true, () => neighbors.ForEach(n => n.Water(this)), last);
        } 
        public void DoPreparatedWater() {
            water.DoPreparatedGrow();
        }


        public void Water(bool ignoreNeighbor) {
            if (water == null) {
                water = GetComponentInChildren<Water>();
            }

            if (ignoreNeighbor) {
                water.Grow(true, null, null);
            } else {
                Water();
            }
        }

        [ContextMenu("Dry")]
        public void Dry() {
            water.Grow(false, () => neighbors.ForEach(n => n.Dry()), null);
        }

        public void AdminDryScript(bool add, int newNodeIndex = -1) {
            if (add) {
                if (!GetComponent<DryController>() && newNodeIndex != -1) {
                    DryController newDryController = gameObject.AddComponent<DryController>();
                    newDryController.initDry(newNodeIndex);
                    Semaphore.Instance.AddTurn(newDryController);
                }
            } else {
                if (GetComponent<DryController>()) {
                    Semaphore.Instance.RemoveTurn(GetComponent<DryController>().turnIndex);
                }
            }
        }
        #endregion

        #region Obstacles
        public void SetObstacle(GameObject obstacle) {
            this.obstacle = obstacle;
        }

        public void DestroyObstacle() {
            this.obstacle = null;
        }
        #endregion
    }
}