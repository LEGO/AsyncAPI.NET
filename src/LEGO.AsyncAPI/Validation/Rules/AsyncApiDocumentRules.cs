﻿namespace LEGO.AsyncAPI.Validation.Rules
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Validations;

    [AsyncApiRule]
    public static class AsyncApiDocumentRules
    {
        /// <summary>
        /// The key regex.
        /// </summary>
        public static Regex KeyRegex = new Regex(@"^[a-zA-Z0-9\.\-_]+$");

        public static ValidationRule<AsyncApiDocument> AsyncApiDocumentRequiredFields =>
            new ValidationRule<AsyncApiDocument>(
                (context, document) =>
                {
                    // info
                    context.Enter("info");
                    if (document.Info == null)
                    {
                        context.CreateError(
                            nameof(AsyncApiDocumentRequiredFields),
                            string.Format(Resource.Validation_FieldRequired, "info", "document"));
                    }

                    context.Exit();

                    // channels
                    context.Enter("channels");
                    if (document.Channels == null)
                    {
                        context.CreateError(
                            nameof(AsyncApiDocumentRequiredFields),
                            string.Format(Resource.Validation_FieldRequired, "channels", "document"));
                    }

                    context.Exit();
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
