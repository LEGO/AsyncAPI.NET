// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Any
{
    /// <summary>
    /// Async API string.
    /// </summary>
    public struct String : IPrimitiveValue<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="String"/> class.
        /// </summary>
        /// <param name="value">Initialization value.</param>
        public String(string value)
        {
            this.Value = value;
        }

        /// <summary>
        /// The type of <see cref="IAny"/>.
        /// </summary>
        public PrimitiveType PrimitiveType => PrimitiveType.String;

        /// <summary>
        /// Value.
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// AnyType.Primitive.
        /// </summary>
        public AnyType AnyType => AnyType.Primitive;

        public static explicit operator string?(String s) => s.Value;

        public static explicit operator String(string s) => new() { Value = s };
    }
}