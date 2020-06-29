using Blog.Generator.Contexts;
using Blog.Generator.Processors.Abstractions;
using System;


namespace Blog.Generator.Processors
{
    public class SitemapsProcessor : FinaliseProcessor
    {
        public override void Invoke(FinaliseContext context)
        {

        }

        public override string ToString()
            => "Sitemaps Processor"
        ;
    }
}
