using System;
using LEGO.AsyncAPI.Extensions;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Readers.ParseNodes;

namespace LEGO.AsyncAPI.Readers
{
    /// <summary>
    /// Class containing logic to deserialize AsyncApi  document into
    /// runtime AsyncApi object model.
    /// </summary>
    internal static partial class AsyncApiDeserializer
    {
        private static FixedFieldMap<AsyncApiContact> _contactFixedFields = new FixedFieldMap<AsyncApiContact>
        {
            {
                "name", (o, n) =>
                {
                    o.Name = n.GetScalarValue();
                }
            },
            {
                "email", (o, n) =>
                {
                    o.Email = n.GetScalarValue();
                }
            },
            {
                "url", (o, n) =>
                {
                    o.Url = new Uri(n.GetScalarValue(), UriKind.RelativeOrAbsolute);
                }
            },
        };

        private static PatternFieldMap<AsyncApiContact> _contactPatternFields = new PatternFieldMap<AsyncApiContact>
        {
            {s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p,n))}
        };

        public static AsyncApiContact LoadContact(ParseNode node)
        {
            var mapNode = node as MapNode;
            var contact = new AsyncApiContact();

            ParseMap(mapNode, contact, _contactFixedFields, _contactPatternFields);

            return contact;
        }
    }
}