// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Validation.Rules
{
    using System.Linq;
    using System.Text.RegularExpressions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Validations;

    [AsyncApiRule]
    public static class AsyncApiAvroRules
    {
        /// <summary>
        /// The key regex.
        /// </summary>
        public static Regex NameRegex = new Regex(@"^[A-Za-z_][A-Za-z0-9_]*$");

        public static ValidationRule<AsyncApiAvroSchemaPayload> NameRegularExpression =>
            new ValidationRule<AsyncApiAvroSchemaPayload>(
                (context, avroPayload) =>
                {
                    string name = null;
                    context.Enter("name");
                    if (avroPayload.TryGetAs<AvroRecord>(out var record))
                    {
                        name = record.Name;
                    }

                    if (avroPayload.TryGetAs<AvroEnum>(out var @enum))
                    {
                        name = @enum.Name;
                        if (@enum.Symbols.Any(symbol => !NameRegex.IsMatch(symbol)))
                        {
                            context.CreateError(
                                "SymbolsRegularExpression",
                                string.Format(Resource.Validation_SymbolsMustMatchRegularExpression, NameRegex.ToString()));
                        }
                    }

                    if (avroPayload.TryGetAs<AvroFixed>(out var @fixed))
                    {
                        name = @fixed.Name;
                    }

                    if (name == null)
                    {
                        return;
                    }

                    if (!NameRegex.IsMatch(record.Name))
                    {
                        context.CreateError(
                            nameof(NameRegex),
                            string.Format(Resource.Validation_NameMustMatchRegularExpr, name, NameRegex.ToString()));
                    }

                    context.Exit();
                });
    }
}
