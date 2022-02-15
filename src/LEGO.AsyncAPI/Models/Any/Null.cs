// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Any
{
    /// <summary>
    /// Async API null.
    /// </summary>
    public class Null : PrimitiveValue<string>
    {
        /// <summary>
        /// The type of <see cref="IOpenApiAny"/>.
        /// </summary>
        public AnyType AnyType { get; } = AnyType.Null;

        public override PrimitiveType PrimitiveType { get => PrimitiveType.Null; }

        public override string? Value
        {
            get => null;
            set => throw new NotImplementedException();
        }
    }
}