// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Validations
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Services;

    /// <summary>
    /// Class containing dispatchers to execute validation rules on for AsyncApi document.
    /// </summary>
    public class AsyncApiValidator : AsyncApiVisitorBase, IValidationContext
    {
        private readonly ValidationRuleSet ruleSet;
        private readonly IList<AsyncApiValidatorError> errors = new List<AsyncApiValidatorError>();
        private readonly IList<AsyncApiValidatorWarning> warnings = new List<AsyncApiValidatorWarning>();

        /// <summary>
        /// Create a vistor that will validate an AsyncApiDocument.
        /// </summary>
        /// <param name="ruleSet"></param>
        public AsyncApiValidator(ValidationRuleSet ruleSet)
        {
            this.ruleSet = ruleSet;
        }

        /// <summary>
        /// Gets the validation errors.
        /// </summary>
        public IEnumerable<AsyncApiValidatorError> Errors
        {
            get
            {
                return this.errors;
            }
        }

        /// <summary>
        /// Gets the validation warnings.
        /// </summary>
        public IEnumerable<AsyncApiValidatorWarning> Warnings
        {
            get
            {
                return this.warnings;
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

            this.errors.Add(error);
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

            this.warnings.Add(warning);
        }

        /// <summary>
        /// Execute validation rules against an <see cref="AsyncApiDocument"/>.
        /// </summary>
        /// <param name="item">The object to be validated.</param>
        public override void Visit(AsyncApiDocument item) => this.Validate(item);

        /// <summary>
        /// Execute validation rules against an <see cref="AsyncApiInfo"/>.
        /// </summary>
        /// <param name="item">The object to be validated.</param>
        public override void Visit(AsyncApiInfo item) => this.Validate(item);

        /// <summary>
        /// Execute validation rules against an <see cref="AsyncApiContact"/>.
        /// </summary>
        /// <param name="item">The object to be validated.</param>
        public override void Visit(AsyncApiContact item) => this.Validate(item);

        /// <summary>
        /// Execute validation rules against an <see cref="AsyncApiComponents"/>.
        /// </summary>
        /// <param name="item">The object to be validated.</param>
        public override void Visit(AsyncApiComponents item) => this.Validate(item);

        /// <summary>
        /// Execute validation rules against an <see cref="AsyncApiLicense"/>.
        /// </summary>
        /// <param name="item">The object to be validated.</param>
        public override void Visit(AsyncApiLicense item) => this.Validate(item);

        /// <summary>
        /// Execute validation rules against an <see cref="AsyncApiOAuthFlow"/>.
        /// </summary>
        /// <param name="item">The object to be validated.</param>
        public override void Visit(AsyncApiOAuthFlow item) => this.Validate(item);

        /// <summary>
        /// Execute validation rules against an <see cref="AsyncApiTag"/>.
        /// </summary>
        /// <param name="item">The object to be validated.</param>
        public override void Visit(AsyncApiTag item) => this.Validate(item);

        /// <summary>
        /// Execute validation rules against an <see cref="AsyncApiParameter"/>.
        /// </summary>
        /// <param name="item">The object to be validated.</param>
        public override void Visit(AsyncApiParameter item) => this.Validate(item);

        /// <summary>
        /// Execute validation rules against an <see cref="AsyncApiSchema"/>.
        /// </summary>
        /// <param name="item">The object to be validated.</param>
        public override void Visit(AsyncApiSchema item) => this.Validate(item);

        /// <summary>
        /// Execute validation rules against an <see cref="AsyncApiServer"/>.
        /// </summary>
        /// <param name="item">The object to be validated.</param>
        public override void Visit(AsyncApiServer item) => this.Validate(item);

        /// <summary>
        /// Execute validation rules against an <see cref="IAsyncApiExtensible"/>.
        /// </summary>
        /// <param name="item">The object to be validated.</param>
        public override void Visit(IAsyncApiExtensible item) => this.Validate(item);

        /// <summary>
        /// Execute validation rules against an <see cref="IAsyncApiExtension"/>.
        /// </summary>
        /// <param name="item">The object to be validated.</param>
        public override void Visit(IAsyncApiExtension item) => this.Validate(item, item.GetType());

        /// <summary>
        /// Execute validation rules against a list of <see cref="AsyncApiExample"/>.
        /// </summary>
        /// <param name="items">The object to be validated.</param>
        public override void Visit(IList<AsyncApiMessageExample> items) => this.Validate(items, items.GetType());

        private void Validate<T>(T item)
        {
            var type = typeof(T);

            this.Validate(item, type);
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

            var rules = this.ruleSet.FindRules(type);
            foreach (var rule in rules)
            {
                rule.Evaluate(this as IValidationContext, item);
            }
        }
    }
}
