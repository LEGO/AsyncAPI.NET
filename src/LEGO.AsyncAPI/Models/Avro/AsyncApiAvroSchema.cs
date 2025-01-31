// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    public abstract class AsyncApiAvroSchema : IAsyncApiSerializable, IAsyncApiMessagePayload
    {
        public abstract string Type { get; }

        /// <summary>
        /// A map of properties not in the schema, but added as additional metadata.
        /// </summary>
        public abstract IDictionary<string, AsyncApiAny> Metadata { get; set; }

        public static implicit operator AsyncApiAvroSchema(AvroPrimitiveType type)
        {
            return new AvroPrimitive(type);
        }

        public abstract void SerializeV2(IAsyncApiWriter writer);

        public virtual bool TryGetAs<T>(out T result)
            where T : AsyncApiAvroSchema
        {
            result = this as T;
            return result != null;
        }

        public virtual bool Is<T>()
            where T : AsyncApiAvroSchema
        {
            return this is T;
        }

        public virtual T As<T>()
            where T : AsyncApiAvroSchema
        {
            return this as T;
        }
    }
}