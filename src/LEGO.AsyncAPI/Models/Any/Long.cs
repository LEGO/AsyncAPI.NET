namespace LEGO.AsyncAPI.Models.Any
{
    /// <summary>
    /// Async API null.
    /// </summary>
    public struct Long : PrimitiveValue<long?>
    {
        public Long(long? value) : this()
        {
            Value = value;
        }

        /// <summary>
        /// The type of <see cref="IOpenApiAny"/>.
        /// </summary>
        public PrimitiveType PrimitiveType { get; } = PrimitiveType.Long;

        public AnyType AnyType => AnyType.Primitive;

        public long? Value { get; set; }

        public static explicit operator long?(Long l) => l.Value;

        public static explicit operator Long(long l) => new(value: l);
    }
}