using System;


namespace blog.generator.contexts
{
    public class Context
    {
        public Context(SiteContext site, ArticleContext article)
            => (Site, Article) = (site, article)
        ;


        public SiteContext Site { get; internal set; }

        public ArticleContext Article { get; internal set; }
    }
}
