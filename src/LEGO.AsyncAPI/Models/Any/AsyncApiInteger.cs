// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Any
{
    using LEGO.AsyncAPI.Models.Interfaces;

    /// <summary>
    /// AsyncApi Integer
    /// </summary>
    public class AsyncApiInteger : AsyncApiPrimitive<int>
    {
        /// <summary>
        /// Initializes the <see cref="AsyncApiInteger"/> class.
        /// </summary>
        public AsyncApiInteger(int value)
            : base(value)
        {
        }

        /// <summary>
        /// Primitive type this object represents.
        /// </summary>
        public override PrimitiveType PrimitiveType { get; } = PrimitiveType.Integer;
    }
}
