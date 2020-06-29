using Blog.Generator.Contexts;
using Blog.Generator.Processors.Abstractions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Blog.Generator.Processors
{
    public class InjectMarkdownArticlesProcessor : ScaffoldProcessor
    {
        public override void Invoke(SiteContext context)
        {
            Console.WriteLine("Copying raw articles...");
            FileSystemHelper.DeepCopyDirectory(context.ArticleSource, context.ArticleTarget);
        }

        public override string ToString()
            => "Inject Markdown Articles Processor"
        ;
    }
}
