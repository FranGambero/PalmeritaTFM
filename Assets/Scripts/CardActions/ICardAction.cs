using UnityEngine;
using UnityEngine.Events;

namespace ElJardin.CardActions
{
    public interface ICardAction
    {
        int size { get; set; }
        GameObject insectPrefab { get; set; }
        UnityEvent<bool> onActionCompleted { get; set; }
        UnityEvent<bool> onCardUsed { get; set; }
        void DoAction(Node targetNode);
        
    }
}