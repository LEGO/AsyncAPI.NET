// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text.Encodings.Web;
    using System.Text.Json;
    using System.Text.Json.Nodes;
    using LEGO.AsyncAPI.Exceptions;

    /// <summary>
    /// Contains helper methods for working with Json 
    /// </summary>
    internal static class JsonHelper
    {
        private static readonly JsonWriterOptions WriterOptions;

        static JsonHelper()
        {
            WriterOptions = new JsonWriterOptions()
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                Indented = false,
                MaxDepth = 1,
                SkipValidation = true,
            };
        }

        /// <summary>
        /// Takes a <see cref="JsonValue"/> and converts it into a string value.
        /// </summary>
        /// <param name="jsonValue">The node to convert.</param>
        /// <returns>The string value.</returns>
        public static string GetScalarValue(this JsonValue jsonValue)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            using (Utf8JsonWriter writer = new Utf8JsonWriter(memoryStream, WriterOptions))
            {
                jsonValue.WriteTo(writer);
                writer.Flush();
                memoryStream.Position = 0;
                using (StreamReader reader = new StreamReader(memoryStream))
                {
                    string value = reader.ReadToEnd();
                    return value.Trim('"');
                }
            }
        }

        public static JsonNode ParseJsonString(string jsonString)
        {
            return JsonNode.Parse(jsonString);
        }
    }
}
