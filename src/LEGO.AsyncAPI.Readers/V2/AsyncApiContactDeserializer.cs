// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Readers
{
    using System;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers.ParseNodes;

    /// <summary>
    /// Class containing logic to deserialize AsyncApi  document into
    /// runtime AsyncApi object model.
    /// </summary>
    internal static partial class AsyncApiV2Deserializer
    {
        private static FixedFieldMap<AsyncApiContact> contactFixedFields = new()
        {
            { "name", (o, n) => { o.Name = n.GetScalarValue(); } },
            { "email", (o, n) => { o.Email = n.GetScalarValue(); } },
            { "url", (o, n) => { o.Url = new Uri(n.GetScalarValue(), UriKind.RelativeOrAbsolute); } },
        };

        private static PatternFieldMap<AsyncApiContact> contactPatternFields = new()
        {
            { s => s.StartsWith("x-"), (o, p, n) => o.AddExtension(p, LoadExtension(p, n)) },
        };

        public static AsyncApiContact LoadContact(ParseNode node)
        {
            var mapNode = node as MapNode;
            var contact = new AsyncApiContact();

            ParseMap(mapNode, contact, contactFixedFields, contactPatternFields);

            return contact;
        }
    }
}
