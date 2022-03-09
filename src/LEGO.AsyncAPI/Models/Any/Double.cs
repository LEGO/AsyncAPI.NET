// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Any
{
    /// <summary>
    /// Async API double.
    /// </summary>
    public struct Double : IPrimitiveValue<double?>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Double"/> class.
        /// </summary>
        /// <param name="value">Initialization value.</param>
        public Double(double? value)
            : this()
        {
            this.Value = value;
        }

        /// <summary>
        /// The type of <see cref="IAny"/>.
        /// </summary>
        public PrimitiveType PrimitiveType { get; } = PrimitiveType.Double;

        public double? Value { get; set; }

        public AnyType AnyType => AnyType.Primitive;

        public static explicit operator double?(Double d) => d.Value;

        public static explicit operator Double(double d) => new (value: d);
    }
}