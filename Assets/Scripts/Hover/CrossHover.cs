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
            Debug.Log($"CrossHover - Hovering node {targetNode.name}");
            BuildManager.Instance.ShowNodesPreview(targetNode.directionInHover);
        }

        public override void Hide()
        {
            
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
        #endregion
    }
}