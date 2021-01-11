using System;
using ElJardin.CardActions;
using ElJardin.Hover;
using UnityEngine.Events;

namespace ElJardin
{
    public class ActionCard
    {
        IHover hoverProvider;
        ICardAction actionProvider;

        public void HoverOnGrab() => hoverProvider.HoverOnGrab();
        public void HoverOnNodeEnter(Node targetNode) => hoverProvider.HoverOnNodeEnter(targetNode);
        public void UnHover() => hoverProvider.Hide();
        public void Action(Node targetNode) => actionProvider.DoAction(targetNode);

        public UnityEvent<bool> OnActionCompleted => actionProvider.onActionCompleted;
        public UnityEvent<bool> OnCardUsed => actionProvider.onCardUsed;

        public ActionCard(IHover hoverProvider, ICardAction actionProvider)
        {
            this.hoverProvider = hoverProvider;
            this.actionProvider = actionProvider;
        }
    }
}