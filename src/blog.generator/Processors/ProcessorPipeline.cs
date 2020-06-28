using blog.generator.contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace blog.generator.processors
{
    public delegate Task NextDelegate(Context context);


    public class ProcessorPipeline
    {
        readonly List<Processor> _scaffoldProcessors = new List<Processor>();
        readonly List<Processor> _markupProcessors = new List<Processor>();
        readonly List<Processor> _finaliseProcessors = new List<Processor>();


        public ProcessorPipeline(
            List<Processor> scaffoldProcessors,
            List<Processor> markupProcessors,
            List<Processor> finaliseProcessors
        )
            => (_scaffoldProcessors, _markupProcessors, _finaliseProcessors)
            =  ( scaffoldProcessors,  markupProcessors,  finaliseProcessors)
        ;


        public List<Processor> ScaffoldProcessors => _scaffoldProcessors;
        public List<Processor> MarkupProcessors => _markupProcessors;
        public List<Processor> FinaliseProcessors => _finaliseProcessors;

    }
}
