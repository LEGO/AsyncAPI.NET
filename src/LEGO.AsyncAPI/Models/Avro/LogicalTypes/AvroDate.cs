// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Avro.LogicalTypes
{
    public class AvroDate : AvroLogicalType
    {
        public AvroDate()
            : base(AvroPrimitiveType.Int)
        {
        }

        public override LogicalType LogicalType => LogicalType.Date;
    }
}
