namespace LEGO.AsyncAPI.Validations
{
    /// <summary>
    /// Helper methods to simplify creating validation rules
    /// </summary>
    public static class ValidationContextExtensions
    {
        /// <summary>
        /// Helper method to simplify validation rules
        /// </summary>
        public static void CreateError(this IValidationContext context, string ruleName, string message)
        {
            AsyncApiValidatorError error = new AsyncApiValidatorError(ruleName, context.PathString, message);
            context.AddError(error);
        }

        /// <summary>
        /// Helper method to simplify validation rules
        /// </summary>
        public static void CreateWarning(this IValidationContext context, string ruleName, string message)
        {
            AsyncApiValidatorWarning warning = new AsyncApiValidatorWarning(ruleName, context.PathString, message);
            context.AddWarning(warning);
        }

    }
}