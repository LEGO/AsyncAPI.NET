// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.Json.Nodes;
    using LEGO.AsyncAPI.Exceptions;
    using YamlDotNet.RepresentationModel;

    internal static class JsonHelper
    {
        public static string GetScalarValue(this JsonNode node)
        {
            var scalarNode = node is JsonValue value ? value : throw new AsyncApiException($"Expected scalar value");
            return Convert.ToString(scalarNode.GetValue<object>(), CultureInfo.InvariantCulture);
        }

        public static JsonNode ParseJsonString(string jsonString)
        {
            return JsonNode.Parse(jsonString);
            var reader = new StringReader(jsonString);
            var yamlStream = new YamlStream();
            yamlStream.Load(reader);

            var yamlDocument = yamlStream.Documents.First();
            return yamlDocument.RootNode.ToJsonNode();
        }
    }
}
