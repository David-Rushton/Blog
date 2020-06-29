using Blog.Generator.Contexts;
using Blog.Generator.Processors.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;


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

        public void InvokeMarkupPipeline(MarkupContext context)
        {
            foreach(var processor in _markupProcessors)
            {
                processor.Invoke(context);
            }
        }

        public void InvokeFinalisePipeline()
        {

        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("Scaffold processors enabled:\n\t");
            sb.Append(String.Join("\n\t", _scaffoldProcessors));

            sb.Append("\nMarkup processors enabled:\n\t");
            sb.Append(String.Join("\n\t", _markupProcessors));

            sb.Append("\nFinalise processors enabled:\n\t");
            sb.Append(String.Join("\n\t", _finaliseProcessors));

            return sb.ToString();
        }
    }
}
