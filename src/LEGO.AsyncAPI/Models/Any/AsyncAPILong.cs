// Copyright (c) The LEGO Group. All rights reserved.

using LEGO.AsyncAPI.Models.Interfaces;

namespace LEGO.AsyncAPI.Models.Any
{
    /// <summary>
    /// Open API long.
    /// </summary>
    public class AsyncApiLong : AsyncApiPrimitive<long>
    {
        /// <summary>
        /// Initializes the <see cref="AsyncApiLong"/> class.
        /// </summary>
        public AsyncApiLong(long value)
            : base(value)
        {
        }

        /// <summary>
        /// Primitive type this object represents.
        /// </summary>
        public override PrimitiveType PrimitiveType { get; } = PrimitiveType.Long;
    }
}
