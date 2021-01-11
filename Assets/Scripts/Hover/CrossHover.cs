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
            HoverAroundSepalo();
        }

        public override void HoverOnNodeEnter(Node targetNode)
        {

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