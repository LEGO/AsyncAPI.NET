// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.NewtonUtils
{
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Mapper from JToken to an object.
    /// </summary>
    internal static class ObjectFromJToken
    {
        /// <summary>
        /// Map from any object to JToken.
        /// </summary>
        /// <typeparam name="T">Type to map to.</typeparam>
        /// <param name="token">JToken value.</param>
        /// <returns>Object of type T.</returns>
        public static T Map<T>(JToken token)
        {
            return token.ToObject<T>(JsonSerializerUtils.Serializer);
        }
    }
}