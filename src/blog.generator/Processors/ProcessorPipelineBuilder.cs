using System;
using System.Collections.Generic;


namespace blog.generator.processors
{
    public class ProcessorPipelineBuilder
    {
        List<Processor> _processors = new List<Processor>();


        public ProcessorPipelineBuilder RegisterPipelineProcessor(Processor processor)
        {
            _processors.Add(processor);
            return this;
        }

        public ProcessorPipeline Build()
            => new ProcessorPipeline(_processors)
        ;
    }
}
