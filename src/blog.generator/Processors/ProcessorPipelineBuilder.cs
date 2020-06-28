using System;
using System.Collections.Generic;


namespace blog.generator.processors
{
    public class ProcessorPipelineBuilder
    {
        List<Processor> _scaffoldProcessors = new List<Processor>();
        List<Processor> _markupProcessors = new List<Processor>();
        List<Processor> _finaliseProcessors = new List<Processor>();
        readonly internal Config _config;


        public ProcessorPipelineBuilder(Config config)
            => (_config) = (config)
        ;


        public ProcessorPipelineBuilder RegisterPipelineProcessor(Processor processor)
        {
            switch(processor.Type)
            {
                case ProcessorType.ScaffoldProcessor:
                    _scaffoldProcessors.Add(processor);
                    break;

                case ProcessorType.MarkupProcessor:
                    _markupProcessors.Add(processor);
                    break;

                case ProcessorType.FinaliseProcessor:
                    _finaliseProcessors.Add(processor);
                    break;

                default:
                    throw new Exception($"Processor type not supported: {processor.Type}");
            };

            return this;
        }

        public ProcessorPipeline Build()
            => new ProcessorPipeline(_markupProcessors)
        ;
    }
}
