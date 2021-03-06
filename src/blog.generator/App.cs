using Blog.Generator.Contexts;
using Blog.Generator.Processors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace Blog.Generator
{
    public class App
    {
        readonly Config _config;
        readonly ContextFactory _contextFactory;
        readonly ProcessorPipeline _processorPipeline;


        /// <summary>
        /// Prepares the application for use
        /// </summary>
        /// <param name="config">Command arguments</param>
        /// <param name="contextFactory">Generates the contexts passed to the processor pipeline</param>
        /// <param name="processorPipeline">Executes a series of processes that build the site, piece by piece</param>
        public App(Config config, ContextFactory contextFactory, ProcessorPipeline processorPipeline)
        {
            _config = config;
            _contextFactory = contextFactory;
            _processorPipeline = processorPipeline;
        }


        /// <summary>
        /// Builds the website, using provided arguments and source files
        /// </summary>
        public async Task InvokeAsync()
        {
            Console.WriteLine("Generating blog...");
            Console.WriteLine(_config);
            Console.WriteLine(_processorPipeline);

            InvokeScaffoldPipeline();
            await InvokeMarkupPipeline();
            await InvokeFinalisePipeline();

            OutputSuccessMessage();
        }


        /// <summary>
        /// This is where the heavy lifting occurs.
        /// The core of the site is constructing by coping the template site and injecting markup articles.
        /// </summary>
        private void InvokeScaffoldPipeline()
        {
            var scaffoldContext = _contextFactory.GetScaffoldContext();
            _processorPipeline.InvokeScaffoldPipeline(scaffoldContext);
        }

        /// <summary>
        /// Converts the raw markup files into processed Html files.
        /// </summary>
        private async Task InvokeMarkupPipeline()
        {
            var markupContexts = await _contextFactory.NewMarkupContextsAsync();
            foreach(var markupContext in markupContexts)
            {
                _processorPipeline.InvokeMarkupPipeline(markupContext);
            }

            var markupChangesToSaveTask = markupContexts
                .Where(c => c.Html.IsDirty)
                .Select(async c =>
                {
                    Console.WriteLine($"Saving content: {c.Html.Path}");
                    await c.Html.SaveContentAsync();
                })
                .ToList()
            ;

            await Task.WhenAll(markupChangesToSaveTask);
        }

        /// <summary>
        /// Applies the final spit and polish.
        /// Builds pages that require the processed Html files, generated by earlier steps.
        /// Examples include the index, search and sitemaps pages.
        /// </summary>
        private async Task InvokeFinalisePipeline()
        {
            var finaliseContext = await _contextFactory.NewFinaliseContextAsync();
            _processorPipeline.InvokeFinalisePipeline(finaliseContext);

            var htmlChangesToSaveTask = finaliseContext.HtmlContexts.Values
                .Where(c => c.IsDirty)
                .Select(async c =>
                {
                    Console.WriteLine($"Saving content: {c.Path}");
                    await c.SaveContentAsync();
                })
                .ToList()
            ;

            await Task.WhenAll(htmlChangesToSaveTask);
        }

        /// <summary>
        /// Prints a handy completed message to the console
        /// </summary>
        private void OutputSuccessMessage()
        {
            var original = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("🏁 ✔ 👍 Blog generated 👍 ✔ 🏁");
            Console.ForegroundColor = original;
        }
    }
}
