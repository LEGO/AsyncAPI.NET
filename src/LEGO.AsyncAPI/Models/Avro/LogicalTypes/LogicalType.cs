// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Avro.LogicalTypes
{
    using LEGO.AsyncAPI.Attributes;

    public enum LogicalType
    {
        [Display("decimal")]
        Decimal,

        [Display("uuid")]
        UUID,

        [Display("date")]
        Date,

        [Display("time-millis")]
        Time_Millis,

        [Display("time-micros")]
        Time_Micros,

        [Display("timestamp-millis")]
        Timestamp_Millis,

        [Display("timestamp-micros")]
        Timestamp_Micros,

        [Display("duration")]
        Duration,
    }
}
