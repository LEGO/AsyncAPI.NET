// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    /// <summary>
    /// Class containing logic to deserialize AsyncApi document into
    /// runtime AsyncApi object model.
    /// </summary>
    internal static partial class AsyncApiV2Deserializer
    {
        public static AsyncApiSecurityRequirement LoadSecurityRequirement(ParseNode node)
        {
            var mapNode = node.CheckMapNode("security");

            var securityRequirement = new AsyncApiSecurityRequirement();

            foreach (var property in mapNode)
            {
                var scheme = LoadSecuritySchemeByReference(mapNode.Context, property.Name);
                var scopes = property.Value.CreateSimpleList(value => value.GetScalarValue());

                if (scheme != null)
                {
                    securityRequirement.Add(scheme, scopes);
                }
                else
                {
                    mapNode.Context.Diagnostic.Errors.Add(
                        new AsyncApiError(node.Context.GetLocation(), $"Scheme {property.Name} is not found"));
                }
            }

            return securityRequirement;
        }

        private static AsyncApiSecuritySchemeReference LoadSecuritySchemeByReference(
            ParsingContext context,
            string schemeName)
        {
            var securitySchemeObject = new AsyncApiSecuritySchemeReference(schemeName);

            return securitySchemeObject;
        }
    }
}
