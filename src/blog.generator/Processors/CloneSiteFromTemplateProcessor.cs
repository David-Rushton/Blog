using Blog.Generator.Contexts;
using Blog.Generator.Processors.Abstractions;
using System;


namespace Blog.Generator.Processors
{
    public class CloneSiteFromTemplateProcessor : ScaffoldProcessor
    {
        public override void Invoke(SiteContext context)
        {
            Console.WriteLine("Coping template site...");
            FileSystemHelper.DeepCopyDirectory(context.TemplateSiteRoot, context.SiteRoot);
        }
    }
}
