﻿// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Validations
{
    using System;
    using LEGO.AsyncAPI.Models.Interfaces;

    /// <summary>
    /// Class containing validation rule logic for <see cref="IAsyncApiElement"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ValidationRule<T> : ValidationRule
        where T : IAsyncApiElement
    {
        private readonly Action<IValidationContext, T> validate;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationRule"/> class.
        /// </summary>
        /// <param name="validate">Action to perform the validation.</param>
        public ValidationRule(Action<IValidationContext, T> validate)
        {
            this.validate = validate ?? throw Error.ArgumentNull(nameof(validate));
        }

        internal override Type ElementType
        {
            get { return typeof(T); }
        }

        internal override void Evaluate(IValidationContext context, object item)
        {
            if (context == null)
            {
                throw Error.ArgumentNull(nameof(context));
            }

            if (item == null)
            {
                return;
            }

            if (!(item is T))
            {
                throw Error.Argument(string.Format("Input type must be of type {0}", typeof(T).FullName));
            }

            T typedItem = (T)item;
            this.validate(context, typedItem);
        }
    }
}
