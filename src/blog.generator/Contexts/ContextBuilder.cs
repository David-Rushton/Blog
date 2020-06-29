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


        public Context Build(string path, string content)
            => new Context(_siteContext, BuildArticleContext(path, content))
        ;


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
            {
                MarkdownPath = markdownPath,
                MarkdownContent = markdownContent,
                HtmlContentTemplate = htmlContentTemplate
            }
        ;




        private ArticleContext BuildArticleContext(string path, string content)
            => new ArticleContext(path, content, BuildImageContext());

        private ImageContext BuildImageContext()
            => new ImageContext();
    }
}
