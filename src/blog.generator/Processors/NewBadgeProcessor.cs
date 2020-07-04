using Blog.Generator.Contexts;
using Blog.Generator.Processors.Abstractions;
using System;


namespace Blog.Generator.Processors
{
    public class NewBadgeProcessor : FinaliseProcessor
    {
        int _newCutOffInDays;


        public NewBadgeProcessor(int newCutOffInDays)
            => (_newCutOffInDays) = (newCutOffInDays)
        ;


        public override void Invoke(FinaliseContext context)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
            => "New Badge Processor"
        ;
    }
}
