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
        public bool Hovering { get; set; }

        Material currentMaterial;
        NodeType nodeType;

        MeshRenderer _mr;
        MeshFilter _mf;
        Color baseColor;

        bool canBuild, hasPreviewOn, indestructible;
        public Water water;
        private DryController _dryController;

        public int GCost { get; set; }
        public int HCost { get; set; }
        public int FCost { get; set; }
        public Node CameFromNode { get; set; }
        public DryController dryController {
            get {
                if (!_dryController) {
                    _dryController = GetComponent<DryController>();
                }
                return _dryController;
            }
        }

        public bool IsStatic {
            get {
                return this.Indestructible == true &&
this.canBuild == false;
            }
        }

        public bool CanBuild => canBuild;

        public DirectionType directionInHover;

        public GameObject obstacle;
        #endregion

        #region Accessors
        public bool HasObstacle => obstacle != null;
        public bool IsWalkable => obstacle == null && nodeType != NodeType.Water;

        public bool Indestructible { get => indestructible; set => indestructible = value; }
        #endregion

        private void Awake() {
            nodeType = NodeType.Undefined;
            directionInHover = DirectionType.Undefined;

            _mr = GetComponentInChildren<MeshRenderer>();
            _mf = GetComponentInChildren<MeshFilter>();

            Hovering = false;

            canBuild = true;
        }

        private void Start() {
            water = GetComponentInChildren<Water>();
            baseColor = _mr.material.color;
        }

        public void ChangeNodeType(NodeType newType, Material newMaterial = null) {
            nodeType = newType;
            if (newMaterial)
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
            Hovering = true;
            hasPreviewOn = true;
            directionInHover = direction;
        }

        public void HoverOn()
        {
            _mr.material.color = hoverColor;
            Hovering = true;
        }

        public void HoverOff() {
            Hovering = false;
            directionInHover = DirectionType.Undefined;
            hasPreviewOn = false;
            if (!dryController.active) {
                _mr.material.color = baseColor;
            }
        }

        /**
         * 
         */
        public void ShowPreview(bool show) {
            if (show) {
                hasPreviewOn = show;
                _mr.material.color = new Color32(0xF3, 0xE3, 0x26, 0xff);
                VFXDirector.Instance.Play("OnPreview", this.transform);
            } else if (Hovering) {
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
            if (GameManager.Instance.CanPlay) {
                if(GameManager.Instance.selectedCard != null)
                    GameManager.Instance.selectedCard.HoverOnNodeEnter(this);
                //BuildManager.Instance.ShowNodesPreview(this.directionInHover);
                if (Hovering) {
                    GameManager.Instance.SelectedNode = this;

                    //TODO: ShowCantBuildPreview
                } else if (!GameManager.Instance.draggingCard && this.CanBuild && this.IsWalkable) {
                    if (GameManager.Instance.Sepalo.CurrentNode != this)
                        PositionMoveHover();
                }
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

        public Vector3 GetSurfacePosition() {
            return new Vector3(this.transform.position.x,
                this.transform.position.y + .63f,
                this.transform.position.z);
        }

        /**
         * Si esta en hover (por carta) quita la preview (ahora en negro)
         * Si no, quita el hover de pasitos
         */
        private void OnMouseExit() {
            if (Hovering) {
                if(GameManager.Instance.selectedCard != null)
                    GameManager.Instance.selectedCard.UnHover();
                GameManager.Instance.SelectedNode = null;
                // BuildManager.Instance.dictionaryNodesAround[directionInHover].ForEach(n => n.ShowPreview(false));
            } else {
                // ShowPreview(false);
                GameManager.Instance.PositionHover.SetActive(false);
            }
        }
        #endregion

        #region Builder
        private void OnMouseUp() {
            if (GameManager.Instance.CanPlay) {
                GameManager.Instance.Sepalo.DoTheMove(this);
                //TODO: preguntar, esto no hace nada
                if (EventSystem.current.IsPointerOverGameObject())
                    return;
            }
        }
        public void MakeStatic() {
            this.Indestructible = true;
            this.canBuild = false;
        }
        #endregion

        #region Neighbors
        public bool IsGround() {
            return nodeType == NodeType.Ground ? true : false;
        }

        public void CalculateNeighbors() {
            this.neighbors = new List<Node>();

            List<Vector2> positionList = new List<Vector2>();
            List<Node> tmpNeighbors = new List<Node>();
            //Sacamos lista de posibles vecinos en cruz (i+1,j // i-1,j // i,j+1 // i,j-1)

            positionList.Add(new Vector2(row + 1, column));
            positionList.Add(new Vector2(row - 1, column));
            positionList.Add(new Vector2(row, column + 1));
            positionList.Add(new Vector2(row, column - 1));


            //Comprobamos que los vecinos sean validos
            foreach (Vector2 pos in positionList) {
                if (BuildManager.Instance.CheckValidNode((int)pos.x, (int)pos.y)) {
                    tmpNeighbors.Add(MapManager.Instance.GetNode((int)pos.x, (int)pos.y));
                    foreach (Node neighbor in tmpNeighbors) {
                        if (!neighbor.neighbors.Contains(this))
                            neighbor.neighbors.Add(this);
                    }
                }
            }
            this.neighbors.AddRange(tmpNeighbors);
        }

        public List<Node> GetListNeighbors() {
            List<Node> listNeighbors = new List<Node>();

            List<Vector2> positionList = new List<Vector2>();
            //Sacamos lista de posibles vecinos en cruz (i+1,j // i-1,j // i,j+1 // i,j-1)

            positionList.Add(new Vector2(row + 1, column));
            positionList.Add(new Vector2(row - 1, column));
            positionList.Add(new Vector2(row, column + 1));
            positionList.Add(new Vector2(row, column - 1));


            //Comprobamos que los vecinos sean validos
            foreach (Vector2 pos in positionList) {
                if (BuildManager.Instance.CheckNodeInMatrix((int)pos.x, (int)pos.y)) {
                    listNeighbors.Add(MapManager.Instance.GetNode((int)pos.x, (int)pos.y));
                    //this.neighbors.Add(MapManager.Instance.GetNode((int)pos.x, (int)pos.y));
                    //foreach (Node neighbor in listNeighbors) {
                    //    if (!neighbor.neighbors.Contains(this))
                    //        neighbor.neighbors.Add(this);
                    //}
                }
            }

            return listNeighbors;
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
        public void WaterNeighbors() {
            this.neighbors.ForEach(n => n.Water(this));
        }
        public void DryNeighbors() {
            this.neighbors.ForEach(n => n.Dry());
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
                AdminDryScript(false);
            }
        }

        [ContextMenu("Dry")]
        public void Dry() {
            water.Grow(false, () => neighbors.ForEach(n => n.Dry()), null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="add">add or remove to the semaphore</param>
        /// <param name="newNodeIndex">new index in the semaphore</param>
        public void AdminDryScript(bool add, int newNodeIndex = -1, bool resetGround = false) {
            if (add && !Indestructible) {
                if (!dryController.active && newNodeIndex != -1) {
                    dryController.initDry(newNodeIndex);
                }
            } else {
                if (dryController.active) {
                    StopDryComponent(resetGround);
                }
            }
        }

        public void StopDryComponent(bool resetGround) {
            if (dryController.active) {
                dryController.StopDry(resetGround);
            }
        }

        public void ResetMat() {
            _mr.material = (row + column) % 2 == 0 ? MapManager.Instance.groundMat : MapManager.Instance.groundMatOscurecio;
        }
        #endregion

        #region Obstacles
        public void SetObstacle(GameObject obstacle) {
            this.obstacle = obstacle;
        }

        public void DestroyObstacle() {
            Destroy(obstacle);
            this.obstacle = null;
        }
        #endregion

        #region regionDeMierda
        [ContextMenu("FUERZA REGIONES CARLA")]

        public void TestAutoConvertGround() {
            BuildManager.Instance.BuildGround(this);
        }

        #endregion
    }
}