using Blog.Generator.Contexts;


namespace Blog.Generator.Processors.Abstractions
{
    public abstract class MarkupProcessor
    {
        public abstract void Invoke(MarkupContext context);
    }
}
