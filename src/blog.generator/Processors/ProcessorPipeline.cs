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
        readonly List<Processor> _processors = new List<Processor>();
        Context? _context;


        public ProcessorPipeline(List<Processor> processors)
            => (_processors) = (processors)
        ;


        public async Task<Context> InvokePipelineAsync(Context context)
        {
            return await Task.Run(async() =>
            {
                Queue<Processor> processors = new Queue<Processor>(_processors);
                Context returnContext = context;


                if (processors.Count > 0)
                    await processors.Dequeue().InvokeAsync(context, GetNextDelegate());

                return returnContext;


                NextDelegate GetNextDelegate()
                    => processors.Count == 0 ? FinalDelegate() : NextProcessor()
                ;

                NextDelegate NextProcessor()
                    => async(Context context) => await processors.Dequeue().InvokeAsync(context, GetNextDelegate())
                ;

                NextDelegate FinalDelegate()
                    => async(Context context) => await Task.Run(() => returnContext = context)
                ;
            });
        }
    }
}
