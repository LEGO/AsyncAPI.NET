// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    public class AsyncApiAvroSchemaReference : AsyncApiAvroSchema, IAsyncApiReferenceable
    {
        private AsyncApiAvroSchema target;

        private AsyncApiAvroSchema Target
        {
            get
            {
                this.target ??= this.Reference.Workspace?.ResolveReference<AsyncApiAvroSchema>(this.Reference);
                return this.target;
            }
        }

        public AsyncApiAvroSchemaReference(string reference)
        {
            this.Reference = new AsyncApiReference(reference, ReferenceType.Schema);
        }

        public override string Type => this.Target?.Type;

        public override IDictionary<string, AsyncApiAny> Metadata { get => this.Target?.Metadata; set => this.Target.Metadata = value; }

        public bool UnresolvedReference { get { return this.Target == null; } }

        public AsyncApiReference Reference { get; set; }

        public override T As<T>()
        {
            if (this.Target == null)
            {
                return null;
            }
            return this.Target.As<T>();
        }

        public override bool Is<T>()
        {
            if (Target == null)
            {
                return false;
            }
            return this.Target.Is<T>();
        }

        public override bool TryGetAs<T>(out T result)
        {
            if (this.Target == null)
            {
                result = default;
                return false;
            }
            return this.Target.TryGetAs(out result);
        }

        public override void SerializeV2(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            if (this.Reference != null && !writer.GetSettings().ShouldInlineReference(this.Reference))
            {
                this.Reference.SerializeV2(writer);
                return;
            }

            this.Target.SerializeV2(writer);
        }
    }
}