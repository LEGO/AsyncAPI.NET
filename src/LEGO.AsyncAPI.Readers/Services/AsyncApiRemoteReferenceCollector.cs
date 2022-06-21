using System.Collections.Generic;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Interfaces;

namespace LEGO.AsyncAPI.Readers.Services
{
    internal class AsyncApiRemoteReferenceCollector : AsyncApiVisitorBase
    {
        private AsyncApiDocument _document;
        private Dictionary<string, AsyncApiReference> _references = new Dictionary<string, AsyncApiReference>();
        public AsyncApiRemoteReferenceCollector(AsyncApiDocument document)
        {
            _document = document;
        }

        public IEnumerable<AsyncApiReference> References
        {
            get {
                return _references.Values;
            }
        }
        
        public override void Visit(IAsyncApiReferenceable referenceable)
        {
            AddReference(referenceable.Reference);
        }
        
        private void AddReference(AsyncApiReference reference)
        {
            if (reference != null)
            {
                if (reference.IsExternal)
                {
                    if (!_references.ContainsKey(reference.ExternalResource))
                    {
                        _references.Add(reference.ExternalResource, reference);
                    }
                }
            }
        }    
    }
}