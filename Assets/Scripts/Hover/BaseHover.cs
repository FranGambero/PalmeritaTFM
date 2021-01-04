using System;
using System.Collections.Generic;

namespace ElJardin.Hover
{
    public abstract class BaseHover : IHover
    {
        public int size { get; set; }
        public List<Node> hoveredNodesCache { get; set; }

        protected BaseHover(int size)
        {
            this.size = size;
        }

        public abstract void HoverOnGrab();

        public abstract void HoverOnNodeEnter(Node targetNode);

        public void Hide()
        {
            BuildManager.Instance.UnHoverNodesInList(hoveredNodesCache);
            hoveredNodesCache.Clear();
        }
    }
}