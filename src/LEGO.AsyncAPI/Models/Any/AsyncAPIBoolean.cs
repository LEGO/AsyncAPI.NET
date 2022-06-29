// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Any
{
    using LEGO.AsyncAPI.Models.Interfaces;

    /// <summary>
    /// AsyncApi boolean.
    /// </summary>
    public class AsyncApiBoolean : AsyncApiPrimitive<bool>
    {
        /// <summary>
        /// Initializes the <see cref="AsyncApiBoolean"/> class.
        /// </summary>
        /// <param name="value"></param>
        public AsyncApiBoolean(bool value)
            : base(value)
        {
        }

        /// <summary>
        /// Primitive type this object represents.
        /// </summary>
        public override PrimitiveType PrimitiveType { get; } = PrimitiveType.Boolean;
    }
}
