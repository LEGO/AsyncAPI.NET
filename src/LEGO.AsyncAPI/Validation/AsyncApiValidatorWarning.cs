namespace LEGO.AsyncAPI.Validations
{
    using LEGO.AsyncAPI.Models;

    /// <summary>
    /// Warnings detected when validating an AsyncApi Element
    /// </summary>
    public class AsyncApiValidatorWarning : AsyncApiError
    {
        /// <summary>
        /// Initializes the <see cref="AsyncApiError"/> class.
        /// </summary>
        public AsyncApiValidatorWarning(string ruleName, string pointer, string message)
            : base(pointer, message)
        {
            this.RuleName = ruleName;
        }

        /// <summary>
        /// Name of rule that detected the error.
        /// </summary>
        public string RuleName { get; set; }
    }
}