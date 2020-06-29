using Blog.Generator.Contexts;
using Blog.Generator.Documents;


namespace Blog.Generator.Processors.Abstractions
{
    public abstract class MarkupProcessor
    {
        public abstract void Invoke(MarkupContext context, MarkupDocument document);
    }
}
