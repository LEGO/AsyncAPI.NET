// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Any
{
    /// <summary>
    /// Async API Boolean.
    /// </summary>
    public struct Boolean : IPrimitiveValue<bool>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Boolean"/> class.
        /// </summary>
        /// <param name="value">Initialization value.</param>
        public Boolean(bool value)
            : this()
        {
            this.Value = value;
        }

        /// <summary>
        /// The type of <see cref="IAny"/>.
        /// </summary>
        public PrimitiveType PrimitiveType => PrimitiveType.Boolean;

        /// <summary>
        /// Value.
        /// </summary>
        public bool Value { get; set; }

        /// <summary>
        /// AnyType.Primitive.
        /// </summary>
        public AnyType AnyType => AnyType.Primitive;

        public static explicit operator bool(Boolean b) => b.Value;

        public static explicit operator Boolean(bool b) => new (value: b);
    }
}