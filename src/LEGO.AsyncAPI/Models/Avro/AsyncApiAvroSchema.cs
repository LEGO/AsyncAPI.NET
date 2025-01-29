// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    public abstract class AsyncApiAvroSchema : IAsyncApiSerializable, IAsyncApiReferenceable, IAsyncApiMessagePayload
    {
        public abstract string Type { get; }

        /// <summary>
        /// A map of properties not in the schema, but added as additional metadata.
        /// </summary>
        public abstract IDictionary<string, AsyncApiAny> Metadata { get; set; }

        public bool UnresolvedReference { get; set; }

        public AsyncApiReference Reference { get; set; }

        public static implicit operator AsyncApiAvroSchema(AvroPrimitiveType type)
        {
            return new AvroPrimitive(type);
        }

        public virtual void SerializeV2(IAsyncApiWriter writer)
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

            this.SerializeV2Core(writer);
        }

        public abstract void SerializeV2Core(IAsyncApiWriter writer);
    }
}