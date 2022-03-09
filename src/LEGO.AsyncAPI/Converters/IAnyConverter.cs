// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Converters
{
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.NewtonUtils;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Converts an IAny object to and from JSON.
    /// </summary>
    internal class IAnyConverter : JsonConverter
    {
        /// <summary>
        /// Convert IAny object to JToken.
        /// </summary>
        /// <param name="writer">JSON writer.</param>
        /// <param name="value">IAny object.</param>
        /// <param name="serializer">JSON serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            IAnyToJToken.Map(value as IAny).WriteTo(writer);
        }

        /// <summary>
        /// Convert JToken object to IAny object.
        /// </summary>
        /// <param name="reader">JSON reader.</param>
        /// <param name="objectType">objectType not used.</param>
        /// <param name="existingValue">existingValue not used.</param>
        /// <param name="serializer">serializer not used.</param>
        /// <returns>IAny object.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return IAnyFromJToken.Map(JToken.Load(reader));
        }

        /// <summary>
        /// Check if possible to convert to IAny.
        /// </summary>
        /// <param name="objectType">objectType.</param>
        /// <returns>True if possible.</returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IAny);
        }
    }
}