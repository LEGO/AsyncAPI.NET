// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Services;

    internal class AsyncApiReferenceHostDocumentResolver : AsyncApiVisitorBase
    {
        private AsyncApiDocument currentDocument;
        private List<AsyncApiError> errors = new List<AsyncApiError>();

        public AsyncApiReferenceHostDocumentResolver(
            AsyncApiDocument currentDocument)
        {
            this.currentDocument = currentDocument;
        }

        public IEnumerable<AsyncApiError> Errors
        {
            get
            {
                return this.errors;
            }
        }

        public override void Visit(IAsyncApiReferenceable referenceable)
        {
            if (referenceable.Reference != null)
            {
                referenceable.Reference.HostDocument = this.currentDocument;
            }
        }
    }
}