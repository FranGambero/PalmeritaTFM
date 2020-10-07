namespace ElJardin.Movement
{
    public interface IPathable
    {
        int GCost { get; set; }
        int HCost { get; set; }
        int FCost { get; set; }
        Node CameFromNode { get; set; }

        void CalculateFCost();
    }
}