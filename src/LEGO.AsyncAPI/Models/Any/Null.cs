namespace LEGO.AsyncAPI.Models.Any
{
    using JetBrains.Annotations;

    /// <summary>
    /// Async API null.
    /// </summary>
    public struct Null : PrimitiveValue<string>
    {
        public static readonly Null Instance = new();

        /// <summary>
        /// The type of <see cref="IOpenApiAny"/>.
        /// </summary>
        public AnyType AnyType { get; } = AnyType.Primitive;

        public PrimitiveType PrimitiveType => PrimitiveType.Null;

        [CanBeNull]
        public string Value
        {
            get => null;
            set => throw new NotImplementedException();
        }
    }
}