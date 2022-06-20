// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Any
{
    using LEGO.AsyncAPI.Models.Interfaces;

    /// <summary>
    /// Open API Float
    /// </summary>
    public class AsyncApiFloat : AsyncApiPrimitive<float>
    {
        /// <summary>
        /// Initializes the <see cref="AsyncApiFloat"/> class.
        /// </summary>
        public AsyncApiFloat(float value)
            : base(value)
        {
        }

        /// <summary>
        /// Primitive type this object represents.
        /// </summary>
        public override PrimitiveType PrimitiveType { get; } = PrimitiveType.Float;
    }
}
