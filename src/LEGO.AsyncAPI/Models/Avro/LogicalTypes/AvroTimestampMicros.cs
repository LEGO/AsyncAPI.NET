// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Avro.LogicalTypes
{
    public class AvroTimestampMicros : AvroLogicalType
    {
        public AvroTimestampMicros()
            : base(AvroPrimitiveType.Long)
        {
        }

        public override LogicalType LogicalType => LogicalType.Timestamp_Micros;
    }
}
