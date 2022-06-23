using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Readers.Interface;
using LEGO.AsyncAPI.Services;
using SharpYaml.Model;

namespace LEGO.AsyncAPI.Readers.Services
{
    internal class AsyncApiWorkspaceLoader 
    {
        private AsyncApiWorkspace _workspace;
        private IStreamLoader _loader;
        private readonly AsyncApiReaderSettings _readerSettings;

        public AsyncApiWorkspaceLoader(AsyncApiWorkspace workspace, IStreamLoader loader, AsyncApiReaderSettings readerSettings)
        {
            _workspace = workspace;
            _loader = loader;
            _readerSettings = readerSettings;
        }

        internal async Task LoadAsync(AsyncApiReference reference, AsyncApiDocument document)
        {
            _workspace.AddDocument(reference.ExternalResource, document);
            document.Workspace = _workspace;

            var referenceCollector = new AsyncApiRemoteReferenceCollector(document);
            var collectorWalker = new AsyncApiWalker(referenceCollector);
            collectorWalker.Walk(document);

            var reader = new AsyncApiStreamReader(_readerSettings);
            
            foreach (var item in referenceCollector.References)
            {
                if (!_workspace.Contains(item.ExternalResource))
                {
                    var input = await _loader.LoadAsync(new Uri(item.ExternalResource, UriKind.RelativeOrAbsolute));
                    var result = await reader.ReadAsync(input);
                    await LoadAsync(item, result.AsyncApiDocument);
                }
            }
        }
    }
}