using Blog.Generator.Contexts;


namespace Blog.Generator.Processors.Abstractions
{
    public abstract class ScaffoldProcessor
    {
        public abstract void Invoke(SiteContext context);
    }
}
