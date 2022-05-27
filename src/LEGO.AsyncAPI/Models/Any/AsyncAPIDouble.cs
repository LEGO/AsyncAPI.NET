// Copyright (c) The LEGO Group. All rights reserved.

using LEGO.AsyncAPI.Models.Interfaces;

namespace LEGO.AsyncAPI.Models.Any
{
    /// <summary>
    /// Open API Double
    /// </summary>
    public class AsyncApiDouble : AsyncApiPrimitive<double>
    {
        /// <summary>
        /// Initializes the <see cref="AsyncApiDouble"/> class.
        /// </summary>
        public AsyncApiDouble(double value)
            : base(value)
        {
        }

        /// <summary>
        /// Primitive type this object represents.
        /// </summary>
        public override PrimitiveType PrimitiveType { get; } = PrimitiveType.Double;
    }
}
