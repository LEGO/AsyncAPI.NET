// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Avro.LogicalTypes
{
    using System.Linq;
    using LEGO.AsyncAPI.Writers;

    public abstract class AvroLogicalType : AvroPrimitive
    {
        protected AvroLogicalType(AvroPrimitiveType type)
            : base(type)
        {
        }

        public abstract LogicalType LogicalType { get; }

        public override void SerializeV2Core(IAsyncApiWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteOptionalProperty("type", this.Type);
            writer.WriteOptionalProperty("logicalType", this.LogicalType.GetDisplayName());

            this.SerializeV2Core(writer);

            if (this.Metadata.Any())
            {
                foreach (var item in this.Metadata)
                {
                    writer.WritePropertyName(item.Key);
                    if (item.Value == null)
                    {
                        writer.WriteNull();
                    }
                    else
                    {
                        writer.WriteAny(item.Value);
                    }
                }
            }

            writer.WriteEndObject();
        }
    }
}
