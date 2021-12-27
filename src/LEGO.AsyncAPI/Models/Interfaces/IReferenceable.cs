// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    /// <summary>
    /// Defines that an AsyncAPI Element can be referenced.
    /// </summary>
    public interface IReferenceable
    {
        /// <summary>
        /// Indicates if object is populated with data or is just a reference to the data.
        /// </summary>
        bool UnresolvedReference { get; set; }

        /// <summary>
        /// Reference object.
        /// </summary>
        Reference Reference { get; set; }
    }
}
