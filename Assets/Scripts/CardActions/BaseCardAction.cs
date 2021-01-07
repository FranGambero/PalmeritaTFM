using ElJardin.Util;
using UnityEngine;
using UnityEngine.Events;

namespace ElJardin.CardActions
{
    public abstract class BaseCardAction : ICardAction
    {
        public int size { get; set; }
        public GameObject insectPrefab { get; set; }
        public UnityEvent<bool> onActionCompleted { get; set; }

        protected BaseCardAction(int size)
        {
            this.size = size;
            onActionCompleted = new BoolUnityEvent();
        }
        
        public abstract void DoAction(Node targetNode);
    }
}