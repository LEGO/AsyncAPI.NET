// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Avro.LogicalTypes
{
    public class AvroTimestampMillis : AvroLogicalType
    {
        public AvroTimestampMillis()
            : base(AvroPrimitiveType.Long)
        {
        }

        public override LogicalType LogicalType => LogicalType.Timestamp_Millis;
    }
}
