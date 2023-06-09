// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Collections.Generic;

    public static class SchemaTypeHelpers
    {
        public static IEnumerable<SchemaType> GetFlags(SchemaType input)
        {
            foreach (SchemaType value in System.Enum.GetValues(input.GetType()))
            {
                if (input.HasFlag(value))
                {
                    yield return value;
                }
            }
        }
    }
}
