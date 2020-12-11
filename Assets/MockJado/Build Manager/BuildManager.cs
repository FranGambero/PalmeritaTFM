using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ElJardin.Characters;
using UnityEngine;
using UnityEngine.Events;

namespace ElJardin {
    public class BuildManager : Singleton<BuildManager> {
        #region Variables

        [Header("Materials")]
        public Material waterMat;
        //public Material waterMatStr8, waterMatCurve, waterMatFork, waterMatCross, waterMatEnd;

        public Material grooveMat;
        //public Material grooveMatStr8, grooveMatCurve, grooveMatFork, grooveMatCross, grooveMatEnd;

        [Header("Meshs")]
        public Mesh ground_m;
        public Mesh canal_m;
        public Mesh elbow_m;
        public Mesh intersection3_m;
        public Mesh intersection4_m;
        public Mesh pond_m;
        public Mesh start_m;

        [Header("Test Cards")]
        public int amount;
        public DirectionType direction;

        [Header("Characters")]
        public SepaloController Sepalo;

        private List<Node> nodesToBuild, savedNodes;
        public Dictionary<DirectionType, List<Node>> dictionaryNodesAround;
        private Coroutine hoverCoroutine;

        #endregion

        #region Build

        public void BuildGroove(Node node) {
            if (node.IsGround())
                node.ChangeNodeType(NodeType.Water, waterMat);
        }

        public void BuildGround(Node node) {
            //Cambio nodo a tierra
            if (node.GetComponent<DryController>()) {
                node.RemoveDryComponent();
            }
            node.ChangeNodeType(NodeType.Ground, ground_m);
            node.water.Reset();
            node.DryNeighbors();//TODO Cambiar esto para que seque solo las que no vayan hasta la fuente
            //Correccion de vecinos
            UpdateNeighbors(node);
        }

        public bool UpdateNeighbors(Node node) {
            bool anyNeighborHasWater = false;
            foreach (Node neighbor in node.neighbors) {
                if (node != this) {
                    neighbor.ChangeNodeType(NodeType.Water, BuildManager.Instance.CalculateMeshToBuild(neighbor));
                    if (neighbor.water.IsActive() || neighbor.water.isGonnaHaveDaWote) {
                        node.PrepareWater(neighbor);
                        anyNeighborHasWater = true;
                    }
                    RotateMesh(neighbor);
                }
            }

            return anyNeighborHasWater;
        }

        private void RotateMesh(Node node) {
            Debug.Log("ROTATING MESH");

            switch (node.neighbors.Count) {
                case 1:
                    Debug.Log("Just one neigh");
                    Node neighbor = node.neighbors[0];
                    if (node.GetPosition().x > neighbor.GetPosition().x) {
                        Debug.Log("conecto abajo");
                        node.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                    } else if (node.GetPosition().y > neighbor.GetPosition().y) {
                        Debug.Log("conecto izquierda");
                        node.gameObject.transform.rotation = Quaternion.Euler(0, 270, 0);
                    } else if (node.GetPosition().y < neighbor.GetPosition().y) {
                        Debug.Log("conecto derecha");
                        node.gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                    }
                    break;

                case 2:
                    //Hay dos casos, puede ser pieza recta o pieza curva
                    //Pieza recta
                    if (node.neighbors[0].GetPosition().x == node.neighbors[1].GetPosition().x) {
                        //pieza horizontal
                        node.gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                    } else if (node.neighbors[0].GetPosition().y == node.neighbors[1].GetPosition().y) {
                        //pieza vertical
                        node.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    } else {
                        //Pieza curva
                        node.neighbors.Sort((n1, n2) => n1.GetPosition().x.CompareTo(n2.GetPosition().x));
                        if (node.neighbors[0].GetPosition().x < node.GetPosition().x) {
                            //un nodo esta abajo, el otro derecha o izquierda
                            if (node.neighbors[1].GetPosition().y < node.GetPosition().y) {
                                //abajo-izquierda
                                node.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                            } else {
                                //abajo-derecha
                                node.gameObject.transform.rotation = Quaternion.Euler(0, 270, 0);
                            }
                        } else if (node.neighbors[1].GetPosition().x > node.GetPosition().x) {
                            //un nodo esta arriba, el otro derecha o izquierda
                            if (node.neighbors[0].GetPosition().y < node.GetPosition().y) {
                                //arriba-izquierda
                                node.gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                            } else {
                                //arriba-derecha
                                node.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                            }
                        }

                    }


                    break;

                case 3:
                    Debug.Log("TRIPLETE");

                    node.neighbors.Sort((n1, n2) => n1.GetPosition().x.CompareTo(n2.GetPosition().x));
                    Node neighbor0 = node.neighbors[0];
                    Node neighbor1 = node.neighbors[1];
                    Node neighbor2 = node.neighbors[2];
                    if (neighbor0.GetPosition().x == neighbor1.GetPosition().x) {
                        //Es en horizontal, sale por arriba o por abajo
                        if (neighbor2.GetPosition().x > node.GetPosition().x) {
                            //Sale por arriba
                            Debug.Log("h arriba");
                            node.gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                        } else {
                            //Sale por abajo
                            Debug.Log("h abajo");
                            node.gameObject.transform.rotation = Quaternion.Euler(0, 270, 0);
                        }
                    } else if (neighbor1.GetPosition().x == neighbor2.GetPosition().x) {
                        //Es en horizontal, sale por arriba o por abajo
                        if (neighbor0.GetPosition().x > node.GetPosition().x) {
                            //Sale por arriba
                            Debug.Log("h arriba");
                            node.gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                        } else {
                            //Sale por abajo
                            Debug.Log("h abajo");
                            node.gameObject.transform.rotation = Quaternion.Euler(0, 270, 0);
                        }
                    } else if (neighbor1.GetPosition().y > node.GetPosition().y) {
                        //Vertical sale derecha
                        node.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                    } else {
                        //Vertical sale izquierda
                        node.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }


                    break;
                default:
                    break;
            }
        }

        public Mesh CalculateMeshToBuild(Node node) {
            Mesh meshToBuild = canal_m;
            node.CalculateNeighbors();

            switch (node.neighbors.Count) {
                case 0:
                    //pond
                    meshToBuild = pond_m;
                    break;

                case 1:
                    //start
                    meshToBuild = start_m;
                    break;
                case 2:
                    //hay dos opciones
                    if (node.neighbors[0].GetPosition().x == node.neighbors[1].GetPosition().x ||
                        node.neighbors[0].GetPosition().y == node.neighbors[1].GetPosition().y) {

                        meshToBuild = canal_m;
                    } else {
                        meshToBuild = elbow_m;
                    }
                    break;
                case 3:
                    //intersection3
                    meshToBuild = intersection3_m;
                    break;
                case 4:
                    //intersection4
                    meshToBuild = intersection4_m;
                    break;
                default:
                    Debug.LogError("Numero de vecinos inesperado");
                    break;
            }

            return meshToBuild;
        }

        public bool CheckValidNode(int row, int column) {
            return ((row >= 0 && row < MapManager.Instance.rows && column >= 0 && column < MapManager.Instance.columns)
                && (MapManager.Instance.GetNode(row, column).GetNodeType() != NodeType.Ground)
                && !MapManager.Instance.GetNode(row, column).HasObstacle);
        }

        private void CreateChangeList(int row, int column) {
            if (row >= 0 && row < MapManager.Instance.rows && column >= 0 && column < MapManager.Instance.columns) {
                Node auxNode = MapManager.Instance.GetNode(row, column);

                if (auxNode.IsGround() && auxNode.CanBuild) {
                    //MapManager.Instance.GetNode(row, column).ChangeNodeType(NodeType.Water, waterMat);
                    nodesToBuild.Add(auxNode);
                }
            }
        }

        private void CreateChangeList(int row, int column, List<Node> nodeList) {
            if (row >= 0 && row < MapManager.Instance.rows && column >= 0 && column < MapManager.Instance.columns) {
                Node auxNode = MapManager.Instance.GetNode(row, column);

                if (auxNode.IsGround() && auxNode.CanBuild) {
                    //MapManager.Instance.GetNode(row, column).ChangeNodeType(NodeType.Water, waterMat);
                    nodeList.Add(auxNode);
                }
            }
        }

        public void changeBuildValues(int newAmount, DirectionType newDirType) {
            amount = newAmount;
            direction = newDirType;
        }

        private bool IsChangeValid(List<Node> nodesToBuild) {
            return (nodesToBuild != null && nodesToBuild.Count == amount);
        }

        public bool ChangeNodesInList() {
            bool isValid = false;
            if (GameManager.Instance.SelectedNode != null) {
                if (!dictionaryNodesAround.ContainsKey(direction))
                    HoverAroundNode(amount);
                List<Node> nodesToBuild = GetNodeListByDirection(GameManager.Instance.SelectedNode.directionInHover);
                isValid = IsChangeValid(nodesToBuild);
                if (isValid) {
                    savedNodes = new List<Node>(nodesToBuild);
                    // Vamos a llamar al movimiento del personaje con el nodo una vez validado
                    Debug.Log("Me llaman con lista " + nodesToBuild);
                    buildCells();
                    //StartCoroutine(Sepalo.Move(nodesToBuild[0]));
                    //characterController.MoveToPosition(nodesToBuild[0], nodesToBuild[nodesToBuild.Count - 1]);
                }
            }
            return isValid;
        }

        public void buildCells() {
            //Correct mesh
            bool neighborWithWater = false;
            foreach (Node node in savedNodes) {
                bool thisNodeNeighborWithWater = false;
                node.ChangeNodeType(NodeType.Water, CalculateMeshToBuild(node));
                bool anyNeighborHasWater = UpdateNeighbors(node);

                if (anyNeighborHasWater) {
                    neighborWithWater = true;
                    thisNodeNeighborWithWater = true;
                }

                RotateMesh(node);
                if (thisNodeNeighborWithWater) {
                    node.DoPreparatedWater();
                }

            }
            if (!neighborWithWater && !(savedNodes.Count == 1 && (
                savedNodes[0].GetComponent<NodeDataModel>().isRiverStart || 
                savedNodes[0].GetComponent<NodeDataModel>().isRiverEnd))) {

                int newIndex = Semaphore.Instance.GetNewIndex();
                Debug.Log("Er new index " + newIndex);
                // Fran dice: Fran aqui falla
                savedNodes.ForEach(node => node.AdminDryScript(true, newIndex));
            } else {
                Debug.LogError("Quito dry");
                savedNodes.ForEach(node => node.water.isGonnaHaveDaWote = true);
                savedNodes.ForEach(node => node.AdminDryScript(false));
            }
            MapManager.Instance.CheckFullRiver();

        }

        public void GetSurroundingsByCard(Node node) {
            nodesToBuild = new List<Node>();

            Vector2 position = node.GetPosition();

            switch (direction) {
                case DirectionType.North:
                    for (int i = (int)position.x; i < (int)position.x + amount; i++) {
                        //MapManager.Instance.GetNode(i, (int)position.y).ChangeNodeType(NodeType.Water, waterMat);
                        CreateChangeList(i, (int)position.y);
                        //ChangeNodesInList();
                    }
                    break;
                case DirectionType.South:
                    for (int i = (int)position.x; i > (int)position.x - amount; i--) {
                        CreateChangeList(i, (int)position.y);
                        //ChangeNodesInList();
                    }
                    break;
                case DirectionType.East:
                    for (int j = (int)position.y; j < (int)position.y + amount; j++) {
                        CreateChangeList((int)position.x, j);
                        //ChangeNodesInList();
                    }
                    break;
                case DirectionType.West:
                    for (int j = (int)position.y; j > (int)position.y - amount; j--) {
                        CreateChangeList((int)position.x, j);
                        //ChangeNodesInList();
                    }
                    break;
                default:

                    break;
            }
        }

        #endregion

        #region Hover

        public void UnHoverNodesInList() {
            if (dictionaryNodesAround != null) {
                foreach (DirectionType dir in dictionaryNodesAround.Keys) {
                    UnHoverNodesInList(dictionaryNodesAround[dir]);
                }
            }
        }

        public void HoverNodesInList(List<Node> nodesAroundList, DirectionType newDirection) {
            foreach (Node node in nodesAroundList) {
                node.HoverOn(newDirection);
            }
        }

        public void UnHoverNodesInList(List<Node> nodesAroundList) {
            foreach (Node node in nodesAroundList) {
                node.HoverOff();
            }
            nodesAroundList.Clear();
        }

        public void HoverAroundNode(int numNodes) {
            StopHoverCoroutine();

            hoverCoroutine = StartCoroutine(HoverAroundNodeCoroutine(numNodes));
        }

        public void StopHoverCoroutine() {
            if (hoverCoroutine != null)
                StopCoroutine(hoverCoroutine);
            hoverCoroutine = null;
        }

        private IEnumerator HoverAroundNodeCoroutine(int numNodes) {
            SepaloController sepalo = GameManager.Instance.Sepalo;
            this.dictionaryNodesAround = new Dictionary<DirectionType, List<Node>>();
            yield return sepalo.movementCoroutine;
            DirectionType[] directions = new DirectionType[] { DirectionType.North, DirectionType.East, DirectionType.South, DirectionType.West };
            foreach (DirectionType directionToFill in directions) {

                dictionaryNodesAround[directionToFill] = GetSurroundingsByNode(
                    sepalo.CurrentNode,
                    directionToFill,
                    numNodes);
                if (dictionaryNodesAround[directionToFill].All(node => !node.HasObstacle))
                    HoverNodesInList(dictionaryNodesAround[directionToFill], directionToFill);

            }
        }

        public List<Node> GetSurroundingsByNode(Node node, DirectionType direction, int amount) {
            List<Node> nodesToBuilAround = new List<Node>();
            Vector2 position = node.GetPosition();

            switch (direction) {
                case DirectionType.North:
                    for (int i = (int)position.x + 1; i <= (int)position.x + amount; i++) {
                        //MapManager.Instance.GetNode(i, (int)position.y).ChangeNodeType(NodeType.Water, waterMat);
                        CreateChangeList(i, (int)position.y, nodesToBuilAround);
                        //ChangeNodesInList();
                    }
                    break;
                case DirectionType.South:
                    for (int i = (int)position.x - 1; i >= (int)position.x - amount; i--) {
                        CreateChangeList(i, (int)position.y, nodesToBuilAround);
                    }
                    break;
                case DirectionType.East:
                    for (int j = (int)position.y + 1; j <= (int)position.y + amount; j++) {
                        CreateChangeList((int)position.x, j, nodesToBuilAround);
                    }
                    break;
                case DirectionType.West:
                    for (int j = (int)position.y - 1; j >= (int)position.y - amount; j--) {
                        CreateChangeList((int)position.x, j, nodesToBuilAround);
                    }
                    break;
                default:

                    break;
            }

            return nodesToBuilAround;
        }

        private List<Node> GetNodeListByDirection(DirectionType direction) {
            List<Node> nodeList = null;
            if (dictionaryNodesAround != null) {
                Debug.Log("N Keys: " + dictionaryNodesAround.Keys);
                if (dictionaryNodesAround.ContainsKey(direction))
                    nodeList = dictionaryNodesAround[direction];
            }

            return nodeList;
        }

        #endregion
    }
}