namespace LEGO.AsyncAPI.Models.Any
{
    using JetBrains.Annotations;

    /// <summary>
    /// Async API null.
    /// </summary>
    public struct Null : PrimitiveValue<string>
    {
        /// <summary>
        /// The type of <see cref="IOpenApiAny"/>.
        /// </summary>
        public AnyType AnyType { get; } = AnyType.Primitive;

        public PrimitiveType PrimitiveType { get => PrimitiveType.Null; }

        [CanBeNull]
        public string Value
        {
            get => null;
            set => throw new NotImplementedException();
        }
    }
}