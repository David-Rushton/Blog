using System;
using System.Collections.Generic;
using System.IO;


namespace Blog.Generator.Contexts
{
    /// <summary>
    /// Contains information required to provide the site with its final spit and polish
    /// </summary>
    public class FinaliseContext
    {
        public FinaliseContext(
            ScaffoldContext scaffoldContext,
            List<MarkupContext> markupContexts,
            Dictionary<string, HtmlContext> html
        )
        {
            ScaffoldContext = scaffoldContext;
            MarkupContexts = markupContexts;
            HtmlContexts = html;
        }


        public ScaffoldContext ScaffoldContext { get; internal set; }
        public List<MarkupContext> MarkupContexts { get; internal set; }
        public Dictionary<string, HtmlContext> HtmlContexts { get; internal set; }
    }
}
