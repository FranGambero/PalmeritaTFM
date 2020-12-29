using ElJardin.CardActions;
using ElJardin.Hover;

namespace ElJardin
{
    public class ActionCard
    {
        IHover hoverProvider;
        ICardAction actionProvider;

        public void Hover(Node targetNode) => hoverProvider.Show(targetNode);
        public void UnHover() => hoverProvider.Hide();
        public void Action(Node targetNode) => actionProvider.DoAction(targetNode);

        public ActionCard(IHover hoverProvider, ICardAction actionProvider)
        {
            this.hoverProvider = hoverProvider;
            this.actionProvider = actionProvider;
        }
    }
}