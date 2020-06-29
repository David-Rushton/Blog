using Blog.Generator.Documents;
using Blog.Generator.Processors.Abstractions;
using System;
using System.Collections.Generic;


namespace Blog.Generator.Processors
{
    public class ProcessorPipelineBuilder
    {
        List<ScaffoldProcessor> _scaffoldProcessors = new List<ScaffoldProcessor>();
        List<MarkupProcessor> _markupProcessors = new List<MarkupProcessor>();
        List<FinaliseProcessor> _finaliseProcessors = new List<FinaliseProcessor>();
        readonly internal Config _config;


        public ProcessorPipelineBuilder(Config config)
            => (_config) = (config)
        ;


        public ProcessorPipelineBuilder RegisterPipelineProcessor(object processor)
        {
            switch(processor)
            {
                case ScaffoldProcessor scaffold:
                    _scaffoldProcessors.Add((ScaffoldProcessor)processor);
                    break;

                case MarkupProcessor markup:
                    _markupProcessors.Add((MarkupProcessor)processor);
                    break;

                case FinaliseProcessor finalise:
                    _finaliseProcessors.Add((FinaliseProcessor)processor);
                    break;

                default:
                    throw new Exception($"Processor type not supported: {processor.GetType()}");
            };

            return this;
        }

        public ProcessorPipeline Build()
            => new ProcessorPipeline(_scaffoldProcessors, _markupProcessors, _finaliseProcessors)
        ;
    }
}
