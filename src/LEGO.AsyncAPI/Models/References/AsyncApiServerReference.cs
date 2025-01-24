// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// The definition of a server this application MAY connect to.
    /// </summary>
    public class AsyncApiServerReference : AsyncApiServer, IAsyncApiReferenceable
    {
        private AsyncApiServer target;

        private AsyncApiServer Target
        {
            get
            {
                this.target ??= this.Reference.HostDocument.ResolveReference<AsyncApiServer>(this.Reference);
                return this.target;
            }
        }

        public AsyncApiServerReference(string reference)
        {
            this.Reference = new AsyncApiReference(reference, ReferenceType.Server);
        }

        public override string Url { get => this.Target?.Url; set => this.Target.Url = value; }

        public override string Protocol { get => this.Target?.Protocol; set => this.Target.Protocol = value; }

        public override string ProtocolVersion { get => this.Target?.ProtocolVersion; set => this.Target.ProtocolVersion = value; }

        public override string Description { get => this.Target?.Description; set => this.Target.Description = value; }

        public override IDictionary<string, AsyncApiServerVariable> Variables { get => this.Target?.Variables; set => this.Target.Variables = value; }

        public override IList<AsyncApiSecurityRequirement> Security { get => this.Target?.Security; set => this.Target.Security = value; }

        public override IList<AsyncApiTag> Tags { get => this.Target?.Tags; set => this.Target.Tags = value; }

        public override AsyncApiBindings<IServerBinding> Bindings { get => this.Target?.Bindings; set => this.Target.Bindings = value; }

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
                this.Target.SerializeV2(writer);
            }
        }
    }
}