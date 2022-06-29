// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models.Interfaces;

    /// <summary>
    /// Comparer for AsyncApiSecurityScheme that only considers the Id in the Reference
    /// (i.e. the string that will actually be displayed in the written document).
    /// </summary>
    public class AsyncApiReferenceEqualityComparer : IEqualityComparer<IAsyncApiReferenceable>
    {
        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        public bool Equals(IAsyncApiReferenceable x, IAsyncApiReferenceable y)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            if (x.Reference == null || y.Reference == null)
            {
                return false;
            }

            return x.Reference.Id == y.Reference.Id;
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        public int GetHashCode(IAsyncApiReferenceable obj)
        {
            return obj?.Reference?.Id == null ? 0 : obj.Reference.Id.GetHashCode();
        }
    }
}