// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System;
    using LEGO.AsyncAPI.Extensions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    internal static partial class AsyncApiV2Deserializer
    {
        private static FixedFieldMap<AsyncApiInfo> infoFixedFields = new ()
        {
            { "title", (a, n) => { a.Title = n.GetScalarValue(); } },
            { "version", (a, n) => { a.Version = n.GetScalarValue(); } },
            { "description", (a, n) => { a.Description = n.GetScalarValue(); } },
            { "termsOfService", (a, n) => { a.TermsOfService = new Uri(n.GetScalarValue()); } },
            { "contact", (a, n) => { a.Contact = LoadContact(n); } },
            { "license", (a, n) => { a.License = LoadLicense(n); } },
        };

        private static PatternFieldMap<AsyncApiInfo> infoPatternFields =
            new ()
            {
                { s => s.StartsWith("x-"), (a, p, n) => a.AddExtension(p, LoadExtension(p, n)) },
            };

        public static AsyncApiInfo LoadInfo(ParseNode node)
        {
            var mapNode = node.CheckMapNode("info");
            var info = new AsyncApiInfo();

            ParseMap(mapNode, info, infoFixedFields, infoPatternFields);

            return info;
        }
    }
}
