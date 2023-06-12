// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Any
{
    using System;
    using LEGO.AsyncAPI.Models.Interfaces;

    /// <summary>
    /// AsyncApi Datetime.
    /// </summary>
    public class AsyncApiDateTime : AsyncApiPrimitive<DateTimeOffset>
    {
        /// <summary>
        /// Initializes the <see cref="AsyncApiDateTime"/> class.
        /// </summary>
        public AsyncApiDateTime(DateTimeOffset value)
            : base(value)
        {
        }

        /// <summary>
        /// Primitive type this object represents.
        /// </summary>
        public override PrimitiveType PrimitiveType { get; } = PrimitiveType.DateTime;
    }
}
