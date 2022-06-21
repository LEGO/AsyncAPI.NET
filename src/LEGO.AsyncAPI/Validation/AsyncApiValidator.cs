using System;
using System.Collections.Generic;
using System.Linq;
using LEGO.AsyncAPI.Models;

namespace LEGO.AsyncAPI.Validations
{
    /// <summary>
    /// Class containing dispatchers to execute validation rules on for Open API document.
    /// </summary>
    public class AsyncApiValidator : AsyncApiVisitorBase, IValidationContext
    {
        private readonly ValidationRuleSet _ruleSet;
        private readonly IList<AsyncApiValidatorError> _errors = new List<AsyncApiValidatorError>();
        private readonly IList<AsyncApiValidatorWarning> _warnings = new List<AsyncApiValidatorWarning>();

        /// <summary>
        /// Create a vistor that will validate an OpenAPIDocument
        /// </summary>
        /// <param name="ruleSet"></param>
        public AsyncApiValidator(ValidationRuleSet ruleSet)
        {
            _ruleSet = ruleSet;
        }

        /// <summary>
        /// Gets the validation errors.
        /// </summary>
        public IEnumerable<AsyncApiValidatorError> Errors
        {
            get
            {
                return _errors;
            }
        }

        /// <summary>
        /// Gets the validation warnings.
        /// </summary>
        public IEnumerable<AsyncApiValidatorWarning> Warnings
        {
            get
            {
                return _warnings;
            }
        }

        /// <summary>
        /// Register an error with the validation context.
        /// </summary>
        /// <param name="error">Error to register.</param>
        public void AddError(AsyncApiValidatorError error)
        {
            if (error == null)
            {
                throw Error.ArgumentNull(nameof(error));
            }

            _errors.Add(error);
        }

        /// <summary>
        /// Register an error with the validation context.
        /// </summary>
        /// <param name="warning">Error to register.</param>
        public void AddWarning(AsyncApiValidatorWarning warning)
        {
            if (warning == null)
            {
                throw Error.ArgumentNull(nameof(warning));
            }

            _warnings.Add(warning);
        }

        /// <summary>
        /// Execute validation rules against an <see cref="AsyncApiDocument"/>
        /// </summary>
        /// <param name="item">The object to be validated</param>
        public override void Visit(AsyncApiDocument item) => Validate(item);

        /// <summary>
        /// Execute validation rules against an <see cref="AsyncApiInfo"/>
        /// </summary>
        /// <param name="item">The object to be validated</param>
        public override void Visit(AsyncApiInfo item) => Validate(item);

        /// <summary>
        /// Execute validation rules against an <see cref="AsyncApiContact"/>
        /// </summary>
        /// <param name="item">The object to be validated</param>
        public override void Visit(AsyncApiContact item) => Validate(item);

        /// <summary>
        /// Execute validation rules against an <see cref="AsyncApiComponents"/>
        /// </summary>
        /// <param name="item">The object to be validated</param>
        public override void Visit(AsyncApiComponents item) => Validate(item);

        /// <summary>
        /// Execute validation rules against an <see cref="AsyncApiHeader"/>
        /// </summary>
        /// <param name="item">The object to be validated</param>
        public override void Visit(AsyncApiHeader item) => Validate(item);

        /// <summary>
        /// Execute validation rules against an <see cref="AsyncApiResponse"/>
        /// </summary>
        /// <param name="item">The object to be validated</param>
        public override void Visit(AsyncApiResponse item) => Validate(item);

        /// <summary>
        /// Execute validation rules against an <see cref="AsyncApiMediaType"/>
        /// </summary>
        /// <param name="item">The object to be validated</param>
        public override void Visit(AsyncApiMediaType item) => Validate(item);

        /// <summary>
        /// Execute validation rules against an <see cref="AsyncApiResponses"/>
        /// </summary>
        /// <param name="item">The object to be validated</param>
        public override void Visit(AsyncApiResponses item) => Validate(item);

        /// <summary>
        /// Execute validation rules against an <see cref="AsyncApiExternalDocs"/>
        /// </summary>
        /// <param name="item">The object to be validated</param>
        public override void Visit(AsyncApiExternalDocs item) => Validate(item);

        /// <summary>
        /// Execute validation rules against an <see cref="AsyncApiLicense"/>
        /// </summary>
        /// <param name="item">The object to be validated</param>
        public override void Visit(AsyncApiLicense item) => Validate(item);

        /// <summary>
        /// Execute validation rules against an <see cref="AsyncApiOauthFlow"/>
        /// </summary>
        /// <param name="item">The object to be validated</param>
        public override void Visit(AsyncApiOauthFlow item) => Validate(item);

        /// <summary>
        /// Execute validation rules against an <see cref="AsyncApiTag"/>
        /// </summary>
        /// <param name="item">The object to be validated</param>
        public override void Visit(AsyncApiTag item) => Validate(item);

        /// <summary>
        /// Execute validation rules against an <see cref="AsyncApiParameter"/>
        /// </summary>
        /// <param name="item">The object to be validated</param>
        public override void Visit(AsyncApiParameter item) => Validate(item);

        /// <summary>
        /// Execute validation rules against an <see cref="AsyncApiSchema"/>
        /// </summary>
        /// <param name="item">The object to be validated</param>
        public override void Visit(AsyncApiSchema item) => Validate(item);

        /// <summary>
        /// Execute validation rules against an <see cref="AsyncApiServer"/>
        /// </summary>
        /// <param name="item">The object to be validated</param>
        public override void Visit(AsyncApiServer item) => Validate(item);

        /// <summary>
        /// Execute validation rules against an <see cref="AsyncApiEncoding"/>
        /// </summary>
        /// <param name="item">The object to be validated</param>
        public override void Visit(AsyncApiEncoding item) => Validate(item);

        /// <summary>
        /// Execute validation rules against an <see cref="AsyncApiCallback"/>
        /// </summary>
        /// <param name="item">The object to be validated</param>
        public override void Visit(AsyncApiCallback item) => Validate(item);

        /// <summary>
        /// Execute validation rules against an <see cref="IAsyncApiExtensible"/>
        /// </summary>
        /// <param name="item">The object to be validated</param>
        public override void Visit(IAsyncApiExtensible item) => Validate(item);

        /// <summary>
        /// Execute validation rules against an <see cref="IAsyncApiExtension"/>
        /// </summary>
        /// <param name="item">The object to be validated</param>
        public override void Visit(IAsyncApiExtension item) => Validate(item, item.GetType());

        /// <summary>
        /// Execute validation rules against a list of <see cref="AsyncApiExample"/>
        /// </summary>
        /// <param name="items">The object to be validated</param>
        public override void Visit(IList<AsyncApiExample> items) => Validate(items, items.GetType());

        private void Validate<T>(T item)
        {
            var type = typeof(T);

            Validate(item, type);
        }

        /// <summary>
        /// This overload allows applying rules based on actual object type, rather than matched interface.  This is 
        /// needed for validating extensions.
        /// </summary>
        private void Validate(object item, Type type)
        {
            if (item == null)
            {
                return;  // Required fields should be checked by higher level objects
            }

            // Validate unresolved references as references
            var potentialReference = item as IAsyncApiReferenceable;
            if (potentialReference != null && potentialReference.UnresolvedReference)
            {
                type = typeof(IAsyncApiReferenceable);
            }

            var rules = _ruleSet.FindRules(type);
            foreach (var rule in rules)
            {
                rule.Evaluate(this as IValidationContext, item);
            }
        }
    }
}
