// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System;
    using System.Globalization;
    using System.Text.Json.Nodes;
    using YamlDotNet.Core;
    using YamlDotNet.RepresentationModel;

    internal static class YamlConverter
    {
        public static JsonNode ToJsonNode(this YamlDocument yamlDocument, AsyncApiReaderSettings settings)
        {
            return yamlDocument.RootNode.ToJsonNode(settings);
        }

        public static JsonObject ToJsonObject(this YamlMappingNode yamlMappingNode, AsyncApiReaderSettings settings)
        {
            var node = new JsonObject();
            foreach (var keyValuePair in yamlMappingNode)
            {
                var key = ((YamlScalarNode)keyValuePair.Key).Value!;
                node[key] = keyValuePair.Value.ToJsonNode(settings);
            }

            return node;
        }

        public static JsonArray ToJsonArray(this YamlSequenceNode yaml, AsyncApiReaderSettings settings)
        {
            var node = new JsonArray();
            foreach (var value in yaml)
            {
                node.Add(value.ToJsonNode(settings));
            }

            return node;
        }

        public static JsonNode ToJsonNode(this YamlNode yaml, AsyncApiReaderSettings settings)
        {
            return yaml switch
            {
                YamlMappingNode map => map.ToJsonObject(settings),
                YamlSequenceNode seq => seq.ToJsonArray(settings),
                YamlScalarNode scalar => scalar.ToJsonValue(settings),
                _ => throw new NotSupportedException("This yaml isn't convertible to JSON"),
            };
        }

        private static JsonValue ToJsonValue(this YamlScalarNode yaml, AsyncApiReaderSettings settings)
        {
            switch (yaml.Style)
            {
                case ScalarStyle.Plain:
                    return decimal.TryParse(yaml.Value, NumberStyles.Float, settings.CultureInfo, out var d)
                        ? JsonValue.Create(d)
                        : bool.TryParse(yaml.Value, out var b)
                            ? JsonValue.Create(b)
                            : JsonValue.Create(yaml.Value)!;
                case ScalarStyle.SingleQuoted:
                case ScalarStyle.DoubleQuoted:
                case ScalarStyle.Literal:
                case ScalarStyle.Folded:
                case ScalarStyle.Any:
                    return JsonValue.Create(yaml.Value);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
