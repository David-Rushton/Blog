using System;
using System.IO;
using System.Text;


namespace Blog.Generator
{
    public class Config
    {
        public Config(
            string versionNumber,
            string blogRoot,
            string templateRoot,
            string articlesSourceRoot,
            string articlesTargetRoot,
            string releaseNotesPath,
            string dbConnectionString,
            string dbName,
            string dbContainer,
            int newBadgeCutoffInDays
        )
            =>  (VersionNumber, BlogRoot, TemplateRoot, ArticlesSourceRoot, ArticlesTargetRoot, ReleaseNotesPath, DbConnectionString, DbName, DbContainer, NewBadgeCutoffInDays)
            =   (versionNumber, blogRoot, templateRoot, articlesSourceRoot, articlesTargetRoot, releaseNotesPath, dbConnectionString, dbName, dbContainer, newBadgeCutoffInDays)
        ;


        public string VersionNumber { get; internal set; }
        public string BlogRoot { get; internal set; }
        public string TemplateRoot { get; internal set; }
        public string ArticlesSourceRoot { get; internal set; }
        public string ArticlesTargetRoot { get; internal set; }
        public string ReleaseNotesPath { get; internal set; }
        public string DbConnectionString { get; internal set; }
        public string DbName { get; internal set; }
        public string DbContainer { get; internal set; }
        public int NewBadgeCutoffInDays { get; internal set; }


        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("Config:\n\tVersion Number: {0}\n\tBlog Root: {1}\n\tTemplate Root: {2}");
            sb.Append("\n\tArticles Source Root: {3}\n\tArticles Target Root: {4}\n\tRelease Notes Path: {5}");
            sb.Append("\n\tdb Connection String: xxxxx\n\tdb Name: {6}\n\tdb Container: {7}");
            sb.Append("\n\tNew Badge Cut-off In Days: {8}");

            return String.Format
            (
                sb.ToString(),
                VersionNumber,
                BlogRoot,
                TemplateRoot,
                ArticlesSourceRoot,
                ArticlesTargetRoot,
                ReleaseNotesPath,
                // DbConnectionString <REDACTED>
                DbName,
                DbContainer,
                NewBadgeCutoffInDays.ToString("#,0")
            );
        }
    }
}
