// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Bindings.Sns;

using System;
using System.Collections.Generic;
using System.Linq;
using LEGO.AsyncAPI.Models.Interfaces;
using LEGO.AsyncAPI.Readers.ParseNodes;
using LEGO.AsyncAPI.Writers;

public class Condition : IAsyncApiElement
{
    public Dictionary<string, Dictionary<string, StringOrStringList>> Value { get; private set; }

    public Condition(Dictionary<string, Dictionary<string, StringOrStringList>> value)
    {
        this.Value = value;
    }

    public void Serialize(IAsyncApiWriter writer)
    {
        if (writer is null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        writer.WriteStartObject();
        foreach (var conditionValue in this.Value)
        {
            writer.WriteRequiredMap(conditionValue.Key, conditionValue.Value, (w, t) => t.Value.Write(w));
        }

        writer.WriteEndObject();
    }

    public static Condition Parse(ParseNode node)
    {
        switch (node)
        {
            case MapNode mapNode:
            {
                var conditionValues = new Dictionary<string, Dictionary<string, StringOrStringList>>();
                foreach (var conditionNode in mapNode)
                {
                    switch (conditionNode.Value)
                    {
                        case MapNode conditionValueNode:
                            conditionValues.Add(conditionNode.Name, new Dictionary<string, StringOrStringList>(conditionValueNode.Select(x =>
                                    new KeyValuePair<string, StringOrStringList>(x.Name, StringOrStringList.Parse(x.Value)))
                                .ToDictionary(x => x.Key, x => x.Value)));
                            break;
                        default:
                            throw new ArgumentException($"An error occured while parsing a {nameof(Condition)} node. " +
                                                        $"AWS condition values should be one or more key value pairs.");
                    }
                }

                return new Condition(conditionValues);
            }

            default:
                throw new ArgumentException($"An error occured while parsing a {nameof(Condition)} node. " +
                                            $"Node should contain a collection of condition types.");
        }
    }
}