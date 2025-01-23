// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Avro.LogicalTypes
{
    public class AvroUUID : AvroLogicalType
    {
        public AvroUUID()
            : base(AvroPrimitiveType.String)
        {
        }

        public override LogicalType LogicalType => LogicalType.UUID;
    }
}
