using Blog.Generator.Contexts;
using Blog.Generator.Processors.Abstractions;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Blog.Generator.Processors
{
    public class DropExistingSiteProcessor : ScaffoldProcessor
    {
        readonly string _target;


        public DropExistingSiteProcessor(string target)
            => (_target) = (target)
        ;


        public override ProcessorType Type
            => ProcessorType.ScaffoldProcessor
        ;


        public override Task InvokeAsync(Context context)
        {
            if(Directory.Exists(_target))
            {
                Console.WriteLine("Dropping existing site");
                Directory.Delete(_target, true);
            }
        }
    }
}
