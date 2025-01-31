// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// The definition of a server variable this application MAY use.
    /// </summary>
    public class AsyncApiServerVariableReference : AsyncApiServerVariable, IAsyncApiReferenceable
    {
        private AsyncApiServerVariable target;

        private AsyncApiServerVariable Target
        {
            get
            {
                this.target ??= this.Reference.Workspace?.ResolveReference<AsyncApiServerVariable>(this.Reference);
                return this.target;
            }
        }

        public AsyncApiServerVariableReference(string reference)
        {
            this.Reference = new AsyncApiReference(reference, ReferenceType.ServerVariable);
        }
        public override IList<string> Enum { get => this.Target?.Enum; set => this.Target.Enum = value; }

        public override string Default { get => this.Target?.Default; set => this.Target.Default = value; }

        public override string Description { get => this.Target?.Description; set => this.Target.Description = value; }

        public override IList<string> Examples { get => this.Target?.Examples; set => this.Target.Examples = value; }

        public override IDictionary<string, IAsyncApiExtension> Extensions { get => this.Target?.Extensions; set => this.Target.Extensions = value; }

        public AsyncApiReference Reference { get; set; }

        public bool UnresolvedReference { get { return this.Target == null; } }

        public override void SerializeV2(IAsyncApiWriter writer)
        {
            if (!writer.GetSettings().ShouldInlineReference(this.Reference))
            {
                this.Reference.SerializeV2(writer);
                return;
            }
            else
            {
                this.Reference.Workspace = writer.Workspace;
                this.Target.SerializeV2(writer);
            }
        }
    }
}
