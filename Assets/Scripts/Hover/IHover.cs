using System.Collections.Generic;

namespace ElJardin.Hover
{
    public interface IHover
    {
        int size { get; set; }
        List<Node> hoveredNodesCache { get; set; }
        void Show(Node targetNode);
        void Hide();
    }
}