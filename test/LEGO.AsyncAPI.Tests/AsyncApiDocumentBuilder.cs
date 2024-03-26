// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests
{
    using Json.Schema;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using System;

    internal class AsyncApiDocumentBuilder
    {
        private AsyncApiDocument document;

        public AsyncApiDocumentBuilder()
        {
            this.document = new AsyncApiDocument();
        }

        public AsyncApiDocumentBuilder WithInfo(AsyncApiInfo info)
        {
            this.document.Info = info;
            return this;
        }

        public AsyncApiDocumentBuilder WithServer(string key, AsyncApiServer server)
        {
            this.document.Servers.Add(key, server);
            return this;
        }

        public AsyncApiDocumentBuilder WithServer(string key, Func<AsyncApiServer, AsyncApiServer> server)
        {
            var _server = new AsyncApiServer();
            this.document.Servers.Add(key, server.Invoke(_server));
            return this;
        }

        public AsyncApiDocumentBuilder WithDefaultContentType(string contentType = "application/json")
        {
            this.document.DefaultContentType = contentType;
            return this;
        }

        public AsyncApiDocumentBuilder WithChannel(string key, AsyncApiChannel channel)
        {
            this.document.Channels.Add(key, channel);
            return this;
        }

        public AsyncApiDocumentBuilder WithComponent(string key, JsonSchema schema)
        {
            if (this.document.Components == null)
            {
                this.document.Components = new AsyncApiComponents();
            }

            this.document.Components.Schemas.Add(key, schema);
            return this;
        }

        public AsyncApiDocumentBuilder WithComponent(string key, AsyncApiServer server)
        {
            if (this.document.Components == null)
            {
                this.document.Components = new AsyncApiComponents();
            }

            this.document.Components.Servers.Add(key, server);
            return this;
        }

        public AsyncApiDocumentBuilder WithComponent(string key, AsyncApiServerVariable serverVariable)
        {
            if (this.document.Components == null)
            {
                this.document.Components = new AsyncApiComponents();
            }

            this.document.Components.ServerVariables.Add(key, serverVariable);
            return this;
        }

        public AsyncApiDocumentBuilder WithComponent(string key, AsyncApiChannel channel)
        {
            if (this.document.Components == null)
            {
                this.document.Components = new AsyncApiComponents();
            }

            this.document.Components.Channels.Add(key, channel);
            return this;
        }

        public AsyncApiDocumentBuilder WithComponent(string key, AsyncApiMessage message)
        {
            if (this.document.Components == null)
            {
                this.document.Components = new AsyncApiComponents();
            }

            this.document.Components.Messages.Add(key, message);
            return this;
        }

        public AsyncApiDocumentBuilder WithComponent(string key, AsyncApiSecurityScheme securityScheme)
        {
            if (this.document.Components == null)
            {
                this.document.Components = new AsyncApiComponents();
            }

            this.document.Components.SecuritySchemes.Add(key, securityScheme);
            return this;
        }

        public AsyncApiDocumentBuilder WithComponent(string key, AsyncApiParameter parameter)
        {
            if (this.document.Components == null)
            {
                this.document.Components = new AsyncApiComponents();
            }

            this.document.Components.Parameters.Add(key, parameter);
            return this;
        }

        public AsyncApiDocumentBuilder WithComponent(string key, AsyncApiCorrelationId correlationId)
        {
            if (this.document.Components == null)
            {
                this.document.Components = new AsyncApiComponents();
            }

            this.document.Components.CorrelationIds.Add(key, correlationId);
            return this;
        }

        public AsyncApiDocumentBuilder WithComponent(string key, AsyncApiOperationTrait operationTrait)
        {
            if (this.document.Components == null)
            {
                this.document.Components = new AsyncApiComponents();
            }

            this.document.Components.OperationTraits.Add(key, operationTrait);
            return this;
        }

        public AsyncApiDocumentBuilder WithComponent(string key, AsyncApiMessageTrait messageTrait)
        {
            if (this.document.Components == null)
            {
                this.document.Components = new AsyncApiComponents();
            }

            this.document.Components.MessageTraits.Add(key, messageTrait);
            return this;
        }

        public AsyncApiDocumentBuilder WithComponent(string key, AsyncApiBindings<IServerBinding> serverBindings)
        {
            if (this.document.Components == null)
            {
                this.document.Components = new AsyncApiComponents();
            }

            this.document.Components.ServerBindings.Add(key, serverBindings);
            return this;
        }

        public AsyncApiDocumentBuilder WithComponent(string key, AsyncApiBindings<IChannelBinding> channelBindings)
        {
            if (this.document.Components == null)
            {
                this.document.Components = new AsyncApiComponents();
            }

            this.document.Components.ChannelBindings.Add(key, channelBindings);
            return this;
        }

        public AsyncApiDocumentBuilder WithComponent(string key, AsyncApiBindings<IOperationBinding> operationBindings)
        {
            if (this.document.Components == null)
            {
                this.document.Components = new AsyncApiComponents();
            }

            this.document.Components.OperationBindings.Add(key, operationBindings);
            return this;
        }

        public AsyncApiDocumentBuilder WithComponent(string key, AsyncApiBindings<IMessageBinding> messageBindings)
        {
            if (this.document.Components == null)
            {
                this.document.Components = new AsyncApiComponents();
            }

            this.document.Components.MessageBindings.Add(key, messageBindings);
            return this;
        }

        public AsyncApiDocumentBuilder WithTags(AsyncApiTag tag)
        {
            this.document.Tags.Add(tag);
            return this;
        }

        public AsyncApiDocumentBuilder WithExternalDocs(AsyncApiExternalDocumentation externalDocumentation)
        {
            this.document.ExternalDocs = externalDocumentation;
            return this;
        }

        public AsyncApiDocument Build()
        {
            return this.document;
        }
    }
}
