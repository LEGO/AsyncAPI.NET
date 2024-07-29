// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;

    public class AsyncApiWorkspace
    {
        private readonly Dictionary<string, Uri> documentsIdRegistry = new();
        private readonly Dictionary<Uri, Stream> artifactsRegistry = new();
        private readonly Dictionary<Uri, IAsyncApiReferenceable> asyncApiReferenceableRegistry = new();

        public Uri BaseUrl { get; }

        public AsyncApiWorkspace()
        {
            this.BaseUrl = new Uri(AsyncApiConstants.BaseRegistryUri);
        }

        public bool RegisterComponent<T>(string location, T component)
        {
            var uri = this.ToLocationUrl(location);
            if (component is IAsyncApiReferenceable referenceable)
            {
                if (!this.asyncApiReferenceableRegistry.ContainsKey(uri))
                {
                    this.asyncApiReferenceableRegistry[uri] = referenceable;
                    return true;
                }
            }
            else if (component is Stream stream)
            {
                if (!this.artifactsRegistry.ContainsKey(uri))
                {
                    this.artifactsRegistry[uri] = stream;
                    return true;
                }
            }

            return false;
        }

        public void AddDocumentId(string key, Uri value)
        {
            if (!this.documentsIdRegistry.ContainsKey(key))
            {
                this.documentsIdRegistry[key] = value;
            }
        }

        public Uri GetDocumentId(string key)
        {
            if (this.documentsIdRegistry.TryGetValue(key, out var id))
            {
                return id;
            }
            return null;
        }

        public T ResolveReference<T>(string location)
        {
            if (string.IsNullOrEmpty(location))
            {
                return default;
            }

            var uri = this.ToLocationUrl(location);
            if (this.asyncApiReferenceableRegistry.TryGetValue(uri, out var referenceableValue))
            {
                return (T)referenceableValue;
            }

            return default;
        }

        private Uri ToLocationUrl(string location)
        {
            return new(this.BaseUrl, location);
        }
    }
}
