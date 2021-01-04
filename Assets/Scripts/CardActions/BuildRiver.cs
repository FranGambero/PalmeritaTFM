namespace ElJardin.CardActions
{
    public class BuildRiver : BaseCardAction
    {
        public BuildRiver(int size) : base(size)
        {
        }

        public override void DoAction(Node targetNode)
        {
            //construir rio en las n posiciones a partir del nodo (targetNode)
            //targetNode tiene que ser el nodo mas cercano a sepalo en la direccion elegida
        }
    }
}