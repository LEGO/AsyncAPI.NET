// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Any
{
    /// <summary>
    /// Async API long.
    /// </summary>
    public struct AsyncAPILong : IPrimitiveValue<long>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncAPILong"/> class.
        /// </summary>
        /// <param name="value">Initialization value.</param>
        public AsyncAPILong(long value)
            : this()
        {
            this.Value = value;
        }

        /// <summary>
        /// The type of <see cref="IAny"/>.
        /// </summary>
        public PrimitiveType PrimitiveType => PrimitiveType.Long;

        /// <summary>
        /// AnyType.Primitive.
        /// </summary>
        public AnyType AnyType => AnyType.Primitive;

        /// <summary>
        /// Value of the struct.
        /// </summary>
        public long Value { get; set; }

        public static explicit operator long(AsyncAPILong l) => l.Value;

        public static explicit operator AsyncAPILong(long l) => new (value: l);
    }
}