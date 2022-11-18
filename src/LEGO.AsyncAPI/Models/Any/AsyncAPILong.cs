namespace LEGO.AsyncAPI.Models.Any
{
    using LEGO.AsyncAPI.Models.Interfaces;

    /// <summary>
    /// AsyncApi long.
    /// </summary>
    public class AsyncApiLong : AsyncApiPrimitive<long>
    {
        /// <summary>
        /// Initializes the <see cref="AsyncApiLong"/> class.
        /// </summary>
        public AsyncApiLong(long value)
            : base(value)
        {
        }

        /// <summary>
        /// Primitive type this object represents.
        /// </summary>
        public override PrimitiveType PrimitiveType { get; } = PrimitiveType.Long;
    }
}
