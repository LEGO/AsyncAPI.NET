namespace LEGO.AsyncAPI.Models.Any
{
    /// <summary>
    /// Async API null.
    /// </summary>
    public struct Double : PrimitiveValue<double?>
    {
        private double? _value;

        /// <summary>
        /// The type of <see cref="IOpenApiAny"/>.
        /// </summary>
        public PrimitiveType PrimitiveType { get; } = PrimitiveType.Double;

        public double? Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public static explicit operator double?(Double d) => d.Value;

        public static explicit operator Double(double d) => new () { Value = d };
        public AnyType AnyType => AnyType.Primitive;
    }
}