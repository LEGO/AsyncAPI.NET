// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Any
{
    /// <summary>
    /// Async API null.
    /// </summary>
    public class String : Primitive<string>
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
    }
}