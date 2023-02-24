// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Validation.Rules
{
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Validations;
    using System.Linq;

    [AsyncApiRule]
    public static class AsyncApiTagRules
    {
        public static ValidationRule<AsyncApiTag> TagRequiredFields =>
            new ValidationRule<AsyncApiTag>(
                (context, tag) =>
                {
                    context.Enter("name");
                    if (tag.Name == null)
                    {
                        context.CreateError(
                            nameof(TagRequiredFields),
                            string.Format(Resource.Validation_FieldRequired, "name", "tag"));
                    }

                    context.Exit();

                });
    }
}