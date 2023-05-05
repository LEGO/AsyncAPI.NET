using System;
using System.Collections.Generic;
using System.Linq;
using LEGO.AsyncAPI.Bindings;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Readers.ParseNodes;
using LEGO.AsyncAPI.Writers;
using YamlDotNet.Core.Tokens;

namespace LEGO.AsyncAPI.Bindings.Sns;

using LEGO.AsyncAPI.Models.Interfaces;

/// <summary>
/// Binding class for SNS channel settings.
/// </summary>
public class SnsChannelBinding : ChannelBinding<SnsChannelBinding>
{
    /// <summary>
    /// The name of the topic. Can be different from the channel name to allow flexibility around AWS resource naming limitations.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// By default, we assume an unordered SNS topic. This field allows configuration of a FIFO SNS Topic.
    /// </summary>
    public OrderingConfiguration Ordering { get; set; }

    /// <summary>
    /// The security policy for the SNS Topic.
    /// </summary>
    public Policy Policy { get; set; }

    /// <summary>
    /// Key-value pairs that represent AWS tags on the topic.
    /// </summary>
    public Dictionary<string, string> Tags { get; set; }

    public override string BindingKey => "sns";

    /// <summary>
    /// The version of this binding.
    /// </summary>
    public string BindingVersion { get; set; }

    protected override FixedFieldMap<SnsChannelBinding> FixedFieldMap => new()
    {
        { "name", (a, n) => { a.Name = n.GetScalarValue(); } },
        { "policy", (a, n) => { a.Policy = LoadPolicy(n); } },
    };

    private static FixedFieldMap<Policy> policyFixedFields = new()
    {
        { "statements", (a, n) => { a.Statements = n.CreateList(s => LoadStatement(s)); } },
    };

    private static FixedFieldMap<Statement> statementFixedFields = new()
    {
        { "effect", (a, n) => { a.Effect = n.GetScalarValue().GetEnumFromDisplayName<Effect>(); } },
        { "principal", (a, n) => { a.Principal = LoadStringOrStringList(n, "principal"); } },
        { "action", (a, n) => { a.Action = LoadStringOrStringList(n, "action"); } },
    };

    private static Policy LoadPolicy(ParseNode node)
    {
        var mapNode = node.CheckMapNode("policy");
        var policy = new Policy();
        ParseMap(mapNode, policy, policyFixedFields);
        return policy;
    }
    
    private static Statement LoadStatement(ParseNode node)
    {
        var mapNode = node.CheckMapNode("statement");
        var statement = new Statement();
        ParseMap(mapNode, statement, statementFixedFields);
        return statement;
    }

    public static Effect LoadEffectEnum(ParseNode node)
    {
        Enum.TryParse(node.GetScalarValue(), out Effect effect);
        return effect;
    }

    public static StringOrStringList LoadStringOrStringList(ParseNode node, string nodeName)
    {
        var stringOrStringList = new StringOrStringList();
        if (node is ValueNode v)
        {
            stringOrStringList.StringValue = v.GetScalarValue();
        }

        if (node is ListNode m)
        {
            stringOrStringList.StringList = m.CreateList(s => s.GetScalarValue());
        }

        return stringOrStringList;
    }

    /// <inheritdoc/>
    public override void SerializeProperties(IAsyncApiWriter writer)
    {
        if (writer is null)
        {
            throw new ArgumentNullException(nameof(writer));
        }

        writer.WriteStartObject();
        writer.WriteOptionalProperty(AsyncApiConstants.Name, this.Name);
        writer.WriteOptionalObject(AsyncApiConstants.Policy, this.Policy, (w, t) => t.Serialize(w));
        writer.WriteEndObject();
    }
}