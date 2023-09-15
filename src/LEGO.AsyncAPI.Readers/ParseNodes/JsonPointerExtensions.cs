// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers.ParseNodes
{
    using System;
    using System.Text.Json.Nodes;

    public static class JsonPointerExtensions
    {
        public static JsonNode Find(this JsonPointer currentPointer, JsonNode baseJsonNode)
        {
            if (currentPointer.Tokens.Length == 0)
            {
                return baseJsonNode;
            }

            try
            {
                var pointer = baseJsonNode;
                foreach (var token in currentPointer.Tokens)
                {
                    var sequence = pointer as JsonArray;

                    if (sequence != null && int.TryParse(token, out var tokenValue))
                    {
                        pointer = sequence[tokenValue];
                    }
                    else if (pointer is JsonObject map && !map.TryGetPropertyValue(token, out pointer))
                    {
                        return null;
                    }
                }

                return pointer;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}