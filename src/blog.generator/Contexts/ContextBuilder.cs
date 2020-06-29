using System;
using System.Collections.Generic;


namespace Blog.Generator.Contexts
{
    public class ContextBuilder
    {
        readonly Config _config;
        readonly ScaffoldContext _scaffoldContext;
        readonly List<MarkupContext> _markupContexts = new List<MarkupContext>();


        public ContextBuilder(Config config)
        {
            _config = config;
            _scaffoldContext = new ScaffoldContext
            {
                SiteRoot = _config.BlogRoot,
                TemplateSiteRoot = _config.TemplateRoot,
                ArticleSource = _config.ArticlesSourceRoot,
                ArticleTarget = _config.ArticlesBlogRoot,
                BuildNumber = _config.BuildNumber,
                BuildSha = _config.BuildSha
            };
        }


        public ScaffoldContext GetScaffoldContext()
            => _scaffoldContext
        ;

        public MarkupContext BuildMarkupContext(string markdownPath, string markdownContent, string htmlContentTemplate)
        {
            var returnContext = new MarkupContext
            (
                markdownPath,
                markdownContent,
                htmlContentTemplate
            );

            _markupContexts.Add(returnContext);

            return returnContext;
        }


        public FinaliseContext BuildFinaliseContext(List<string> htmlFilePaths)
            => new FinaliseContext(_scaffoldContext, _markupContexts, htmlFilePaths)
        ;
    }
}
