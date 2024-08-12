// Copyright (c) The LEGO Group. All rights reserved.
namespace LEGO.AsyncAPI.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LEGO.AsyncAPI.Exceptions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;

    /// <summary>
    /// This class is used to walk an AsyncApiDocument and convert unresolved references to references to populated objects.
    /// </summary>
    internal class AsyncApiReferenceResolver : AsyncApiVisitorBase
    {
        private AsyncApiDocument currentDocument;
        private List<AsyncApiError> errors = new List<AsyncApiError>();

        public AsyncApiReferenceResolver(
            AsyncApiDocument currentDocument)
        {
            this.currentDocument = currentDocument;
        }

        public IEnumerable<AsyncApiError> Errors
        {
            get
            {
                return this.errors;
            }
        }

        public override void Visit(IAsyncApiReferenceable referenceable)
        {
            if (referenceable.Reference != null)
            {
                referenceable.Reference.HostDocument = this.currentDocument;
            }
        }

        public override void Visit(AsyncApiComponents components)
        {
            this.ResolveMap(components.Parameters);
            this.ResolveMap(components.Channels);
            this.ResolveMap(components.Schemas);
            this.ResolveMap(components.Servers);
            this.ResolveMap(components.CorrelationIds);
            this.ResolveMap(components.MessageTraits);
            this.ResolveMap(components.OperationTraits);
            this.ResolveMap(components.SecuritySchemes);
            this.ResolveMap(components.ChannelBindings);
            this.ResolveMap(components.MessageBindings);
            this.ResolveMap(components.OperationBindings);
            this.ResolveMap(components.ServerBindings);
            this.ResolveMap(components.Messages);
        }

        public override void Visit(AsyncApiDocument doc)
        {
            this.ResolveMap(doc.Servers);
            this.ResolveMap(doc.Channels);
        }

        public override void Visit(AsyncApiChannel channel)
        {
            this.ResolveMap(channel.Parameters);
            this.ResolveObject(channel.Bindings, r => channel.Bindings = r);
        }

        public override void Visit(AsyncApiMessageTrait trait)
        {
            this.ResolveObject(trait.CorrelationId, r => trait.CorrelationId = r);
            this.ResolveObject(trait.Headers, r => trait.Headers = r);
        }

        /// <summary>
        /// Resolve all references used in an operation.
        /// </summary>
        public override void Visit(AsyncApiOperation operation)
        {
            this.ResolveList(operation.Message);
            this.ResolveList(operation.Traits);
            this.ResolveObject(operation.Bindings, r => operation.Bindings = r);
        }

        public override void Visit(AsyncApiMessage message)
        {
            this.ResolveObject(message.Headers, r => message.Headers = r);

            // #ToFix Resolve references correctly
            if (message.Payload is AsyncApiJsonSchemaPayload)
            {
                this.ResolveObject<AsyncApiSchema>(message.Payload as AsyncApiJsonSchemaPayload, r => message.Payload = new AsyncApiJsonSchemaPayload(r));
            }

            this.ResolveList(message.Traits);
            this.ResolveObject(message.CorrelationId, r => message.CorrelationId = r);
            this.ResolveObject(message.Bindings, r => message.Bindings = r);
        }

        public override void Visit(AsyncApiServer server)
        {
            this.ResolveObject(server.Bindings, r => server.Bindings = r);
        }

        /// <summary>
        /// Resolve all references to SecuritySchemes.
        /// </summary>
        public override void Visit(AsyncApiSecurityRequirement securityRequirement)
        {
            foreach (var scheme in securityRequirement.Keys.ToList())
            {
                this.ResolveObject(scheme, (resolvedScheme) =>
                {
                    if (resolvedScheme != null)
                    {
                        // If scheme was unresolved
                        // copy Scopes and remove old unresolved scheme
                        var scopes = securityRequirement[scheme];
                        securityRequirement.Remove(scheme);
                        securityRequirement.Add(resolvedScheme, scopes);
                    }
                });
            }
        }

        /// <summary>
        /// Resolve all references to parameters.
        /// </summary>
        public override void Visit(IList<AsyncApiParameter> parameters)
        {
            this.ResolveList(parameters);
        }

        /// <summary>
        /// Resolve all references used in a parameter.
        /// </summary>
        public override void Visit(AsyncApiParameter parameter)
        {
            this.ResolveObject(parameter.Schema, r => parameter.Schema = r);
        }

        /// <summary>
        /// Resolve all references used in a schema.
        /// </summary>
        public override void Visit(AsyncApiSchema schema)
        {
            this.ResolveObject(schema.Items, r => schema.Items = r);
            this.ResolveList(schema.OneOf);
            this.ResolveList(schema.AllOf);
            this.ResolveList(schema.AnyOf);
            this.ResolveObject(schema.Contains, r => schema.Contains = r);
            this.ResolveObject(schema.Else, r => schema.Else = r);
            this.ResolveObject(schema.If, r => schema.If = r);
            this.ResolveObject(schema.Items, r => schema.Items = r);
            this.ResolveObject(schema.Not, r => schema.Not = r);
            this.ResolveObject(schema.Then, r => schema.Then = r);
            this.ResolveObject(schema.PropertyNames, r => schema.PropertyNames = r);
            this.ResolveObject(schema.AdditionalProperties, r => schema.AdditionalProperties = r);
            this.ResolveMap(schema.Properties);
        }

        private void ResolveObject<T>(T entity, Action<T> assign)
            where T : class, IAsyncApiReferenceable, new()
        {
            if (entity == null)
            {
                return;
            }

            if (this.IsUnresolvedReference(entity))
            {
                assign(this.ResolveReference<T>(entity.Reference));
            }
        }

        private void ResolveList<T>(IList<T> list)
            where T : class, IAsyncApiReferenceable, new()
        {
            if (list == null)
            {
                return;
            }

            for (int i = 0; i < list.Count; i++)
            {
                var entity = list[i];
                if (this.IsUnresolvedReference(entity))
                {
                    list[i] = this.ResolveReference<T>(entity.Reference);
                }
            }
        }

        private void ResolveMap<T>(IDictionary<string, T> map)
            where T : class, IAsyncApiReferenceable, new()
        {
            if (map == null)
            {
                return;
            }

            foreach (var key in map.Keys.ToList())
            {
                var entity = map[key];
                if (this.IsUnresolvedReference(entity))
                {
                    map[key] = this.ResolveReference<T>(entity.Reference);
                }
            }
        }

        private T ResolveReference<T>(AsyncApiReference reference)
            where T : class, IAsyncApiReferenceable, new()
        {
            // external references are resolved by the AsyncApiExternalReferenceResolver
            if (reference.IsExternal)
            {
                return new()
                {
                    UnresolvedReference = true,
                    Reference = reference,
                };
            }

            try
            {
                var resolvedReference = this.currentDocument.ResolveReference(reference) as T;
                if (resolvedReference == null)
                {
                    throw new AsyncApiException($"Cannot resolve reference '{reference.Reference}' to '{typeof(T).Name}'.");
                }

                return resolvedReference;
            }
            catch (AsyncApiException ex)
            {
                this.errors.Add(new AsyncApiReferenceError(ex));
                return null;
            }
        }

        private bool IsUnresolvedReference(IAsyncApiReferenceable possibleReference)
        {
            return (possibleReference != null && possibleReference.UnresolvedReference);
        }
    }
}