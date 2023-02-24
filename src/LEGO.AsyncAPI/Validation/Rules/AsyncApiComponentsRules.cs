namespace LEGO.AsyncAPI.Validation.Rules
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Validations;

    [AsyncApiRule]
    public static class AsyncApiComponentsRules
    {
        /// <summary>
        /// The key regex.
        /// </summary>
        public static Regex KeyRegex = new Regex(@"^[a-zA-Z0-9\.\-_]+$");

        /// <summary>
        /// All the fixed fields declared above are objects
        /// that MUST use keys that match the regular expression: ^[a-zA-Z0-9\.\-_]+$.
        /// </summary>
        public static ValidationRule<AsyncApiComponents> KeyMustBeRegularExpression =>
            new ValidationRule<AsyncApiComponents>(
                (context, components) =>
                {
                    ValidateKeys(context, components.Channels?.Keys, "channels");
                    ValidateKeys(context, components.ChannelBindings?.Keys, "channelbindings");
                    ValidateKeys(context, components.CorrelationIds?.Keys, "correlationIds");
                    ValidateKeys(context, components.MessageBindings?.Keys, "messageBindings");
                    ValidateKeys(context, components.Messages?.Keys, "messages");
                    ValidateKeys(context, components.MessageTraits?.Keys, "messageTraits");
                    ValidateKeys(context, components.OperationBindings?.Keys, "operationBindings");
                    ValidateKeys(context, components.OperationTraits?.Keys, "operationTraits");
                    ValidateKeys(context, components.Parameters?.Keys, "parameters");
                    ValidateKeys(context, components.Schemas?.Keys, "schemas");
                    ValidateKeys(context, components.SecuritySchemes?.Keys, "securitySchemes");
                    ValidateKeys(context, components.ServerBindings?.Keys, "serverBindings");
                    ValidateKeys(context, components.Servers?.Keys, "servers");
                    ValidateKeys(context, components.ServerVariables?.Keys, "serverVariables");
                });

        private static void ValidateKeys(IValidationContext context, IEnumerable<string> keys, string component)
        {
            if (keys == null)
            {
                return;
            }

            foreach (var key in keys)
            {
                if (!KeyRegex.IsMatch(key))
                {
                    context.CreateError(
                        nameof(KeyMustBeRegularExpression),
                        string.Format(Resource.Validation_KeyMustMatchRegularExpr, key, component, KeyRegex.ToString()));
                }
            }
        }
    }
}
