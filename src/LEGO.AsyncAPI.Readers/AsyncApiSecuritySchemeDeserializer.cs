using System;
using LEGO.AsyncAPI.Extensions;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Readers.ParseNodes;
using LEGO.AsyncAPI.Writers;

namespace LEGO.AsyncAPI.Readers
{
    /// <summary>
    /// Class containing logic to deserialize AsyncApi document into
    /// runtime AsyncApi object model.
    /// </summary>
    internal static partial class AsyncApiDeserializer
    {
        private static readonly FixedFieldMap<AsyncApiSecurityScheme> _securitySchemeFixedFields =
            new FixedFieldMap<AsyncApiSecurityScheme>
            {
                {
                    "type", (o, n) =>
                    {
                        o.Type = n.GetScalarValue().GetEnumFromDisplayName<SecuritySchemeType>();
                    }
                },
                {
                    "description", (o, n) =>
                    {
                        o.Description = n.GetScalarValue();
                    }
                },
                {
                    "name", (o, n) =>
                    {
                        o.Name = n.GetScalarValue();
                    }
                },
                {
                    "in", (o, n) =>
                    {
                        o.In = n.GetScalarValue().GetEnumFromDisplayName<ParameterLocation>();
                    }
                },
                {
                    "scheme", (o, n) =>
                    {
                        o.Scheme = n.GetScalarValue();
                    }
                },
                {
                    "bearerFormat", (o, n) =>
                    {
                        o.BearerFormat = n.GetScalarValue();
                    }
                },
                {
                    "openIdConnectUrl", (o, n) =>
                    {
                        o.OpenIdConnectUrl = new Uri(n.GetScalarValue(), UriKind.RelativeOrAbsolute);
                    }
                },
                {
                    "flows", (o, n) =>
                    {
                        o.Flows = LoadOAuthFlows(n);
                    }
                }
            };

        private static readonly PatternFieldMap<AsyncApiSecurityScheme> _securitySchemePatternFields =
            new PatternFieldMap<AsyncApiSecurityScheme>
            {
                {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))}
            };

        public static AsyncApiSecurityScheme LoadSecurityScheme(ParseNode node)
        {
            var mapNode = node.CheckMapNode("securityScheme");

            var securityScheme = new AsyncApiSecurityScheme();
            foreach (var property in mapNode)
            {
                property.ParseField(securityScheme, _securitySchemeFixedFields, _securitySchemePatternFields);
            }

            return securityScheme;
        }
    }
}
