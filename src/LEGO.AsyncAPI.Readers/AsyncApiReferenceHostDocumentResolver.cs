// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Services;

    internal class AsyncApiReferenceHostDocumentResolver : AsyncApiVisitorBase
    {
        private AsyncApiWorkspace workspace;
        private List<AsyncApiError> errors = new List<AsyncApiError>();

        public AsyncApiReferenceHostDocumentResolver(
            AsyncApiWorkspace workspace)
        {
            this.workspace = workspace;
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
                referenceable.Reference.Workspace = this.workspace;
            }
        }
    }
}