namespace LEGO.AsyncAPI.Models.Any
{
    public struct Boolean : PrimitiveValue<bool>
    {
        private bool _value;

        /// <summary>
        /// The type of <see cref="IOpenApiAny"/>.
        /// </summary>
        public PrimitiveType PrimitiveType { get; } = PrimitiveType.Boolean;

        public bool Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public static explicit operator bool(Boolean b) => b.Value;

        public static explicit operator Boolean(bool b) => new () { Value = b };

        public AnyType AnyType => AnyType.Primitive;
    }
}