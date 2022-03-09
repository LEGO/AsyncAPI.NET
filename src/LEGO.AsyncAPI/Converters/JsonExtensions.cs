// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Converters
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    internal static class JsonExtensions
    {
        public static JsonReader MoveToContent(this JsonReader reader)
        {
            if (reader.TokenType == JsonToken.None)
            {
                reader.Read();
            }

            while (reader.TokenType == JsonToken.Comment && reader.Read())
            {
            }

            return reader;
        }

        public static JToken RemoveFromLowestPossibleParent(this JToken node)
        {
            if (node == null)
            {
                return null;
            }

            var contained = node.AncestorsAndSelf().FirstOrDefault(t => t.Parent is not null && t.Parent.Type != JTokenType.Property);
            contained?.Remove();

            // Also detach the node from its immediate containing property -- Remove() does not do this even though it seems like it should
            if (node.Parent is JProperty)
            {
                ((JProperty)node.Parent).Value = null;
            }

            return node;
        }
    }
}
