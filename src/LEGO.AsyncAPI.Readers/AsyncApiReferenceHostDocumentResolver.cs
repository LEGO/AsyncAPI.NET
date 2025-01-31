// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Services;

    internal class AsyncApiReferenceWorkspaceResolver : AsyncApiVisitorBase
    {
        private AsyncApiWorkspace workspace;

        public AsyncApiReferenceWorkspaceResolver(
            AsyncApiWorkspace workspace)
        {
            this.workspace = workspace;
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