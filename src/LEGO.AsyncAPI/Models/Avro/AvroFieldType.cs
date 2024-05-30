// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    public abstract class AvroSchema : IAsyncApiSerializable
    {
        public static implicit operator AvroSchema(AvroPrimitiveType type)
        {
            return new AvroPrimitive(type);
        }

        public abstract void SerializeV2(IAsyncApiWriter writer);
    }
}