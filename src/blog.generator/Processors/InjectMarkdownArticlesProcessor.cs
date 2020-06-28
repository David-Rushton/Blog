using Blog.Generator.Contexts;
using Blog.Generator.Processors.Abstractions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Blog.Generator.Processors
{
    public class InjectMarkdownArticlesProcessor : ScaffoldProcessor
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
