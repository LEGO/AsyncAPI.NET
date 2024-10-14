// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Avro.LogicalTypes
{
    public class AvroTimeMillis : AvroLogicalType
    {
        public AvroTimeMillis()
            : base(AvroPrimitiveType.Int)
        {
        }

        public override LogicalType LogicalType => LogicalType.Time_Millis;
    }
}
