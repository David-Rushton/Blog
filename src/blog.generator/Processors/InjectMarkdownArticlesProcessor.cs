using blog.generator.contexts;
using System;
using System.IO;
using System.Threading.Tasks;

namespace blog.generator.processors
{
    public class InjectMarkdownArticlesProcessor : Processor
    {
        readonly string _source;
        readonly string _target;


        public InjectMarkdownArticlesProcessor(string source, string target)
            => (_source, _target) = (source, target)
        ;


        public override ProcessorType Type
            => ProcessorType.ScaffoldProcessor
        ;


        public override Task InvokeAsync(Context context)
        {
            FileSystemHelper.DeepCopyDirectory(source, target)
        }
    }
}
