using System.Collections.Generic;
using UnityEngine;

namespace ElJardin.CardActions
{
    public class BuildRiver : BaseCardAction
    {
        bool actionCompleted;
        public BuildRiver(int size) : base(size)
        {
        }

        public override void DoAction(Node targetNode) {
            Debug.Log($"CardAction - Building river");
            BuildManager.Instance.OnBuildEnds = () => { onActionCompleted.Invoke(actionCompleted); };
            TryBuildRiver();
        }

        void TryBuildRiver()
        {
            actionCompleted = BuildManager.Instance.ChangeNodesInList();
            onCardUsed?.Invoke(actionCompleted);
        }
    }
}