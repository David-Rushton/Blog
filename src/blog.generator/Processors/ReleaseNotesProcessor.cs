using Blog.Generator.Contexts;
using Blog.Generator.Processors.Abstractions;
using Markdig;
using System;
using System.IO;


namespace Blog.Generator.Processors
{
    public class ReleaseNotesProcessor : FinaliseProcessor
    {
        readonly MarkdownPipeline _pipeline;


        public ReleaseNotesProcessor(MarkdownPipeline pipeline) => (_pipeline) = (pipeline);


        public override void Invoke(FinaliseContext context)
        {
            Console.WriteLine($"Generating release notes page.");

            var releaseNotesPage = context.HtmlContexts["/release-notes.html"];
            var mdReleaseNotes = File.ReadAllText(context.ScaffoldContext.ReleaseNotesPath);
            var htmlReleaseNotes = Markdown.ToHtml(mdReleaseNotes, _pipeline);

            releaseNotesPage.Content = releaseNotesPage.Content
                .Replace("$(version-number)",   context.ScaffoldContext.VersionNumber)
                .Replace("$(last-updated)",     DateTime.Now.ToString("yyy-MM-dd"))
                .Replace("$(release-notes)",    htmlReleaseNotes)
            ;
        }

        public override string ToString() => "Release Notes Processor";
    }
}
