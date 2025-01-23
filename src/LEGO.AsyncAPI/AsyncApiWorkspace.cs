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
        private readonly Dictionary<Uri, IAsyncApiSerializable> asyncApiReferenceableRegistry = new();

        public Uri BaseUrl { get; }

        public AsyncApiWorkspace()
        {
            this.BaseUrl = new Uri("http://asyncapi.net");
        }

        public AsyncApiWorkspace(Uri baseUrl)
        {
            this.BaseUrl = baseUrl;
        }

        public void RegisterComponents(AsyncApiDocument document)
        {
            document.Workspace = this;
            if (document?.Components == null)
            {
                return;
            }

            string baseUri = "#/components/";
            string location;

            // Register Schema
            foreach (var item in document.Components.Schemas)
            {
                location = baseUri + ReferenceType.Schema.GetDisplayName() + "/" + item.Key;
                this.RegisterComponent(location, item.Value);
            }

            // Register Parameters
            foreach (var item in document.Components.Parameters)
            {
                location = baseUri + ReferenceType.Parameter.GetDisplayName() + "/" + item.Key;
                this.RegisterComponent(location, item.Value);
            }

            // Register Channels
            foreach (var item in document.Components.Channels)
            {
                location = baseUri + ReferenceType.Channel.GetDisplayName() + "/" + item.Key;
                this.RegisterComponent(location, item.Value);
            }

            // Register Servers
            foreach (var item in document.Components.Servers)
            {
                location = baseUri + ReferenceType.Server.GetDisplayName() + "/" + item.Key;
                this.RegisterComponent(location, item.Value);
            }

            // Register ServerVariables
            foreach (var item in document.Components.ServerVariables)
            {
                location = baseUri + ReferenceType.ServerVariable.GetDisplayName() + "/" + item.Key;
                this.RegisterComponent(location, item.Value);
            }

            // Register Messages
            foreach (var item in document.Components.Messages)
            {
                location = baseUri + ReferenceType.Message.GetDisplayName() + "/" + item.Key;
                this.RegisterComponent(location, item.Value);
            }

            // Register SecuritySchemes
            foreach (var item in document.Components.SecuritySchemes)
            {
                location = baseUri + ReferenceType.SecurityScheme.GetDisplayName() + "/" + item.Key;
                this.RegisterComponent(location, item.Value);
            }

            // Register Parameters
            foreach (var item in document.Components.Parameters)
            {
                location = baseUri + ReferenceType.Parameter.GetDisplayName() + "/" + item.Key;
                this.RegisterComponent(location, item.Value);
            }

            // Register CorrelationIds
            foreach (var item in document.Components.CorrelationIds)
            {
                location = baseUri + ReferenceType.CorrelationId.GetDisplayName() + "/" + item.Key;
                this.RegisterComponent(location, item.Value);
            }

            // Register OperationTraits
            foreach (var item in document.Components.OperationTraits)
            {
                location = baseUri + ReferenceType.OperationTrait.GetDisplayName() + "/" + item.Key;
                this.RegisterComponent(location, item.Value);
            }

            // Register MessageTraits
            foreach (var item in document.Components.MessageTraits)
            {
                location = baseUri + ReferenceType.MessageTrait.GetDisplayName() + "/" + item.Key;
                this.RegisterComponent(location, item.Value);
            }

            // Register ServerBindings
            foreach (var item in document.Components.ServerBindings)
            {
                location = baseUri + ReferenceType.ServerBindings.GetDisplayName() + "/" + item.Key;
                this.RegisterComponent(location, item.Value);
            }

            // Register ChannelBindings
            foreach (var item in document.Components.ChannelBindings)
            {
                location = baseUri + ReferenceType.ChannelBindings.GetDisplayName() + "/" + item.Key;
                this.RegisterComponent(location, item.Value);
            }

            // Register OperationBindings
            foreach (var item in document.Components.OperationBindings)
            {
                location = baseUri + ReferenceType.OperationBindings.GetDisplayName() + "/" + item.Key;
                this.RegisterComponent(location, item.Value);
            }

            // Register MessageBindings
            foreach (var item in document.Components.MessageBindings)
            {
                location = baseUri + ReferenceType.MessageBindings.GetDisplayName() + "/" + item.Key;
                this.RegisterComponent(location, item.Value);
            }
        }

        private void RegisterInternalComponent<T>(string location, IAsyncApiReferenceable component)
        {
            var uri = this.ToLocationUrl(location);
            if (!this.asyncApiReferenceableRegistry.ContainsKey(uri))
            {
                this.asyncApiReferenceableRegistry[uri] = component;
            }
        }

        public bool RegisterComponent<T>(string location, T component)
        {
            var uri = this.ToLocationUrl(location);
            if (component is IAsyncApiSerializable referenceable)
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
