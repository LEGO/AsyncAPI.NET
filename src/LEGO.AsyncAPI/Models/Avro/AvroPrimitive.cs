// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using LEGO.AsyncAPI.Writers;
    using System.Runtime.CompilerServices;

    public class AvroPrimitive : AvroFieldType
    {
        public AvroPrimitiveType Type { get; set; }

        public AvroPrimitive(AvroPrimitiveType type)
        {
            this.Type = type;
        }

        public override void SerializeV2(IAsyncApiWriter writer)
        {
            writer.WriteValue(this.Type.GetDisplayName());
        }
    }
}