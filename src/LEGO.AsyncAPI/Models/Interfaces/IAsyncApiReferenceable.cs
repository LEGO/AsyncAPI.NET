// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models.Interfaces
{
    using LEGO.AsyncAPI.Writers;

    public interface IAsyncApiReferenceable : IAsyncApiSerializable
    {
        /// <summary>
        /// Indicates if object is populated with data or is just a reference to the data.
        /// </summary>
        bool UnresolvedReference { get; set; }

        /// <summary>
        /// Reference object.
        /// </summary>
        AsyncApiReference Reference { get; set; }

        /// <summary>
        /// Serialize to AsyncAPI V2 document without using reference.
        /// </summary>
        void SerializeV2WithoutReference(IAsyncApiWriter writer);
    }
}