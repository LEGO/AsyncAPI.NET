// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Any
{
    /// <summary>
    /// Async API Boolean.
    /// </summary>
    public class AsyncAPIBoolean : IPrimitiveValue<bool>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncAPIBoolean"/> class.
        /// </summary>
        /// <param name="value">Initialization value.</param>
        public AsyncAPIBoolean(bool value)
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

        public static explicit operator bool(AsyncAPIBoolean b) => b.Value;

        public static explicit operator AsyncAPIBoolean(bool b) => new(value: b);
    }
}