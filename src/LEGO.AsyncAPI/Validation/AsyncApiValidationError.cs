// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Validations
{
    using LEGO.AsyncAPI.Models;

    /// <summary>
    /// Errors detected when validating an AsyncApi Element
    /// </summary>
    public class AsyncApiValidatorError : AsyncApiError
    {
        /// <summary>
        /// Initializes the <see cref="AsyncApiError"/> class.
        /// </summary>
        public AsyncApiValidatorError(string ruleName, string pointer, string message)
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