// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Any
{
    /// <summary>
    /// Async API double.
    /// </summary>
    public class AsyncAPIDouble : IPrimitiveValue<double>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncAPIDouble"/> class.
        /// </summary>
        /// <param name="value">Initialization value.</param>
        public AsyncAPIDouble(double value)
        {
            this.Value = value;
        }

        /// <summary>
        /// The type of <see cref="IAny"/>.
        /// </summary>
        public PrimitiveType PrimitiveType => PrimitiveType.Double;

        /// <summary>
        /// Value.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// AnyType.Primitive.
        /// </summary>
        public AnyType AnyType => AnyType.Primitive;

        public static explicit operator double(AsyncAPIDouble d) => d.Value;

        public static explicit operator AsyncAPIDouble(double d) => new (value: d);
    }
}