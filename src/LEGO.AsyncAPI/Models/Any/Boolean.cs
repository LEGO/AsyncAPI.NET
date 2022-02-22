namespace LEGO.AsyncAPI.Models.Any
{
    public struct Boolean : PrimitiveValue<bool>
    {
        public Boolean(bool value) : this()
        {
            Value = value;
        }

        /// <summary>
        /// The type of <see cref="IOpenApiAny"/>.
        /// </summary>
        public PrimitiveType PrimitiveType { get; } = PrimitiveType.Boolean;

        public bool Value { get; set; }

        public static explicit operator bool(Boolean b) => b.Value;

        public static explicit operator Boolean(bool b) => new(value: b);

        public AnyType AnyType => AnyType.Primitive;
    }
}