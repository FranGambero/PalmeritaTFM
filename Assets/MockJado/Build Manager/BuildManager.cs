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

        [Header("Test Cards")]
        public int amount;
        public DirectionType direction;

        private List<Node> nodesToBuild;

        #endregion

        #region Build

        public void BuildGroove(Node node)
        {
            if(node.IsGround())
                node.ChangeNodeType(NodeType.Water, waterMat);
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
                foreach( Node node in nodesToBuild){
                    node.ChangeNodeType(NodeType.Water, waterMat);
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