using System;
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
        private static readonly FixedFieldMap<AsyncApiOAuthFlow> _oAuthFlowFixedFileds =
            new FixedFieldMap<AsyncApiOAuthFlow>
            {
                {
                    "authorizationUrl", (o, n) =>
                    {
                        o.AuthorizationUrl = new Uri(n.GetScalarValue(), UriKind.RelativeOrAbsolute);
                    }
                },
                {
                    "tokenUrl", (o, n) =>
                    {
                        o.TokenUrl = new Uri(n.GetScalarValue(), UriKind.RelativeOrAbsolute);
                    }
                },
                {
                    "refreshUrl", (o, n) =>
                    {
                        o.RefreshUrl = new Uri(n.GetScalarValue(), UriKind.RelativeOrAbsolute);
                    }
                },
                {"scopes", (o, n) => o.Scopes = n.CreateSimpleMap(LoadString)}
            };

        private static readonly PatternFieldMap<AsyncApiOAuthFlow> _oAuthFlowPatternFields =
            new PatternFieldMap<AsyncApiOAuthFlow>
            {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))}
            };

        public static AsyncApiOAuthFlow LoadOAuthFlow(ParseNode node)
        {
            var mapNode = node.CheckMapNode("OAuthFlow");

            var oauthFlow = new AsyncApiOAuthFlow();
            foreach (var property in mapNode)
            {
                property.ParseField(oauthFlow, _oAuthFlowFixedFileds, _oAuthFlowPatternFields);
            }

            return oauthFlow;
        }
    }
}
