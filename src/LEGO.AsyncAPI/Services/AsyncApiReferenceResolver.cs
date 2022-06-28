namespace LEGO.AsyncAPI.Services
{

using System;
using System.Collections.Generic;
using System.Linq;
using Exceptions;
using Models;
using Models.Interfaces;

/// <summary>
    /// This class is used to walk an AsyncApiDocument and convert unresolved references to references to populated objects
    /// </summary>
    internal class AsyncApiReferenceResolver : AsyncApiVisitorBase
    {
        private AsyncApiDocument currentDocument;
        private List<AsyncApiError> errors = new List<AsyncApiError>();

        public AsyncApiReferenceResolver(AsyncApiDocument currentDocument)
        {
            this.currentDocument = currentDocument;
        }

        public IEnumerable<AsyncApiError> Errors
        {
            get
            {
                return errors;
            }
        }

        public override void Visit(IAsyncApiReferenceable referenceable)
        {
            if (referenceable.Reference != null)
            {
                referenceable.Reference.HostDocument = currentDocument;
            }
        }
        public override void Visit(AsyncApiComponents components)
        {
            ResolveMap(components.Parameters);
            ResolveMap(components.Channels); // TODO override
            ResolveMap(components.Schemas);
            ResolveMap(components.Servers); // TODO override
            ResolveMap(components.CorrelationIds); // TODO override
            ResolveMap(components.MessageTraits); // TODO override
            ResolveMap(components.OperationTraits); // TODO override
            ResolveMap(components.SecuritySchemes);
            // TODO: figure out bindings
            // ResolveMap(components.ChannelBindings);
            // ResolveMap(components.MessageBindings);
            // ResolveMap(components.OperationBindings);
            // ResolveMap(components.ServerBindings);
            ResolveMap(components.Messages);
            
        }

        public override void Visit(AsyncApiChannel channel)
        {
            ResolveMap(channel.Parameters);
            // TODO: figure out bindings
            //ResolveMap(channel.Bindings);
        }
        

        /// <summary>
        /// Resolve all references used in an operation
        /// </summary>
        public override void Visit(AsyncApiOperation operation)
        {
            ResolveObject(operation.Message, r => operation.Message = r);
            ResolveList(operation.Traits);
            // TODO: Figure out bindings
            // ResolveMap(operation.Bindings);
        }

        /// <summary>
        /// Resolve all references to SecuritySchemes
        /// </summary>
        public override void Visit(AsyncApiSecurityRequirement securityRequirement)
        {
            foreach (var scheme in securityRequirement.Keys.ToList())
            {
                ResolveObject(scheme, (resolvedScheme) =>
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
        /// Resolve all references to parameters
        /// </summary>
        public override void Visit(IList<AsyncApiParameter> parameters)
        {
            ResolveList(parameters);
        }

        /// <summary>
        /// Resolve all references used in a parameter
        /// </summary>
        public override void Visit(AsyncApiParameter parameter)
        {
            ResolveObject(parameter.Schema, r => parameter.Schema = r);
        }

        /// <summary>
        /// Resolve all references used in a schema
        /// </summary>
        public override void Visit(AsyncApiSchema schema)
        {
            ResolveObject(schema.Items, r => schema.Items = r);
            ResolveList(schema.OneOf);
            ResolveList(schema.AllOf);
            ResolveList(schema.AnyOf);
            ResolveObject(schema.Contains, r => schema.Contains = r);
            ResolveObject(schema.Else, r => schema.Else = r);
            ResolveObject(schema.If, r => schema.If = r);
            ResolveObject(schema.Items, r => schema.Items = r);
            ResolveObject(schema.Not, r => schema.Not=r);
            ResolveObject(schema.Then, r => schema.Then = r);
            ResolveObject(schema.PropertyNames, r => schema.PropertyNames = r);
            ResolveObject(schema.AdditionalProperties, r => schema.AdditionalProperties = r);
            ResolveMap(schema.Properties);
        }

        private void ResolveObject<T>(T entity, Action<T> assign) where T : class, IAsyncApiReferenceable, new()
        {
            if (entity == null) return;

            if (IsUnresolvedReference(entity))
            {
                assign(ResolveReference<T>(entity.Reference));
            }
        }

        private void ResolveList<T>(IList<T> list) where T : class, IAsyncApiReferenceable, new()
        {
            if (list == null) return;

            for (int i = 0; i < list.Count; i++)
            {
                var entity = list[i];
                if (IsUnresolvedReference(entity))
                {
                    list[i] = ResolveReference<T>(entity.Reference);
                }
            }
        }

        private void ResolveMap<T>(IDictionary<string, T> map) where T : class, IAsyncApiReferenceable, new()
        {
            if (map == null) return;

            foreach (var key in map.Keys.ToList())
            {
                var entity = map[key];
                if (IsUnresolvedReference(entity))
                {
                    map[key] = ResolveReference<T>(entity.Reference);
                }
            }
        }

        private T ResolveReference<T>(AsyncApiReference reference) where T : class, IAsyncApiReferenceable, new()
        {
            try
                {
                    return currentDocument.ResolveReference(reference) as T;
                }
                catch (AsyncApiException ex)
                {
                    errors.Add(new AsyncApiReferenceError(ex));
                    return null;
                }
            
            // The concept of merging references with their target at load time is going away in the next major version
            // External references will not support this approach.
            //else if (_resolveRemoteReferences == true)
            //{
            //    if (_currentDocument.Workspace == null)
            //    {
            //        _errors.Add(new AsyncApiReferenceError(reference,"Cannot resolve external references for documents not in workspaces."));
            //        // Leave as unresolved reference
            //        return new T()
            //        {
            //            UnresolvedReference = true,
            //            Reference = reference
            //        };
            //    }
            //    var target = _currentDocument.Workspace.ResolveReference(reference);

            //    // TODO:  If it is a document fragment, then we should resolve it within the current context

            //    return target as T;
            //}
        }

        private bool IsUnresolvedReference(IAsyncApiReferenceable possibleReference)
        {
            return (possibleReference != null && possibleReference.UnresolvedReference);
        }
    }
}

}