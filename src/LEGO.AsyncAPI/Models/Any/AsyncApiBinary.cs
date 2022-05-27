// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Any
{
    using LEGO.AsyncAPI.Models.Interfaces;

    /// <summary>
    /// Open API binary.
    /// </summary>
    public class AsyncApiBinary : AsyncApiPrimitive<byte[]>
    {
        /// <summary>
        /// Initializes the <see cref="AsyncApiBinary"/> class.
        /// </summary>
        /// <param name="value"></param>
        public AsyncApiBinary(byte[] value)
            : base(value)
        {
        }

        /// <summary>
        /// Primitive type this object represents.
        /// </summary>
        public override PrimitiveType PrimitiveType { get; } = PrimitiveType.Binary;
    }
}
