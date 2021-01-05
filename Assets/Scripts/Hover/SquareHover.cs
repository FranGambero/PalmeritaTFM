using System.Collections.Generic;

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

        public override void HoverOnNodeEnter(Node targetNode)
        {
            //Por seguridad se borran los hovers antiguos
            Hide();
            
            //Se hacen los hover alrededor del nodo, todas las posiciones
            for(var row = targetNode.row - size; row < targetNode.row + size; row++)
            {
                for(var column = targetNode.column - size; column < targetNode.column + size; column++)
                {
                    //Si se encuentra dentro del rango del mapa
                    if(row >= 0 && row < MapManager.Instance.rows && column >= 0 && column < MapManager.Instance.columns)
                    {
                        hoveredNodesCache.Add(MapManager.Instance.GetNode(row,column));
                    }
                }
            }
            
            BuildManager.Instance.HoverNodesInList(hoveredNodesCache);
        }
        
        public override void Hide()
        {
            BuildManager.Instance.UnHoverNodesInList(hoveredNodesCache);
            hoveredNodesCache.Clear();
        }
    }
}