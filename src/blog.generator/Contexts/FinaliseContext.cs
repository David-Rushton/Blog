using System;
using System.Collections.Generic;


namespace Blog.Generator.Contexts
{
    public class FinaliseContext
    {
        public FinaliseContext(
            ScaffoldContext scaffoldContext,
            List<MarkupContext> markupContexts,
            List<HtmlPathUrl> html
        )
        {
            ScaffoldContext = scaffoldContext;
            MarkupContexts = markupContexts;
            Html = html;
        }


        public ScaffoldContext ScaffoldContext { get; internal set; }
        public List<MarkupContext> MarkupContexts { get; internal set; }
        public List<HtmlPathUrl> Html { get; internal set; }
    }
}
