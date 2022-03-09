// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Any
{
    /// <summary>
    /// Async API long.
    /// </summary>
    public struct Long : IPrimitiveValue<long?>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Long"/> class.
        /// </summary>
        /// <param name="value">Initialization value.</param>
        public Long(long? value)
            : this()
        {
            this.Value = value;
        }

        /// <summary>
        /// The type of <see cref="IAny"/>.
        /// </summary>
        public PrimitiveType PrimitiveType { get; } = PrimitiveType.Long;

        /// <summary>
        /// AnyType.Primitive.
        /// </summary>
        public AnyType AnyType => AnyType.Primitive;

        /// <summary>
        /// Value of the struct.
        /// </summary>
        public long? Value { get; set; }

        public static explicit operator long?(Long l) => l.Value;

        public static explicit operator Long(long l) => new (value: l);
    }
}