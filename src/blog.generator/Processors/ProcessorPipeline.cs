using Blog.Generator.Contexts;
using Blog.Generator.Processors.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Blog.Generator.Processors
{
    public delegate Task NextDelegate(Context context);


    public class ProcessorPipeline
    {
        readonly List<ScaffoldProcessor> _scaffoldProcessors = new List<ScaffoldProcessor>();
        readonly List<MarkupProcessor> _markupProcessors = new List<MarkupProcessor>();
        readonly List<FinaliseProcessor> _finaliseProcessors = new List<FinaliseProcessor>();


        public ProcessorPipeline(
            List<ScaffoldProcessor> scaffoldProcessors,
            List<MarkupProcessor> markupProcessors,
            List<FinaliseProcessor> finaliseProcessors
        )
            => (_scaffoldProcessors, _markupProcessors, _finaliseProcessors)
            =  ( scaffoldProcessors,  markupProcessors,  finaliseProcessors)
        ;


        public List<ScaffoldProcessor> ScaffoldProcessors => _scaffoldProcessors;
        public List<MarkupProcessor> MarkupProcessors => _markupProcessors;
        public List<FinaliseProcessor> FinaliseProcessors => _finaliseProcessors;

    }
}
