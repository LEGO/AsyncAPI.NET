// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Validation.Rules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Validations;

    [AsyncApiRule]
    public static class AsyncApiDocumentRules
    {
        private static TimeSpan RegexTimeout = TimeSpan.FromSeconds(1);

        /// <summary>
        /// The key regex.
        /// </summary>
        public static Regex KeyRegex = new Regex(@"^[a-zA-Z0-9\.\-_]+$", RegexOptions.None, RegexTimeout);
        public static Regex ChannelKeyUriTemplateRegex = new Regex(@"^(?:(?:[^\x00-\x20""'<>%\\^`{|}]|%[0-9a-f]{2})|\{[+#./;?&=,!@|]?(?:[a-z0-9_]|%[0-9a-f]{2})+(?::[1-9][0-9]{0,3}|\*)?(?:,(?:[a-z0-9_]|%[0-9a-f]{2})+(?::[1-9][0-9]{0,3}|\*)?)*\})*$", RegexOptions.IgnoreCase, RegexTimeout);

        public static ValidationRule<AsyncApiDocument> DocumentRequiredFields =>
            new ValidationRule<AsyncApiDocument>(
                (context, document) =>
                {
                    context.Enter("info");
                    if (document.Info == null)
                    {
                        context.CreateError(
                            nameof(DocumentRequiredFields),
                            string.Format(Resource.Validation_FieldRequired, "info", "document"));
                    }

                    context.Exit();

                    context.Enter("channels");
                    try
                    {
                        // MUST have at least 1 channel
                        if (document.Channels == null || !document.Channels.Keys.Any())
                        {
                            context.CreateError(
                                nameof(DocumentRequiredFields),
                                string.Format(Resource.Validation_FieldRequired, "channels", "document"));
                            return;
                        }
                        var hashSet = new HashSet<string>();
                        foreach (var key in document.Channels.Keys)
                        {
                            // Uri-template
                            if (!ChannelKeyUriTemplateRegex.IsMatch(key))
                            {
                                context.CreateError(
                                    "ChannelKeys",
                                    string.Format(Resource.Validation_KeyMustMatchRegularExpr, key, "channels", KeyRegex.ToString()));
                            }

                            // Unique channel keys
                            var pathSignature = GetKeySignature(key);
                            if (!hashSet.Add(pathSignature))
                            {
                                context.CreateError("ChannelKey", string.Format(Resource.Validation_ChannelsMustBeUnique, pathSignature));
                            }
                        }
                    }
                    finally
                    {
                        context.Exit();
                    }                    
                });

        private static string GetKeySignature(string path)
        {
            for (int openBrace = path.IndexOf('{'); openBrace > -1; openBrace = path.IndexOf('{', openBrace + 2))
            {
                int closeBrace = path.IndexOf('}', openBrace);

                if (closeBrace < 0)
                {
                    return path;
                }

                path = path.Substring(0, openBrace + 1) + path.Substring(closeBrace);
            }

            return path;
        }

        public static ValidationRule<AsyncApiDocument> KeyMustBeRegularExpression =>
            new ValidationRule<AsyncApiDocument>(
                (context, document) =>
                {
                    if (document.Servers?.Keys == null)
                    {
                        return;
                    }

                    context.Enter("servers");
                    foreach (var key in document.Servers?.Keys)
                    {
                        if (!KeyRegex.IsMatch(key))
                        {
                            context.CreateError(
                                nameof(KeyMustBeRegularExpression),
                                string.Format(Resource.Validation_KeyMustMatchRegularExpr, key, "servers", KeyRegex.ToString()));
                        }
                    }

                    context.Exit();
                });
    }
}
