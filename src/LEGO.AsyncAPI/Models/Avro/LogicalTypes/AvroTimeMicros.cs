// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Avro.LogicalTypes
{
    public class AvroTimeMicros : AvroLogicalType
    {
        public AvroTimeMicros()
            : base(AvroPrimitiveType.Long)
        {
        }

        public override LogicalType LogicalType => LogicalType.Time_Micros;
    }
}
