namespace ElJardin.Util.Patterns
{
    public interface IConverter<in Source, out Target>
    {
        Target Convert(Source source);
    }

    public interface ITranslator<Source, Target>
    {
        Target Translate(Source source);
        Source Translate(Target target);
    }
}