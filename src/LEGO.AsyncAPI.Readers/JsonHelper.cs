// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System;
    using System.Globalization;
    using System.Text.Json.Nodes;
    using LEGO.AsyncAPI.Exceptions;

    internal static class JsonHelper
    {
        public static string GetScalarValue(this JsonNode node)
        {
            var scalarNode = node is JsonValue value ? value : throw new AsyncApiException($"Expected scalar value");
            return Convert.ToString(scalarNode.GetValue<object>(), Configuration.CultureInfo);
        }

        public static JsonNode ParseJsonString(string jsonString)
        {
            return JsonNode.Parse(jsonString);
        }
    }
}
