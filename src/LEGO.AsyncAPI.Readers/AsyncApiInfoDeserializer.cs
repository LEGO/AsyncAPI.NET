using System;
using LEGO.AsyncAPI.Extensions;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Readers.ParseNodes;

namespace LEGO.AsyncAPI.Readers
{
    internal static partial class AsyncApiDeserializer
    {
        private static FixedFieldMap<AsyncApiInfo> _infoFixedFields = new ()
        {
            { "title", (a, n) => { a.Title = n.GetScalarValue(); } },
            { "version", (a, n) => { a.Version = n.GetScalarValue(); } },
            { "description", (a, n) => { a.Description = n.GetScalarValue(); } },
            { "termsOfService", (a, n) => { a.TermsOfService = new Uri(n.GetScalarValue()); } },
            { "contact", (a, n) => { a.Contact = LoadContact(n); } },
            { "license", (a, n) => { a.License = LoadLicense(n); } },
        };
        
        private static PatternFieldMap<AsyncApiInfo> _infoPatternFields =
            new ()
            {
                { s => s.StartsWith("x-"), (a, p, n) => a.AddExtension(p, LoadExtension(p, n)) }
            };

        public static AsyncApiInfo LoadInfo(ParseNode node)
        {
            var mapNode = node.CheckMapNode("traits");
            var info = new AsyncApiInfo();

            ParseMap(mapNode, info, _infoFixedFields, _infoPatternFields);

            return info;
        }
    }
}