// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System;
    using LEGO.AsyncAPI.Extensions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    internal static partial class AsyncApiV2Deserializer
    {
        private static FixedFieldMap<AsyncApiLicense> licenseFixedFields = new ()
        {
            { "name", (a, n) => { a.Name = n.GetScalarValue(); } },
            { "url", (a, n) => { a.Url = new Uri(n.GetScalarValue()); } },
        };

        private static PatternFieldMap<AsyncApiLicense> licensePatternFields =
            new ()
            {
                { s => s.StartsWith("x-"), (a, p, n) => a.AddExtension(p, LoadExtension(p, n)) },
            };

        public static AsyncApiLicense LoadLicense(ParseNode node)
        {
            var mapNode = node.CheckMapNode("license");
            var license = new AsyncApiLicense();

            ParseMap(mapNode, license, licenseFixedFields, licensePatternFields);

            return license;
        }
    }
}
