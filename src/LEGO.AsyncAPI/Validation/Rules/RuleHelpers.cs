// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Validation.Rules
{
    using System.Net.Mail;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Any;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Validations;

    internal static class RuleHelpers
    {
        internal const string DataTypeMismatchedErrorMessage = "Data and type mismatch found.";

        /// <summary>
        /// Input string must be in the format of an email address.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>True if it's an email address. Otherwise False.</returns>
        public static bool IsEmailAddress(this string input)
        {
            try
            {
                _ = new MailAddress(input);
            }
            catch (System.Exception)
            {
                return false;
            }

            return true;
        }

        public static void ValidateDataTypeMismatch(
            IValidationContext context,
            string ruleName,
            IAsyncApiAny value,
            AsyncApiSchema schema)
        {
            if (schema == null)
            {
                return;
            }

            var types = EnumExtensions.GetFlags<SchemaType>(schema.Type);
            var format = schema.Format;
            var nullable = schema.Nullable;

            // Before checking the type, check first if the schema allows null.
            // If so and the data given is also null, this is allowed for any type.
            if (nullable)
            {
                if (value is AsyncApiNull)
                {
                    return;
                }
            }

            foreach (var type in types)
            {
                if (type == SchemaType.Object)
                {
                    // It is not against the spec to have a string representing an object value.
                    // To represent examples of media types that cannot naturally be represented in JSON or YAML,
                    // a string value can contain the example with escaping where necessary
                    if (value is AsyncApiString)
                    {
                        return;
                    }

                    // If value is not a string and also not an object, there is a data mismatch.
                    if (!(value is AsyncApiObject))
                    {
                        context.CreateWarning(
                            ruleName,
                            DataTypeMismatchedErrorMessage);
                        return;
                    }

                    var anyObject = (AsyncApiObject)value;

                    foreach (var key in anyObject.Keys)
                    {
                        context.Enter(key);

                        if (schema.Properties != null && schema.Properties.ContainsKey(key))
                        {
                            ValidateDataTypeMismatch(context, ruleName, anyObject[key], schema.Properties[key]);
                        }
                        else
                        {
                            ValidateDataTypeMismatch(context, ruleName, anyObject[key], schema.AdditionalProperties);
                        }

                        context.Exit();
                    }

                    return;
                }

                if (type == SchemaType.Array)
                {
                    // It is not against the spec to have a string representing an array value.
                    // To represent examples of media types that cannot naturally be represented in JSON or YAML,
                    // a string value can contain the example with escaping where necessary
                    if (value is AsyncApiString)
                    {
                        return;
                    }

                    // If value is not a string and also not an array, there is a data mismatch.
                    if (!(value is AsyncApiArray))
                    {
                        context.CreateWarning(
                            ruleName,
                            DataTypeMismatchedErrorMessage);
                        return;
                    }

                    var anyArray = (AsyncApiArray)value;

                    for (int i = 0; i < anyArray.Count; i++)
                    {
                        context.Enter(i.ToString());

                        ValidateDataTypeMismatch(context, ruleName, anyArray[i], schema.Items);

                        context.Exit();
                    }

                    return;
                }

                if (type == SchemaType.Integer && format == "int32")
                {
                    if (!(value is AsyncApiInteger))
                    {
                        context.CreateWarning(
                            ruleName,
                            DataTypeMismatchedErrorMessage);
                    }

                    return;
                }

                if (type == SchemaType.Integer && format == "int64")
                {
                    if (!(value is AsyncApiLong))
                    {
                        context.CreateWarning(
                           ruleName,
                           DataTypeMismatchedErrorMessage);
                    }

                    return;
                }

                if (type == SchemaType.Integer && !(value is AsyncApiInteger))
                {
                    if (!(value is AsyncApiInteger))
                    {
                        context.CreateWarning(
                            ruleName,
                            DataTypeMismatchedErrorMessage);
                    }

                    return;
                }

                if (type == SchemaType.Number && format == "float")
                {
                    if (!(value is AsyncApiFloat))
                    {
                        context.CreateWarning(
                            ruleName,
                            DataTypeMismatchedErrorMessage);
                    }

                    return;
                }

                if (type == SchemaType.Number && format == "double")
                {
                    if (!(value is AsyncApiDouble))
                    {
                        context.CreateWarning(
                            ruleName,
                            DataTypeMismatchedErrorMessage);
                    }

                    return;
                }

                if (type == SchemaType.Number)
                {
                    if (!(value is AsyncApiDouble))
                    {
                        context.CreateWarning(
                            ruleName,
                            DataTypeMismatchedErrorMessage);
                    }

                    return;
                }

                if (type == SchemaType.String && format == "byte")
                {
                    if (!(value is AsyncApiByte))
                    {
                        context.CreateWarning(
                            ruleName,
                            DataTypeMismatchedErrorMessage);
                    }

                    return;
                }

                if (type == SchemaType.String && format == "date")
                {
                    if (!(value is AsyncApiDate))
                    {
                        context.CreateWarning(
                            ruleName,
                            DataTypeMismatchedErrorMessage);
                    }

                    return;
                }

                if (type == SchemaType.String && format == "date-time")
                {
                    if (!(value is AsyncApiDateTime))
                    {
                        context.CreateWarning(
                            ruleName,
                            DataTypeMismatchedErrorMessage);
                    }

                    return;
                }

                if (type == SchemaType.String)
                {
                    if (!(value is AsyncApiString))
                    {
                        context.CreateWarning(
                            ruleName,
                            DataTypeMismatchedErrorMessage);
                    }

                    return;
                }

                if (type == SchemaType.Boolean)
                {
                    if (!(value is AsyncApiBoolean))
                    {
                        context.CreateWarning(
                            ruleName,
                            DataTypeMismatchedErrorMessage);
                    }

                    return;
                }
            }
        }
    }
}
