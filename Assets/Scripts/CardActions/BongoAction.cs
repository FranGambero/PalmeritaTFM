using System.Collections;
using System.Collections.Generic;
using ElJardin.Characters;
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
            if(targetNode == null)
                onActionCompleted.Invoke(false);
            else
            {
                if(targetNode != GameManager.Instance.Sepalo.CurrentNode)
                {
                    onCardUsed.Invoke(true);

                    CreateBongo(targetNode);
                    //DestroySurroundingGround(targetNode);
                    onActionCompleted.Invoke(true);
                } else {
                    onCardUsed.Invoke(false);
                    onActionCompleted.Invoke(false);
                }
            }
        }

        void CreateBongo(Node position)
        {
            var bongoInstance = Object.Instantiate(insectPrefab, position.transform.position + new Vector3(0,0.7f,0), Quaternion.identity ) as GameObject;
            //bongoInstance.GetComponentInChildren<Animator>().Play("PlayBongo0");
            
            bongoInstance.GetComponent<BongormigaAfterAction>().onAnimationEnd.AddListener(()=>DestroySurroundingGround(position));
            bongoInstance.GetComponent<BongormigaAfterAction>().PlayBongos();
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
            Debug.Log($"Try destroy obstacles");
            foreach(Transform childTransform in parent.transform)
            {
                //Si tiene obstaculo
                if(childTransform.gameObject.CompareTag(tag))
                {
                    Debug.Log($" ----- object is obstacle");
                    foreach(Transform possibleObstacleTransform in childTransform)
                    {
                        if(possibleObstacleTransform.gameObject.CompareTag(obstacleTag))
                        {
                            Debug.Log($" ----- obstacle is rock");
                            parent.GetComponent<Node>()?.DestroyObstacle();
                            VFXDirector.Instance.Play("DestroyRock",parent.GetComponent<Node>().GetSurfacePosition());
                        }
                    }
                }
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

            //South
            var southNode = MapManager.Instance?.GetNode(node.row, node.column - 1);
            if(southNode != null)
            {
                if(!southNode.IsGround())
                    listDestroyNodesSquare.Add(southNode);
                else
                    TryDestroyObstacle(southNode);
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