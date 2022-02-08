namespace LEGO.AsyncAPI.Any
{
    public class Boolean : Primitive<bool?>
    {
        private bool? _value;

        /// <summary>
        /// The type of <see cref="IOpenApiAny"/>.
        /// </summary>
        public override PrimitiveType PrimitiveType { get; } = PrimitiveType.Boolean;

        public override bool? Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}