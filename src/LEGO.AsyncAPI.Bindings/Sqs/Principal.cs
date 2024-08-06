namespace LEGO.AsyncAPI.Bindings.Sqs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncAPI.Readers.ParseNodes;

public class Principal : IAsyncApiElement
{
    public Principal(AsyncApiAny value)
    {
        this.Value = value;
    }

    public AsyncApiAny Value { get; }

    public static Principal Parse(ParseNode node)
    {
        switch (node)
        {
            case ValueNode:
                var nodeValue = node.GetScalarValue();
                if (!IsStarString(nodeValue))
                {
                    throw new ArgumentException($"An error occured while parsing a {nameof(Principal)} node. " +
                                                $"Principal value without a property name can only be a string value of '*'.");
                }

                return new Principal(new AsyncApiAny(nodeValue));
            case MapNode mapNode:
            {
                var propertyNode = mapNode.First();
                if (!IsValidPrincipalProperty(propertyNode.Name))
                {
                    throw new ArgumentException($"An error occured while parsing a {nameof(Principal)} node. " +
                                                $"Node should contain a valid AWS principal property name.");
                }

                var parsedObject = new Dictionary<string, AsyncApiAny>()
                    { { propertyNode.Name, StringOrStringList.Parse(propertyNode.Value).Value } };

                return new Principal(new AsyncApiAny(parsedObject));
            }

            default:
                throw new ArgumentException($"An error occured while parsing a {nameof(Principal)} node. " +
                                            $"Node should contain a string value of '*' or a valid AWS principal property.");
        }
    }

    private static bool IsStarString(JsonNode value)
    {
        var element = JsonDocument.Parse(value.ToJsonString()).RootElement;

        return element.ValueKind == JsonValueKind.String && element.ValueEquals("*");
    }

    private static bool IsValidPrincipalProperty(string property)
    {
        return new[] { "AWS", "Service" }.Contains(property);
    }
}