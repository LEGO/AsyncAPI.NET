// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System;
    using LEGO.AsyncAPI.Extensions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    /// <summary>
    /// Class containing logic to deserialize AsyncAPI document into
    /// runtime AsyncAPI object model.
    /// </summary>
    internal static partial class AsyncApiDeserializer
    {
        private static readonly FixedFieldMap<AsyncApiOAuthFlow> oAuthFlowFixedFields =
            new()
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
                { "scopes", (o, n) => o.Scopes = n.CreateSimpleMap(LoadString) },
            };

        private static readonly PatternFieldMap<AsyncApiOAuthFlow> oAuthFlowPatternFields =
            new()
            {
                { s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n)) },
            };

        public static AsyncApiOAuthFlow LoadOAuthFlow(ParseNode node)
        {
            var mapNode = node.CheckMapNode("OAuthFlow");

            var oauthFlow = new AsyncApiOAuthFlow();
            foreach (var property in mapNode)
            {
                property.ParseField(oauthFlow, oAuthFlowFixedFields, oAuthFlowPatternFields);
            }

            return oauthFlow;
        }
    }
}
