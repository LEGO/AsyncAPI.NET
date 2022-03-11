// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI
{
    using System.IO;
    using LEGO.AsyncAPI.NewtonUtils;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// AsyncApi JSON stream reader.
    /// </summary>
    /// <typeparam name="T">AsyncAPI object type.</typeparam>
    public class JsonStreamReader<T> : IStreamReader<T>
    {
        /// <summary>
        /// Deserializes stream into AsyncAPI object of type T.
        /// </summary>
        /// <param name="stream">JSON content stream.</param>
        /// <returns>AsyncAPI object.</returns>
        public T Read(Stream stream)
        {
            var jsonString = new StreamReader(stream).ReadToEnd();
            var root = JsonConvert.DeserializeObject<JToken>(jsonString, JsonSerializerUtils.Settings);
            root.ResolveReferences();

            return JsonConvert.DeserializeObject<T>(root.ToString(), JsonSerializerUtils.Settings);
        }
    }
}