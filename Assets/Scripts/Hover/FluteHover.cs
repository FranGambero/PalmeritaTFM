using UnityEngine;

namespace ElJardin.Hover
{
    public class FluteHover : BaseHover
    {
        public FluteHover(int size) : base(size)
        {
        }

        public override void HoverOnGrab()
        {
            //La flauta no hace nada on grab
        }

        public override void HoverOnNodeEnter(Node targetNode)
        {
            BuildManager.Instance.GenericHoverStart(()=>MouseNodeHover(targetNode));
        }

        public override void Hide()
        {
            GameManager.Instance.SelectedNode = null;
            BuildManager.Instance.UnHoverNodesInList(hoveredNodesCache);
            hoveredNodesCache.Clear();
        }
        
        #region On Node Enter

        void MouseNodeHover(Node node)
        {
            if(NodeHasBramble(node))
            {
                node.Hovering = true;
                Hide();

                GameManager.Instance.SelectedNode = node;

                hoveredNodesCache.Add(node);

                BuildManager.Instance.HoverNodesInList(hoveredNodesCache);
            }

        }

        bool NodeHasBramble(Node node)
        {
            if (node.HasObstacle && node.obstacle.gameObject.CompareTag("obstacle"))
            {
                if(node.obstacle.transform.GetChild(0).gameObject.CompareTag("Burnable"))
                    return true;
            }
            return false;
        }
        
        #endregion
    }
}