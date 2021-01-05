using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ElJardin.Hover
{
    public class CrossHover : BaseHover
    {
        public CrossHover(int size) : base(size)
        {
        }

        public override void HoverOnGrab()
        {
            Debug.Log($"CrossHover - Hovering grab");
            HoverAroundSepalo();
        }

        public override void HoverOnNodeEnter(Node targetNode)
        {
            Debug.Log($"CrossHover - Hovering node");
        }

        public override void Hide()
        {
            BuildManager.Instance.StopHoverCoroutine();
            BuildManager.Instance.UnHoverNodesInList();
            //hoveredNodesCache.Clear();
        }

        #region Hover On Grab
        void HoverAroundSepalo()
        {
            HoverAround();
            BuildManager.Instance.changeBuildValues(size);
        }

        void HoverAround()
        {
            BuildManager.Instance.HoverAroundNode(size);
        }

        List<Node> CalculateNodesToHoverAroundSepalo()
        {
            var nodes = new List<Node>();
            var sepaloPosition = GameManager.Instance.Sepalo.CurrentNode;
            
            //North
            for(var column = sepaloPosition.column; column <= sepaloPosition.column + size; column++)
            {
                var auxNode = MapManager.Instance?.GetNode(sepaloPosition.row, column);
                if(auxNode != null)
                {
                    
                }
            }

            throw new NotImplementedException();
        }
        #endregion
    }
}