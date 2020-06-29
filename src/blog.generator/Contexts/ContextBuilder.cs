using Blog.Generator;
using System;
using System.IO;


namespace Blog.Generator.Contexts
{
    public class ContextBuilder
    {
        readonly Config _config;
        readonly SiteContext _siteContext;


        public ContextBuilder(Config config)
        {
            _config = config;
            _siteContext = BuildSiteContext();
        }


        public SiteContext BuildSiteContext()
            => new SiteContext
            {
                SiteRoot = _config.BlogRoot,
                TemplateSiteRoot = _config.TemplateRoot,
                ArticleSource = _config.ArticlesSourceRoot,
                ArticleTarget = _config.ArticlesBlogRoot,
                BuildNumber = _config.BuildNumber,
                BuildSha = _config.BuildSha
            }
        ;

        public MarkupContext BuildMarkupContext(string markdownPath, string markdownContent, string htmlContentTemplate)
            => new MarkupContext
            (
                markdownPath,
                markdownContent,
                htmlContentTemplate
            )
        ;
    }
}
