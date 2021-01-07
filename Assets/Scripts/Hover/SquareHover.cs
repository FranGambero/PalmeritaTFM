using System.Collections.Generic;
using UnityEngine;

namespace ElJardin.Hover
{
    public class SquareHover : BaseHover
    {
        public SquareHover(int size) : base(size)
        {
        }

        public override void HoverOnGrab()
        {
            //La hormiga no hace hover al agarrar la carta
        }

        public override void HoverOnNodeEnter(Node node)
        {
            BuildManager.Instance.GenericHoverStart(()=>CrossHover(node));
        }

        void CrossHover(Node node)
        {
            Debug.Log($"Hovering on Node {node.name}");
            node.Hovering = true;
            //Por seguridad se borran los hovers antiguos
            Hide();
            GameManager.Instance.SelectedNode = node;
            
            var listDestroyNodesSquare = new List<Node>();

            //North
            var northNode = MapManager.Instance?.GetNode(node.row, node.column + 1);
            if(northNode != null)
            {
                listDestroyNodesSquare.Add(northNode);
            }

            //South
            var southNode = MapManager.Instance?.GetNode(node.row, node.column - 1);
            if(southNode != null)
            {
                listDestroyNodesSquare.Add(southNode);
            }

            //East
            var eastNode = MapManager.Instance?.GetNode(node.row + 1, node.column);
            if(eastNode != null)
            {
                listDestroyNodesSquare.Add(eastNode);
            }

            //West
            var westNode = MapManager.Instance?.GetNode(node.row - 1,node.column);
            if(westNode != null)
            {
                listDestroyNodesSquare.Add(westNode);
            }

            hoveredNodesCache = listDestroyNodesSquare;

            BuildManager.Instance.HoverNodesInList(hoveredNodesCache);
        }
        
        public override void Hide()
        {
            Debug.Log("Hide");
            GameManager.Instance.SelectedNode = null;
            BuildManager.Instance.UnHoverNodesInList(hoveredNodesCache);
            hoveredNodesCache.Clear();
        }
    }
}