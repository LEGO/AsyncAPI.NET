namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// The definition of a parameter this application MAY use.
    /// </summary>
    public class AsyncApiParameterReference : AsyncApiParameter, IAsyncApiReferenceable
    {
        private AsyncApiParameter target;

        private AsyncApiParameter Target
        {
            get
            {
                this.target ??= this.Reference.HostDocument.ResolveReference<AsyncApiParameter>(this.Reference);
                return this.target;
            }
        }

        public AsyncApiParameterReference(string reference)
        {
            this.Reference = new AsyncApiReference(reference, ReferenceType.Parameter);
        }

        public override string Description { get => this.Target?.Description; set => this.Target.Description = value; }

        public override AsyncApiJsonSchema Schema { get => this.Target?.Schema; set => this.Target.Schema = value; }

        public override string Location { get => this.Target?.Location; set => this.Target.Location = value; }

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
