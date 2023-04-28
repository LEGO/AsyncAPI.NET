namespace LEGO.AsyncAPI.Readers
{

    using LEGO.AsyncAPI.Models.Bindings.Sns;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    internal static partial class AsyncApiV2Deserializer
    {
        private static FixedFieldMap<SnsChannelBinding> snsChannelBindingFixedFields = new()
        {
            { "name", (a, n) => { a.Name = n.GetScalarValue(); } },
            { "policy", (a, n) => { a.Policy = LoadPolicy(n); } },
        };

        private static FixedFieldMap<Policy> policyFixedFields = new()
        {
            { "statements", (a, n) => { a.Statements = n.CreateSimpleList(s => LoadStatement(s)); } },
        };

        private static FixedFieldMap<Statement> statementFixedFields = new()
        {
            { "principal", (a, n) => { a.Principal = n.GetScalarValue(); } },
        };

        private static Policy LoadPolicy(ParseNode node)
        {
            var mapNode = node.CheckMapNode("policy");
            var policy = new Policy();
            ParseMap(mapNode, policy, policyFixedFields, null);
            return policy;
        }

        private static Statement LoadStatement(ParseNode node)
        {
            var mapNode = node.CheckMapNode("statement");
            var statement = new Statement();
            ParseMap(mapNode, statement, statementFixedFields, null);
            return statement;
        }
    }
}