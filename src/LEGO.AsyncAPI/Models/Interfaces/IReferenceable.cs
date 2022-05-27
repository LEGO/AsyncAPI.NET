// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Interfaces
{
    /// <summary>
    /// Defines that an AsyncAPI Element can be referenced.
    /// </summary>
    public interface IReferenceable
    {
        /// <summary>
        /// Gets or sets indicates if object is populated with data or is just a reference to the data.
        /// </summary>
        bool? UnresolvedReference { get; set; }

        /// <summary>
        /// Gets or sets reference object.
        /// </summary>
        AsyncApiReference Reference { get; set; }
    }
}
