using System;
using System.Collections.Generic;


namespace Blog.Generator.Documents
{
    public class MarkupDocuments
    {
        readonly Dictionary<string, MarkupDocument> _documents = new Dictionary<string, MarkupDocument>();


        public MarkupDocument this[string path]
            => _documents[path]
        ;


        public MarkupDocument New(string path)
        {
            if(_documents.ContainsKey(path))
                throw new Exception($"Markup document already exists: {path}");

            _documents.Add(path, new MarkupDocument(path));
            return _documents[path];
        }
    }
}
