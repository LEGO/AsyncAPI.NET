using System;
using System.Collections.Generic;
using System.Text;
using LEGO.AsyncAPI.Models.Exceptions;

namespace LEGO.AsyncApi.Validations
{    
    /// <summary>
    /// Warnings detected when validating an OpenAPI Element
    /// </summary>
    public class AsyncApiValidatorWarning : AsyncApiError
    {
        /// <summary>
        /// Initializes the <see cref="AsyncApiError"/> class.
        /// </summary>
        public AsyncApiValidatorWarning(string ruleName, string pointer, string message) : base(pointer, message)
        {
            RuleName = ruleName;
        }

        /// <summary>
        /// Name of rule that detected the error.
        /// </summary>
        public string RuleName { get; set; }
    }

}