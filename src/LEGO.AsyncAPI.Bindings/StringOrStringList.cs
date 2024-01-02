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
            switch (value.GetNode())
            {
                case JsonArray array:
                    this.Value = IsValidStringList(array)
                        ? new AsyncApiAny(array)
                        : throw new ArgumentException($"{nameof(StringOrStringList)} value should only contain string items.");
                    break;
                case JsonValue jValue:
                    this.Value = IsString(jValue)
                        ? new AsyncApiAny(jValue)
                        : throw new ArgumentException($"{nameof(StringOrStringList)} should be a string value or a string list.");
                    break;
                default:
                    throw new ArgumentException($"{nameof(StringOrStringList)} should be a string value or a string list.");
            }
        }

        public AsyncApiAny Value { get; }

        public static StringOrStringList Parse(ParseNode node)
        {
            switch (node)
            {
                case ValueNode vn:
                    return new StringOrStringList(new AsyncApiAny(vn.GetScalarValue()));
                case ListNode ln:
                {
                    var jsonArray = new JsonArray();
                    foreach (var item in ln)
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