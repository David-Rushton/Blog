using Blog.Generator;
using System;
using System.Collections.Generic;


namespace Blog.Generator.Contexts
{
    public class ScaffoldContextFactory
    {
        public ScaffoldContext NewScaffoldContext(Config config)
            => new ScaffoldContext
            {
                SiteRoot = config.BlogRoot,
                TemplateSiteRoot = config.TemplateRoot,
                ArticlesSourceRoot = config.ArticlesSourceRoot,
                ArticlesTargetRoot = config.ArticlesTargetRoot,
                VersionNumber = config.VersionNumber,
                NewBadgeCutoffInDays = config.NewBadgeCutoffInDays
            }
        ;
    }
}
