using blog.generator;
using System;


namespace blog.generator.contexts
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


        private SiteContext BuildSiteContext()
            => new SiteContext
            {
                BuildNumber = _config.BuildNumber,
                BuildSha = _config.BuildSha
            }
        ;

        private ArticleContext BuildArticleContext(string path, string content)
            => new ArticleContext(path, content, BuildImageContext());

        private ImageContext BuildImageContext()
            => new ImageContext();
    }
}
