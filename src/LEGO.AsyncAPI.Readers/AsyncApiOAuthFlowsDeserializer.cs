
using LEGO.AsyncAPI.Extensions;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Readers.ParseNodes;

namespace LEGO.AsyncAPI.Readers
{
    /// <summary>
    /// Class containing logic to deserialize AsyncAPI document into
    /// runtime AsyncAPI object model.
    /// </summary>
    internal static partial class AsyncApiDeserializer
    {
        private static readonly FixedFieldMap<AsyncApiOAuthFlows> _oAuthFlowsFixedFileds =
            new FixedFieldMap<AsyncApiOAuthFlows>
            {
                {"implicit", (o, n) => o.Implicit = LoadOAuthFlow(n)},
                {"password", (o, n) => o.Password = LoadOAuthFlow(n)},
                {"clientCredentials", (o, n) => o.ClientCredentials = LoadOAuthFlow(n)},
                {"authorizationCode", (o, n) => o.AuthorizationCode = LoadOAuthFlow(n)}
            };

        private static readonly PatternFieldMap<AsyncApiOAuthFlows> _oAuthFlowsPatternFields =
            new PatternFieldMap<AsyncApiOAuthFlows>
            {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))}
            };

        public static AsyncApiOAuthFlows LoadOAuthFlows(ParseNode node)
        {
            var mapNode = node.CheckMapNode("OAuthFlows");

            var oAuthFlows = new AsyncApiOAuthFlows();
            foreach (var property in mapNode)
            {
                property.ParseField(oAuthFlows, _oAuthFlowsFixedFileds, _oAuthFlowsPatternFields);
            }

            return oAuthFlows;
        }
    }
}