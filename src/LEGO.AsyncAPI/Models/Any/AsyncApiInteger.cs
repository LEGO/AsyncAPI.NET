// Copyright (c) The LEGO Group. All rights reserved.

using LEGO.AsyncAPI.Models.Interfaces;

namespace LEGO.AsyncAPI.Models.Any
{
    /// <summary>
    /// Open API Integer
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
