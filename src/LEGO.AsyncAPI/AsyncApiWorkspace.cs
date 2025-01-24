// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json.Nodes;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;

    public class AsyncApiWorkspace
    {
        private readonly Dictionary<string, IAsyncApiReferenceable> referenceRegistry = new();
        private readonly Dictionary<Uri, Stream> artifactsRegistry = new();
        private readonly Dictionary<Uri, IAsyncApiSerializable> resolvedReferenceRegistry = new();

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

        public bool RegisterComponent<T>(string location, T component)
        {
            var uri = this.ToLocationUrl(location);
            if (component is IAsyncApiSerializable referenceable)
            {
                if (!this.resolvedReferenceRegistry.ContainsKey(uri))
                {
                    this.resolvedReferenceRegistry[uri] = referenceable;
                    return true;
                }
            }

            if (component is Stream stream)
            {
                if (!this.artifactsRegistry.ContainsKey(uri))
                {
                    this.artifactsRegistry[uri] = stream;
                }
                return true;
            }

            return false;
        }

        public void RegisterReference(IAsyncApiReferenceable reference)
        {
            var location = reference.Reference.Reference;
            this.referenceRegistry[location] = reference;
        }

        public bool Contains(string location)
        {
            var key = this.ToLocationUrl(location);
            return this.resolvedReferenceRegistry.ContainsKey(key) || this.artifactsRegistry.ContainsKey(key);
        }

        public T ResolveReference<T>(string location)
        {
            if (string.IsNullOrEmpty(location))
            {
                return default;
            }

            var uri = this.ToLocationUrl(location);
            if (this.resolvedReferenceRegistry.TryGetValue(uri, out var referenceableValue))
            {
                return (T)referenceableValue;
            }

            if (this.artifactsRegistry.TryGetValue(uri, out var stream))
            {
                return (T)(object)stream;
            }

            return default;
        }

        private Uri ToLocationUrl(string location)
        {
            return new(location, UriKind.RelativeOrAbsolute);
        }
    }
}
