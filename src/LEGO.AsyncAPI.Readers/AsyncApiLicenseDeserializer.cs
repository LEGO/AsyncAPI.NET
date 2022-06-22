using System;
using LEGO.AsyncAPI.Extensions;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Readers.ParseNodes;

namespace LEGO.AsyncAPI.Readers
{
    internal static partial class AsyncApiDeserializer
    {
        private static FixedFieldMap<AsyncApiLicense> _licenseFixedFields = new ()
        {
            { "name", (a, n) => { a.Name = n.GetScalarValue(); } },
            { "url", (a, n) => { a.Url = new Uri(n.GetScalarValue()); } },
        };
        
        private static PatternFieldMap<AsyncApiLicense> _licensePatternFields =
            new ()
            {
                { s => s.StartsWith("x-"), (a, p, n) => a.AddExtension(p, LoadExtension(p, n)) }
            };

        public static AsyncApiLicense LoadLicense(ParseNode node)
        {
            var mapNode = node.CheckMapNode("license");
            var license = new AsyncApiLicense();

            ParseMap(mapNode, license, _licenseFixedFields, _licensePatternFields);

            return license;
        }
    }
}
