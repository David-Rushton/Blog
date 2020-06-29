using Blog.Generator.Contexts;


namespace Blog.Generator.Processors.Abstractions
{
    public abstract class FinaliseProcessor
    {
        public abstract void Invoke(FinaliseContext context);
    }
}
