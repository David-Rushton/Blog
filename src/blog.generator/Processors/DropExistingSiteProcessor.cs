using Blog.Generator.Contexts;
using Blog.Generator.Processors.Abstractions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Blog.Generator.Processors
{
    public class DropExistingSiteProcessor : ScaffoldProcessor
    {
        public override void Invoke(SiteContext context)
        {
            if(Directory.Exists(context.SiteRoot))
            {
                Console.WriteLine("Dropping existing site");
                Directory.Delete(context.SiteRoot, true);
            }
        }
    }
}
