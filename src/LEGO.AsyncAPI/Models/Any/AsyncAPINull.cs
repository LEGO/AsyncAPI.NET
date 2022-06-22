// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Any
{
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Open API null.
    /// </summary>
    public class AsyncApiNull : IAsyncApiAny
    {
        /// <summary>
        /// The type of <see cref="IAsyncApiAny"/>
        /// </summary>
        public AnyType AnyType { get; } = AnyType.Null;

        /// <summary>
        /// Write out null representation
        /// </summary>
        /// <param name="writer"></param>
        public void Write(IAsyncApiWriter writer, AsyncApiVersion asyncApiVersion)
        {
            writer.WriteAny(this);
        }
    }
}
