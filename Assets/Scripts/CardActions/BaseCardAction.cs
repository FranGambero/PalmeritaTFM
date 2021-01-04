using UnityEngine;

namespace ElJardin.CardActions
{
    public abstract class BaseCardAction : ICardAction
    {
        public int size { get; set; }
        public GameObject insectPrefab { get; set; }

        protected BaseCardAction(int size)
        {
            this.size = size;
        }
        
        public abstract void DoAction(Node targetNode);
    }
}