// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Validations
{
    using System;

    /// <summary>
    /// Class containing validation rule logic.
    /// </summary>
    public abstract class ValidationRule
    {
        /// <summary>
        /// Element Type.
        /// </summary>
        internal abstract Type ElementType { get; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="item">The object item.</param>
        internal abstract void Evaluate(IValidationContext context, object item);
    }
}
