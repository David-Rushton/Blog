using System;


namespace Blog.Generator.Contexts
{
    /// <summary>
    /// Contains all the information required to scaffold the site
    /// </summary>
    public class ScaffoldContext
    {
        // Used to suppress nullable warnings
        public ScaffoldContext()
            : this("", "", "", "", "", 0)
        { }

        public ScaffoldContext(
            string siteRoot,
            string templateSiteRoot,
            string articlesSourceRoot,
            string articlesTargetRoot,
            string versionNumber,
            int newBadgeCutoffInDays
        )
            => (SiteRoot, TemplateSiteRoot, ArticlesSourceRoot, ArticlesTargetRoot, VersionNumber, NewBadgeCutoffInDays)
            =  (siteRoot, templateSiteRoot, articlesSourceRoot, articlesTargetRoot, versionNumber, newBadgeCutoffInDays)
        ;


        public string SiteRoot { get; internal set; }
        public string TemplateSiteRoot { get; internal set; }
        public string ArticlesSourceRoot { get; internal set; }
        public string ArticlesTargetRoot { get; internal set; }
        public string VersionNumber { get; internal set; }
        public int NewBadgeCutoffInDays { get; internal set; }


        public string ConvertPathToUrl(string path)
            => path
                .Replace(SiteRoot, "")
                .Replace('\\', '/')
        ;
    }
}
