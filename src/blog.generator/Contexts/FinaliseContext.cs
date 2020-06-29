using System;
using System.Collections.Generic;


namespace Blog.Generator.Contexts
{
    public class FinaliseContext
    {
        public FinaliseContext(
            ScaffoldContext scaffoldContext,
            List<MarkupContext> markupContexts,
            List<String> htmlFilePaths
        )
        {
            ScaffoldContext = scaffoldContext;
            MarkupContexts = markupContexts;
            HtmlFilePaths = htmlFilePaths;
        }


        public ScaffoldContext ScaffoldContext { get; internal set; }
        public List<MarkupContext> MarkupContexts { get; internal set; }
        public List<String> HtmlFilePaths { get; internal set; }
    }
}
