namespace ElJardin.Util.Patterns
{
    public interface IFactory <out T>
    {
        T Create();
    }

    public interface IFactory<out T, in TArg>
    {
        T Create(TArg arg);
    }
}