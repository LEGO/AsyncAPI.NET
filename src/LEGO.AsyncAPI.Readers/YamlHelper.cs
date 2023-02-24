// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System.IO;
    using System.Linq;
    using LEGO.AsyncAPI.Exceptions;
    using YamlDotNet.RepresentationModel;

    internal static class YamlHelper
    {
        public static string GetScalarValue(this YamlNode node)
        {
            var scalarNode = node as YamlScalarNode;
            if (scalarNode == null)
            {
                throw new AsyncApiException($"Expected scalar at line {node.Start.Line}");
            }

            return scalarNode.Value;
        }

        public static YamlNode ParseYamlString(string yamlString)
        {
            var reader = new StringReader(yamlString);
            var yamlStream = new YamlStream();
            yamlStream.Load(reader);

            var yamlDocument = yamlStream.Documents.First();
            return yamlDocument.RootNode;
        }
    }
}
