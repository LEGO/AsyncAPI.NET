// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Validation.Rules
{
    using System;
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
                        if (document.Channels == null || !document.Channels.Keys.Any())
                        {
                            context.CreateError(
                                nameof(DocumentRequiredFields),
                                string.Format(Resource.Validation_FieldRequired, "channels", "document"));
                            return;
                        }

                        foreach (var key in document.Channels.Keys)
                        {
                            if (!ChannelKeyUriTemplateRegex.IsMatch(key))
                            {
                                context.CreateError(
                                    "ChannelKeys",
                                    string.Format(Resource.Validation_KeyMustMatchRegularExpr, key, "channels", KeyRegex.ToString()));
                            }
                        }
                    }
                    finally
                    {
                        context.Exit();
                    }                    
                });

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
