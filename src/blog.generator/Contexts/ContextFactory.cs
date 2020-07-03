using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Blog.Generator.Contexts
{
    public class ContextFactory
    {
        readonly Config _config;
        readonly ScaffoldContext _scaffoldContext;
        List<MarkupContext> _markupContexts = new List<MarkupContext>();


        public ContextFactory(Config config)
        {
            _config = config;
            _scaffoldContext = new ScaffoldContextFactory().NewScaffoldContext(config);
        }

        public ScaffoldContext GetScaffoldContext()
            => _scaffoldContext
        ;

        public async Task<List<MarkupContext>> NewMarkupContextsAsync()
        {
            _markupContexts = await new MarkupContextFactory()
                .NewMarkupContextsAsync(_scaffoldContext, _config.ArticlesTargetRoot);

            return _markupContexts;
        }

        public async Task<FinaliseContext> NewFinaliseContextAsync()
        {
            // Once the finalise context has been generated the MarkupContexts become read-only.
            // This avoids the situation where a file could be updated via both contexts.
            _markupContexts.ForEach(c => c.Html.LockContent());
            return await new FinaliseContextFactor().NewFinaliseContextAsync(_scaffoldContext, _markupContexts);
        }
    }
}
