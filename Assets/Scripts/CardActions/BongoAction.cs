using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElJardin.CardActions
{
    public class BongoAction : BaseCardAction
    {
        string obstacleTag = "Rock";
        public BongoAction(int size) : base(size)
        {
        }

        public override void DoAction(Node targetNode)
        {
            CreateBongo(targetNode);
            DestroySurroundingGround(targetNode);
        }

        void CreateBongo(Node position)
        {
            var bongoInstance = Object.Instantiate(insectPrefab, position.transform) as GameObject;
            bongoInstance.GetComponentInChildren<Animator>().Play("PlayBongo0");
        }
        
        IEnumerator PlayBongos(Node targetNode)
        {
            insectPrefab.GetComponentInChildren<Animator>().Play("PlayBongo0");
            //AkSoundEngine.PostEvent("Hormiga_Ataque_In", gameObject);

            DestroySurroundingGround(targetNode);
            yield return null;
        }

        void TryDestroyObstacle(Node node)
        {
           DestroyChildWithTag(node.gameObject, "obstacle");
        }
        
        void DestroyChildWithTag(GameObject parent, string tag)
        {
            var childCount = parent.transform.childCount;
            var index = 0;
            var safeLock = 100;
            while(index < childCount && safeLock > 0)
            {
                safeLock -= 1;
                var child = parent.transform.GetChild(index);
                if(child.CompareTag(tag))
                {
                    if(child.transform.GetChild(0).CompareTag(obstacleTag))
                    {
                        //node.destroyObstacle();
                        childCount = parent.transform.childCount;
                    }
                    else
                        index += 1;
                }
                else
                    index += 1;
            }
        }
        
        void DestroySurroundingGround(Node node)
        {
            var listDestroyNodesSquare = new List<Node>();

            //North
            var northNode = MapManager.Instance?.GetNode(node.row, node.column + 1);
            if(northNode != null)
            {
                if(!northNode.IsGround())
                    listDestroyNodesSquare.Add(northNode);
                else
                    TryDestroyObstacle(northNode);
            }

            //North West
            var northWestNode = MapManager.Instance?.GetNode(node.row -1, node.column + 1);
            if(northWestNode != null)
            {
                if(!northWestNode.IsGround())
                    listDestroyNodesSquare.Add(northWestNode);
                else
                    TryDestroyObstacle(northWestNode);
            }

            //North East
            var northEastNode = MapManager.Instance?.GetNode(node.row +1, node.column + 1);
            if(northEastNode != null)
            {
                if(!northEastNode.IsGround())
                    listDestroyNodesSquare.Add(northEastNode);
                else
                    TryDestroyObstacle(northEastNode);
            }

            //South
            var southNode = MapManager.Instance?.GetNode(node.row, node.column - 1);
            if(southNode != null)
            {
                if(!southNode.IsGround())
                    listDestroyNodesSquare.Add(southNode);
                else
                    TryDestroyObstacle(southNode);
            }

            //South West
            var southWestNode = MapManager.Instance?.GetNode(node.row -1, node.column - 1);
            if(southWestNode != null)
            {
                if(!southWestNode.IsGround()) 
                    listDestroyNodesSquare.Add(southWestNode);
                else
                    TryDestroyObstacle(southWestNode);
            }

            //South East
            var southEastNode = MapManager.Instance?.GetNode(node.row +1, node.column - 1);
            if(southEastNode != null)
            {
                if(!southEastNode.IsGround()) 
                    listDestroyNodesSquare.Add(southEastNode);
                else
                    TryDestroyObstacle(southEastNode);
            }

            //East
            var eastNode = MapManager.Instance?.GetNode(node.row + 1, node.column);
            if(eastNode != null)
            {
                if(!eastNode.IsGround())
                    listDestroyNodesSquare.Add(eastNode);
                else
                    TryDestroyObstacle(eastNode);
            }

            //West
            var westNode = MapManager.Instance?.GetNode(node.row - 1,node.column);
            if(westNode != null)
            {
                if(!westNode.IsGround()) 
                    listDestroyNodesSquare.Add(westNode);
                else
                    TryDestroyObstacle(westNode);
            }

            foreach(var nodeToDestroy in listDestroyNodesSquare)
            {
                BuildManager.Instance?.BuildGround(nodeToDestroy);
            }
        }
        
    }
}