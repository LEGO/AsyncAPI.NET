// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Collections.ObjectModel;
    using System.Text.Json.Nodes;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    public class AsyncApiArray : Collection<AsyncApiAny>, IAsyncApiExtension, IAsyncApiElement
    {

        public static explicit operator AsyncApiArray(AsyncApiAny any)
        {
            var a = new AsyncApiArray();
            if (any.Node is JsonArray arr)
            {
                foreach (var item in arr)
                {
                    a.Add(new AsyncApiAny(item));
                }
            }

            return a;
        }

        public static implicit operator AsyncApiAny(AsyncApiArray arr)
        {
            var jArray = new JsonArray();
            foreach (var item in arr)
            {
                jArray.Add(item.Node);
            }

            return new AsyncApiAny(jArray);
        }

        /// <summary>
        /// Serialize AsyncApiObject to writer.
        /// </summary>
        public void Write(IAsyncApiWriter writer)
        {
            writer.WriteStartArray();

            foreach (var item in this)
            {
                writer.WriteAny(item);
            }

            writer.WriteEndArray();
        }
    }
}
