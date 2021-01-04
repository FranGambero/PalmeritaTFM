using UnityEngine;

namespace ElJardin.CardActions
{
    public interface ICardAction
    {
        int size { get; set; }
        GameObject insectPrefab { get; set; }
        void DoAction(Node targetNode);
    }
}