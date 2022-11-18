namespace LEGO.AsyncAPI.Models.Any
{
    using System;
    using LEGO.AsyncAPI.Models.Interfaces;

    /// <summary>
    /// AsyncApi Date
    /// </summary>
    public class AsyncApiDate : AsyncApiPrimitive<DateTime>
    {
        /// <summary>
        /// Initializes the <see cref="AsyncApiDate"/> class.
        /// </summary>
        public AsyncApiDate(DateTime value)
            : base(value)
        {
        }

        /// <summary>
        /// Primitive type this object represents.
        /// </summary>
        public override PrimitiveType PrimitiveType { get; } = PrimitiveType.Date;
    }
}
