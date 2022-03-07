// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Any
{
    /// <summary>
    /// Async API null.
    /// </summary>
    public struct Double : IPrimitiveValue<double?>
    {
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