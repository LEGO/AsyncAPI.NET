// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System;
    using LEGO.AsyncAPI.Extensions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.ParseNodes;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Class containing logic to deserialize AsyncApi document into
    /// runtime AsyncApi object model.
    /// </summary>
    internal static partial class AsyncApiV2Deserializer
    {
        private static readonly FixedFieldMap<AsyncApiSecurityScheme> securitySchemeFixedFields =
            new()
            {
                {
                    "type", (o, n) => { o.Type = n.GetScalarValue().GetEnumFromDisplayName<SecuritySchemeType>(); }
                },
                {
                    "description", (o, n) => { o.Description = n.GetScalarValue(); }
                },
                {
                    "name", (o, n) => { o.Name = n.GetScalarValue(); }
                },
                {
                    "in", (o, n) => { o.In = n.GetScalarValue().GetEnumFromDisplayName<ParameterLocation>(); }
                },
                {
                    "scheme", (o, n) => { o.Scheme = n.GetScalarValue(); }
                },
                {
                    "bearerFormat", (o, n) => { o.BearerFormat = n.GetScalarValue(); }
                },
                {
                    "flows", (o, n) => { o.Flows = LoadOAuthFlows(n); }
                },
                {
                    "openIdConnectUrl",
                    (o, n) => { o.OpenIdConnectUrl = new Uri(n.GetScalarValue(), UriKind.RelativeOrAbsolute); }
                },
            };

        private static readonly PatternFieldMap<AsyncApiSecurityScheme> securitySchemePatternFields =
            new()
            {
                { s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p, n)) },
            };

        public static AsyncApiSecurityScheme LoadSecurityScheme(ParseNode node)
        {
            var mapNode = node.CheckMapNode("securityScheme");
            var pointer = mapNode.GetReferencePointer();
            if (pointer != null)
            {
                return mapNode.GetReferencedObject<AsyncApiSecurityScheme>(ReferenceType.SecurityScheme, pointer);
            }

            var securityScheme = new AsyncApiSecurityScheme();
            foreach (var property in mapNode)
            {
                property.ParseField(securityScheme, securitySchemeFixedFields, securitySchemePatternFields);
            }

            return securityScheme;
        }
    }
}
