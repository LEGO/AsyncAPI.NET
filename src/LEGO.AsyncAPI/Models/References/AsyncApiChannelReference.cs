// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    public class AsyncApiChannelReference : AsyncApiChannel, IAsyncApiReferenceable
    {
        private AsyncApiChannel target;

        private AsyncApiChannel Target
        {
            get
            {
                this.target ??= this.Reference.HostDocument?.ResolveReference<AsyncApiChannel>(this.Reference);
                return this.target;
            }
        }

        public AsyncApiChannelReference(string reference)
        {
            this.Reference = new AsyncApiReference(reference, ReferenceType.Channel);
        }

        public override string Description { get => this.Target?.Description; set => this.Target.Description = value; }

        public override IList<string> Servers { get => this.Target?.Servers; set => this.Target.Servers = value; }

        public override AsyncApiOperation Subscribe { get => this.Target?.Subscribe; set => this.Target.Subscribe = value; }

        public override AsyncApiOperation Publish { get => this.Target?.Publish; set => this.Target.Publish = value; }

        public override IDictionary<string, AsyncApiParameter> Parameters { get => this.Target?.Parameters; set => this.Target.Parameters = value; }

        public override AsyncApiBindings<IChannelBinding> Bindings { get => this.Target?.Bindings; set => this.Target.Bindings = value; }

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
                this.Reference.HostDocument = writer.RootDocument;
                this.Target.SerializeV2(writer);
            }
        }
    }
}
