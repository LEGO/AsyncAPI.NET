using LEGO.AsyncAPI.Models;

namespace LEGO.AsyncAPI.Validations
{
    /// <summary>
    /// Errors detected when validating an OpenAPI Element
    /// </summary>
    public class AsyncApiValidatorError : AsyncApiError
    {
        /// <summary>
        /// Initializes the <see cref="AsyncApiError"/> class.
        /// </summary>
        public AsyncApiValidatorError(string ruleName, string pointer, string message) : base(pointer, message)
        {
            this.RuleName = ruleName;
        }

        /// <summary>
        /// Name of rule that detected the error.
        /// </summary>
        public string RuleName { get; set; }
    }
}