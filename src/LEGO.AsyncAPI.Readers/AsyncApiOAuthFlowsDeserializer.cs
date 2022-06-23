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
            new ()
            {
                { "implicit", (a, n) => a.Implicit = LoadOAuthFlow(n) },
                { "password", (a, n) => a.Password = LoadOAuthFlow(n) },
                { "clientCredentials", (a, n) => a.ClientCredentials = LoadOAuthFlow(n) },
                { "authorizationCode", (a, n) => a.AuthorizationCode = LoadOAuthFlow(n) }
            };

        private static readonly PatternFieldMap<AsyncApiOAuthFlows> _oAuthFlowsPatternFields =
            new ()
            {
                { s => s.StartsWith("x-"), (a, p, n) => a.AddExtension(p, LoadExtension(p, n)) }
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