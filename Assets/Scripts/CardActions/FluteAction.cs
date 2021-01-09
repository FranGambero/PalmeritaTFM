using System;
using ElJardin.Characters;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ElJardin.CardActions
{
    public class FluteAction : BaseCardAction
    {
        public FluteAction(int size) : base(size)
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
                    Debug.Log($"ShittyFlute - {targetNode.name}");

                    onCardUsed.Invoke(true);

                    CreateFlute(targetNode);
                }
            }
        }
        
        #region Action
        void CreateFlute(Node node)
        {
            var fluteInstance = Object.Instantiate(insectPrefab, node.transform.position + new Vector3(0,0.7f,0), Quaternion.identity ) as GameObject;
            fluteInstance.GetComponent<FluteAfterAction>().onAnimationEnd.AddListener(()=>BurnCurrentGround(node));
            fluteInstance.GetComponent<FluteAfterAction>().PlayFlute();
        }

        void BurnCurrentGround(Node node)
        {
            //node.DestroyObstacle();
            GameManager.Instance.BurnController.BurnNode(node);
            
            onActionCompleted.Invoke(true);
        }
        #endregion
    }
}