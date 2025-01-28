// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers.Services
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Services;

    internal class AsyncApiRemoteReferenceCollector : AsyncApiVisitorBase
    {
        private readonly List<IAsyncApiReferenceable> references = new();
        private AsyncApiDocument currentDocument;

        public AsyncApiRemoteReferenceCollector(
            AsyncApiDocument currentDocument)
        {
            this.currentDocument = currentDocument;
        }
        /// <summary>
        /// List of all external references collected from AsyncApiDocument.
        /// </summary>
        public IEnumerable<IAsyncApiReferenceable> References
        {
            get
            {
                return this.references;
            }
        }

        /// <summary>
        /// Collect reference for each reference.
        /// </summary>
        /// <param name="referenceable"></param>
        public override void Visit(IAsyncApiReferenceable referenceable)
        {
            if (referenceable.Reference != null && referenceable.Reference.IsExternal)
            {
                if (referenceable.Reference.HostDocument == null)
                {
                    referenceable.Reference.HostDocument = this.currentDocument;
                }

                this.references.Add(referenceable);
            }
        }
    }
}
