namespace ElJardin.CardActions
{
    public interface ICardAction
    {
        int size { get; set; }
        void DoAction(Node targetNode);
    }
}