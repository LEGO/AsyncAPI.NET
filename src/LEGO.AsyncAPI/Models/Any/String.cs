// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Any
{
    /// <summary>
    /// Async API null.
    /// </summary>
    public struct String : IPrimitiveValue<string>
    {
        public String(string value)
        {
            this.Value = value;
        }

        /// <summary>
        /// The type of <see cref="IOpenApiAny"/>.
        /// </summary>
        public PrimitiveType PrimitiveType => PrimitiveType.String;

        public string? Value { get; set; }

        public AnyType AnyType => AnyType.Primitive;

        public static explicit operator string?(String s) => s.Value;

        public static explicit operator String(string s) => new () { Value = s };
    }
}