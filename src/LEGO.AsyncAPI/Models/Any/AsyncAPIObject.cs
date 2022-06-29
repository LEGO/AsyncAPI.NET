// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Any
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// AsyncApi object.
    /// </summary>
    public class AsyncApiObject : Dictionary<string, IAsyncApiAny>, IAsyncApiAny
    {
        /// <summary>
        /// Type of <see cref="IAsyncApiAny"/>.
        /// </summary>
        public AnyType AnyType { get; } = AnyType.Object;

        /// <summary>
        /// Serialize AsyncApiObject to writer
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="specVersion">Version of the AsyncApi specification that that will be output.</param>
        public void Write(IAsyncApiWriter writer, AsyncApiVersion asyncApiVersion)
        {
            writer.WriteStartObject();

            foreach (var item in this)
            {
                writer.WritePropertyName(item.Key);
                writer.WriteAny(item.Value);
            }

            writer.WriteEndObject();
        }
    }
}
