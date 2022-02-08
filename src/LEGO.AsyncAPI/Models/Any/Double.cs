// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Any
{
    /// <summary>
    /// Async API null.
    /// </summary>
    public class Double : Primitive<double?>
    {
        private double? _value;

        /// <summary>
        /// The type of <see cref="IOpenApiAny"/>.
        /// </summary>
        public override PrimitiveType PrimitiveType { get; } = PrimitiveType.Double;

        public override double? Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}