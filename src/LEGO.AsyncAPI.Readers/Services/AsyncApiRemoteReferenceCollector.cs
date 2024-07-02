// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers.Services
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Services;

    internal class AsyncApiRemoteReferenceCollector : AsyncApiVisitorBase
    {
        private readonly Dictionary<string, AsyncApiReference> references = new();

        /// <summary>
        /// List of all external references collected from AsyncApiDocument.
        /// </summary>
        public IEnumerable<AsyncApiReference> References
        {
            get
            {
                return this.references.Values;
            }
        }

        /// <summary>
        /// Collect reference for each reference.
        /// </summary>
        /// <param name="referenceable"></param>
        public override void Visit(IAsyncApiReferenceable referenceable)
        {
            this.AddExternalReferences(referenceable.Reference);
        }

        /// <summary>
        /// Collect external references.
        /// </summary>
        private void AddExternalReferences(AsyncApiReference reference)
        {
            if (reference is { IsExternal: true } &&
                !this.references.ContainsKey(reference.ExternalResource))
            {
                this.references.Add(reference.ExternalResource, reference);
            }
        }
    }
}
