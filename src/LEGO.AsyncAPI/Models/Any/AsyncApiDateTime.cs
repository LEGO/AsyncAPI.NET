// Copyright (c) The LEGO Group. All rights reserved.

using LEGO.AsyncAPI.Models.Interfaces;
using System;

namespace LEGO.AsyncAPI.Models.Any
{
    /// <summary>
    /// Open API Datetime
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
