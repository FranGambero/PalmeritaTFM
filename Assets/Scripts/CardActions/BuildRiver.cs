using System.Collections.Generic;
using UnityEngine;

namespace ElJardin.CardActions
{
    public class BuildRiver : BaseCardAction
    {
        public BuildRiver(int size) : base(size)
        {
        }

        public override void DoAction(Node targetNode)
        {
            Debug.Log($"CardAction - Building river");
            TryBuildRiver();
        }

        void TryBuildRiver()
        {
            var actionCompleted = BuildManager.Instance.ChangeNodesInList();
            onActionCompleted.Invoke(actionCompleted);
        }
    }
}