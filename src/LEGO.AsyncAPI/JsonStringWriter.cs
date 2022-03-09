// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI
{
    using LEGO.AsyncAPI.NewtonUtils;
    using Newtonsoft.Json;

    /// <summary>
    /// AsyncAPI object to JSON writer.
    /// </summary>
    /// <typeparam name="T">AsyncApi object type.</typeparam>
    public class JsonStringWriter<T> : IStringWriter<T>
    {
        /// <summary>
        /// Serializes AsyncAPI object of type T into JSON string.
        /// </summary>
        /// <param name="object">AsyncAPI object.</param>
        /// <returns>JSON string.</returns>
        public string Write(T @object)
        {
            return JsonConvert.SerializeObject(@object, JsonSerializerUtils.Settings);
        }
    }
}