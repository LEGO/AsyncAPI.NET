namespace LEGO.AsyncAPI.Readers
{
    using System;
    using System.Globalization;
    using System.Text.Json.Nodes;
    using YamlDotNet.Core;
    using YamlDotNet.RepresentationModel;

    internal static class YamlConverter
    {
        public static JsonNode ToJsonNode(this YamlDocument yamlDocument)
        {
            return yamlDocument.RootNode.ToJsonNode();
        }

        public static JsonObject ToJsonObject(this YamlMappingNode yamlMappingNode)
        {
            var node = new JsonObject();
            foreach (var keyValuePair in yamlMappingNode)
            {
                var key = ((YamlScalarNode)keyValuePair.Key).Value!;
                node[key] = keyValuePair.Value.ToJsonNode();
            }

            return node;
        }

        public static JsonArray ToJsonArray(this YamlSequenceNode yaml)
        {
            var node = new JsonArray();
            foreach (var value in yaml)
            {
                node.Add(value.ToJsonNode());
            }

            return node;
        }

        public static JsonNode ToJsonNode(this YamlNode yaml)
        {
            return yaml switch
            {
                YamlMappingNode map => map.ToJsonObject(),
                YamlSequenceNode seq => seq.ToJsonArray(),
                YamlScalarNode scalar => scalar.ToJsonValue(),
                _ => throw new NotSupportedException("This yaml isn't convertible to JSON")
            };
        }

        public static JsonValue ToJsonValue(this YamlScalarNode yaml)
        {
            string value = yaml.Value;

            switch (yaml.Style)
            {
                case ScalarStyle.Plain:
                    // We need to guess the types just based on it's format, so that means parsing
                    if (int.TryParse(value, out int intValue))
                    {
                        return JsonValue.Create<int>(intValue);
                    }

                    if (double.TryParse(value, out double doubleValue))
                    {
                        return JsonValue.Create<double>(doubleValue);
                    }

                    if (DateTime.TryParse(value, out DateTime dateTimeValue))
                    {
                        return JsonValue.Create<DateTime>(dateTimeValue);
                    }

                    if (bool.TryParse(value, out bool boolValue))
                    {
                        return JsonValue.Create<bool>(boolValue);
                    }

                    return JsonValue.Create<string>(value);
                case ScalarStyle.SingleQuoted:
                case ScalarStyle.DoubleQuoted:
                case ScalarStyle.Literal:
                case ScalarStyle.Folded:
                case ScalarStyle.Any:
                    return JsonValue.Create<string>(yaml.Value);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
