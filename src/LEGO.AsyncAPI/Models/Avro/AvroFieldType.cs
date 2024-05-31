// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;
    using System.Collections.Generic;

    public abstract class AvroSchema : IAsyncApiSerializable
    {
        public abstract string Type { get; }

        /// <summary>
        /// A map of properties not in the schema, but added as additional metadata.
        /// </summary>
        public abstract IDictionary<string, AsyncApiAny> Metadata { get; set; }

        public static implicit operator AvroSchema(AvroPrimitiveType type)
        {
            return new AvroPrimitive(type);
        }

        public abstract void SerializeV2(IAsyncApiWriter writer);
    }
}