using Blog.Generator.Contexts;
using Blog.Generator.Documents;
using Blog.Generator.Processors.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Blog.Generator.Processors
{
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


        public void InvokeScaffoldPipeline(SiteContext context)
        {
            foreach(var processor in _scaffoldProcessors)
            {
                processor.Invoke(context);
            }
        }

        public void InvokeMarkupPipeline(MarkupContext context, MarkupDocument document)
        {
            foreach(var processor in _markupProcessors)
            {
                processor.Invoke(context, document);
            }
        }

        public void InvokeFinalisePipeline()
        {

        }
    }
}
