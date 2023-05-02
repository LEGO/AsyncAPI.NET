// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Validation.Rules
{
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Validations;

    [AsyncApiRule]
    public static class AsyncApiExtensionRules
    {
        /// <summary>
        /// Extension name MUST start with "x-".
        /// </summary>
        public static ValidationRule<IAsyncApiExtensible> ExtensionNameMustStartWithXDash =>
            new ValidationRule<IAsyncApiExtensible>(
                (context, item) =>
                {
                    context.Enter("extensions");
                    foreach (var extensible in item.Extensions)
                    {
                        if (!extensible.Key.StartsWith("x-"))
                        {
                            context.CreateError(
                                nameof(ExtensionNameMustStartWithXDash),
                                string.Format(Resource.Validation_ExtensionNameMustBeginWithXDash, extensible.Key, context.PathString));
                        }
                    }
                    context.Exit();
                });
    }
}
