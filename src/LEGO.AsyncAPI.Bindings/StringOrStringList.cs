// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Bindings
{
    using System;
    using System.Linq;
    using System.Text.Json;
    using System.Text.Json.Nodes;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    public class StringOrStringList : IAsyncApiElement
    {
        public StringOrStringList(AsyncApiAny value)
        {
            this.Value = value.GetNode() switch
            {
                JsonArray array => IsValidStringList(array) ? new AsyncApiAny(array) : throw new ArgumentException($"{nameof(StringOrStringList)} value should only contain string items."),
                JsonValue jValue => IsString(jValue) ? new AsyncApiAny(jValue) : throw new ArgumentException($"{nameof(StringOrStringList)} should be a string value or a string list."),
                _ => throw new ArgumentException($"{nameof(StringOrStringList)} should be a string value or a string list."),
            };
        }

        public AsyncApiAny Value { get; }

        public static StringOrStringList Parse(ParseNode node)
        {
            switch (node)
            {
                case ValueNode:
                    return new StringOrStringList(new AsyncApiAny(node.GetScalarValue()));
                case ListNode:
                    {
                        var jsonArray = new JsonArray();
                        foreach (var item in node as ListNode)
                        {
                            jsonArray.Add(item.GetScalarValue());
                        }

                        return new StringOrStringList(new AsyncApiAny(jsonArray));
                    }

                default:
                    throw new ArgumentException($"An error occured while parsing a {nameof(StringOrStringList)} node. " +
                                                $"Node should contain a string value or a list of strings.");
            }
        }

        private static bool IsString(JsonNode value)
        {
            var element = JsonDocument.Parse(value.ToJsonString()).RootElement;
            return element.ValueKind == JsonValueKind.String;
        }

        private static bool IsValidStringList(JsonArray array)
        {
            return array.All(x => IsString(x));
        }
    }
}