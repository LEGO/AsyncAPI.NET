// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Any
{
    /// <summary>
    /// Async API null.
    /// </summary>
    public class String : PrimitiveValue<string>
    {
        public String()
        {
        }

        public String(string? value)
        {
            Value = value;
        }

        /// <summary>
        /// The type of <see cref="IOpenApiAny"/>.
        /// </summary>
        public override PrimitiveType PrimitiveType { get; } = PrimitiveType.String;

        public override string? Value { get; set; }

        public static explicit operator string?(String s) => s.Value;

        public static explicit operator String(string s) => new () { Value = s };
    }
}