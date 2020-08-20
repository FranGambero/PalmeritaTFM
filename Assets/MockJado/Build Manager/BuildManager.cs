using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElJardin
{
    public class BuildManager : Singleton<BuildManager>
    {
        #region Variables

        [Header("Materials")]
        public Material waterMat;
        //public Material waterMatStr8, waterMatCurve, waterMatFork, waterMatCross, waterMatEnd;

        public Material grooveMat;
        //public Material grooveMatStr8, grooveMatCurve, grooveMatFork, grooveMatCross, grooveMatEnd;

        [Header("Meshs")]
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
        public CharacterController characterController;

        private List<Node> nodesToBuild, savedNodes;

        #endregion

        #region Build

        public void BuildGroove(Node node)
        {
            if(node.IsGround())
                node.ChangeNodeType(NodeType.Water, waterMat);
        }

        public void UpdateNeighbors(Node node)
        {
            foreach (Node neighbor in node.neighbors)
            {
                if (node != this)
                {
                    neighbor.ChangeNodeType(NodeType.Water, BuildManager.Instance.CalculateMeshToBuild(neighbor));
                    RotateMesh(neighbor);
                }
            }
        }

        private void RotateMesh(Node node)
        {
            Debug.Log("ROTATING MESH");

            switch (node.neighbors.Count)
            {
                case 1:
                    Debug.Log("Just one neigh");
                    Node neighbor = node.neighbors[0];
                    if(node.GetPosition().x > neighbor.GetPosition().x)
                    {
                        Debug.Log("conecto abajo");
                        node.gameObject.transform.rotation = Quaternion.Euler(0,180,0);
                    }else if (node.GetPosition().y > neighbor.GetPosition().y)
                    {
                        Debug.Log("conecto izquierda");
                        node.gameObject.transform.rotation = Quaternion.Euler(0,270,0);
                    } else if(node.GetPosition().y < neighbor.GetPosition().y)
                    {
                        Debug.Log("conecto derecha");
                        node.gameObject.transform.rotation = Quaternion.Euler(0,90,0);
                    }
                    break;

                case 2:
                    //Hay dos casos, puede ser pieza recta o pieza curva
                    //Pieza recta
                    if (node.neighbors[0].GetPosition().x == node.neighbors[1].GetPosition().x)
                    {
                        //pieza horizontal
                        node.gameObject.transform.rotation = Quaternion.Euler(0,90,0);
                    }
                    else if(node.neighbors[0].GetPosition().y == node.neighbors[1].GetPosition().y)
                    {
                        //pieza vertical
                        node.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    else
                    {
                        //Pieza curva
                        node.neighbors.Sort((n1, n2) => n1.GetPosition().x.CompareTo(n2.GetPosition().x));
                        if (node.neighbors[0].GetPosition().x < node.GetPosition().x)
                        {
                            //un nodo esta abajo, el otro derecha o izquierda
                            if (node.neighbors[1].GetPosition().y < node.GetPosition().y)
                            {
                                //abajo-izquierda
                                node.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                            }
                            else
                            {
                                //abajo-derecha
                                node.gameObject.transform.rotation = Quaternion.Euler(0, 270, 0);
                            }
                        } else if (node.neighbors[1].GetPosition().x > node.GetPosition().x)
                        {
                            //un nodo esta arriba, el otro derecha o izquierda
                            if (node.neighbors[0].GetPosition().y < node.GetPosition().y)
                            {
                                //arriba-izquierda
                                node.gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                            }
                            else
                            {
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
                    if(neighbor0.GetPosition().x == neighbor1.GetPosition().x)
                    {
                        //Es en horizontal, sale por arriba o por abajo
                        if (neighbor2.GetPosition().x > node.GetPosition().x)
                        {
                            //Sale por arriba
                            Debug.Log("h arriba");
                            node.gameObject.transform.rotation = Quaternion.Euler(0,90,0);
                        }
                        else
                        {
                            //Sale por abajo
                            Debug.Log("h abajo");
                            node.gameObject.transform.rotation = Quaternion.Euler(0, 270, 0);
                        }
                    }else if (neighbor1.GetPosition().x == neighbor2.GetPosition().x)
                    {
                        //Es en horizontal, sale por arriba o por abajo
                        if (neighbor0.GetPosition().x > node.GetPosition().x)
                        {
                            //Sale por arriba
                            Debug.Log("h arriba");
                            node.gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);
                        }
                        else
                        {
                            //Sale por abajo
                            Debug.Log("h abajo");
                            node.gameObject.transform.rotation = Quaternion.Euler(0, 270, 0);
                        }
                    }else if(neighbor1.GetPosition().y > node.GetPosition().y)
                    {
                        //Vertical sale derecha
                        node.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                    }
                    else
                    {
                        //Vertical sale izquierda
                        node.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    

                    break;
                default:
                    break;
            }
        }

        public Mesh CalculateMeshToBuild(Node node)
        {
            Mesh meshToBuild = canal_m;
            /*
            List<Vector2> positionList = new List<Vector2>();
            List<Node> neighbors = new List<Node>();
            Vector2 nodePos = node.GetPosition();
            //Sacamos lista de posibles vecinos en cruz (i+1,j // i-1,j // i,j+1 // i,j-1)

            positionList.Add(new Vector2(nodePos.x+1, nodePos.y));
            positionList.Add(new Vector2(nodePos.x-1, nodePos.y));
            positionList.Add(new Vector2(nodePos.x, nodePos.y+1));
            positionList.Add(new Vector2(nodePos.x, nodePos.y-1));


            //Comprobamos que los vecinos sean validos
            foreach(Vector2 pos in positionList)
            {
                if (CheckValidNode((int)pos.x, (int)pos.y))
                {
                    neighbors.Add(MapManager.Instance.GetNode((int)pos.x,(int)pos.y));
                    node.neighbors.Add(MapManager.Instance.GetNode((int)pos.x, (int)pos.y));
                }
            }
            */
            node.CalculateNeighbors();

            switch (node.neighbors.Count)
            {
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
                    if(node.neighbors[0].GetPosition().x == node.neighbors[1].GetPosition().x ||
                        node.neighbors[0].GetPosition().y == node.neighbors[1].GetPosition().y) { 

                        meshToBuild = canal_m;
                    }
                    else
                    {
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

        public bool CheckValidNode(int row, int column)
        {
            return ((row >= 0 && row < MapManager.Instance.rows && column >= 0 && column < MapManager.Instance.columns) 
                && (MapManager.Instance.GetNode(row,column).GetNodeType()!=NodeType.Ground)) ? true : false;
        }

        private void CreateChangeList(int row, int column)
        {
            if(row >=0 && row < MapManager.Instance.rows && column >=0 && column < MapManager.Instance.columns)
            {
                Node auxNode = MapManager.Instance.GetNode(row,column);

                if (auxNode.IsGround())
                {
                    //MapManager.Instance.GetNode(row, column).ChangeNodeType(NodeType.Water, waterMat);
                    nodesToBuild.Add(auxNode);
                }
            }
        }

        private bool IsChangeValid()
        {
            return (nodesToBuild.Count == amount) ? true : false;
        }

        public void ChangeNodesInList()
        {
            if (IsChangeValid())
            {
                /*
                foreach( Node node in nodesToBuild){
                    node.ChangeNodeType(NodeType.Water, canal_m);
                }
                */

                // Vamos a llamar al movimiento del personaje con el nodo una vez validado
                Debug.Log("Me llaman con lista " + nodesToBuild);
                characterController.MoveToPosition(nodesToBuild[0], nodesToBuild[nodesToBuild.Count - 1]);
                savedNodes = nodesToBuild;


                ////Correct mesh
                //foreach(Node node in nodesToBuild)
                //{
                //    node.ChangeNodeType(NodeType.Water, CalculateMeshToBuild(node));
                //    UpdateNeighbors(node);
                //    RotateMesh(node);
                //}

            }
        }

        public void construyeCasillas() {
            //Correct mesh
            foreach (Node node in savedNodes) {
                node.ChangeNodeType(NodeType.Water, CalculateMeshToBuild(node));
                UpdateNeighbors(node);
                RotateMesh(node);
            }
        }

        public void GetSurroundingsByCard(Node node)
        {
            nodesToBuild = new List<Node>();

            Vector2 position = node.GetPosition();

            switch (direction)
            {
                case DirectionType.North:
                    for (int i = (int)position.x; i < (int)position.x + amount; i++)
                    {
                        //MapManager.Instance.GetNode(i, (int)position.y).ChangeNodeType(NodeType.Water, waterMat);
                        CreateChangeList(i, (int)position.y);
                        //ChangeNodesInList();
                    }
                        break;
                case DirectionType.South:
                    for (int i = (int)position.x; i > (int)position.x - amount; i--)
                    {
                        CreateChangeList(i, (int)position.y);
                        //ChangeNodesInList();
                    }
                    break;
                case DirectionType.East:
                    for (int j = (int)position.y; j < (int)position.y + amount; j++)
                    {
                        CreateChangeList((int)position.x, j);
                        //ChangeNodesInList();
                    }
                    break;
                case DirectionType.West:
                    for (int j = (int)position.y; j > (int)position.y - amount; j--)
                    {
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

        public void HoverNodesInList()
        {
            foreach(Node node in nodesToBuild)
            {
                node.HoverOn();
            }
        }

        public void UnHoverNodesInList()
        {
            foreach (Node node in nodesToBuild)
            {
                node.HoverOff();
            }
        }

        #endregion
    }
}