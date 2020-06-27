using blog.generator.contexts;
using System;
using System.Threading.Tasks;


namespace blog.generator.processors
{
    public abstract class Processor
    {
        public abstract Task InvokeAsync(Context context, NextDelegate next);
    }
}
