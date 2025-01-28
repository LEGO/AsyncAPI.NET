// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// The definition of a security scheme this application MAY use.
    /// </summary>
    public class AsyncApiSecuritySchemeReference : AsyncApiSecurityScheme, IAsyncApiReferenceable
    {
        private AsyncApiSecurityScheme target;

        private AsyncApiSecurityScheme Target
        {
            get
            {
                this.target ??= this.Reference.HostDocument.ResolveReference<AsyncApiSecurityScheme>(this.Reference);
                return this.target;
            }
        }

        public AsyncApiSecuritySchemeReference(string reference)
        {
            this.Reference = new AsyncApiReference(reference, ReferenceType.SecurityScheme);
        }

        public override SecuritySchemeType Type { get => this.Target.Type; set => this.Target.Type = value; }

        public override string Description { get => this.Target?.Description; set => this.Target.Description = value; }

        public override string Name { get => this.Target?.Name; set => this.Target.Name = value; }

        public override ParameterLocation In { get => this.Target.In; set => this.Target.In = value; }

        public override string Scheme { get => this.Target?.Scheme; set => this.Target.Scheme = value; }

        public override string BearerFormat { get => this.Target?.BearerFormat; set => this.Target.BearerFormat = value; }

        public override AsyncApiOAuthFlows Flows { get => this.Target?.Flows; set => this.Target.Flows = value; }

        public override Uri OpenIdConnectUrl { get => this.Target?.OpenIdConnectUrl; set => this.Target.OpenIdConnectUrl = value; }

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
