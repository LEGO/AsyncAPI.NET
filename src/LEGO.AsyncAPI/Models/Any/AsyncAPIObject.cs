// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Nodes;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// AsyncApi object.
    /// </summary>
    [Obsolete("Please use AsyncApiAny instead")]
    public class AsyncApiObject : Dictionary<string, AsyncApiAny>, IAsyncApiExtension, IAsyncApiElement
    {

        public static implicit operator AsyncApiAny(AsyncApiObject obj)
        {
            var jObject = new JsonObject();
            foreach (var item in obj)
            {
                jObject.Add(item.Key, item.Value.GetNode());
            }

            return new AsyncApiAny(jObject);
        }

        /// <summary>
        /// Serialize AsyncApiObject to writer.
        /// </summary>
        public void Write(IAsyncApiWriter writer)
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
