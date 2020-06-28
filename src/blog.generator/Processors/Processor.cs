using blog.generator.contexts;
using System;
using System.Threading.Tasks;


namespace blog.generator.processors
{
    public enum ProcessorType
    {
        ScaffoldProcessor,
        MarkupProcessor,
        FinaliseProcessor
    }


    public abstract class Processor
    {
        public abstract ProcessorType Type { get; }

        public abstract Task InvokeAsync(Context context);
    }
}
