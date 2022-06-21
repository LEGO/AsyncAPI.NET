using System;
using System.Collections.Generic;
using System.Linq;
using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Models.Interfaces;

namespace LEGO.AsyncAPI.Services
{

    public class AsyncApiWalker
    {
        private readonly AsyncApiVisitorBase visitor;
        private readonly Stack<AsyncApiSchema> schemaLoop = new Stack<AsyncApiSchema>();

        public AsyncApiWalker(AsyncApiVisitorBase visitor)
        {
            this.visitor = visitor;
        }

        public void Walk(AsyncApiDocument doc)
        {
            if (doc == null)
            {
                return;
            }

            this.schemaLoop.Clear();

            this.visitor.Visit(doc);

            Walk(AsyncApiConstants.Info, () => Walk(doc.Info));
            Walk(AsyncApiConstants.Servers, () => Walk(doc.Servers));
            Walk(AsyncApiConstants.Channels, () => Walk(doc.Channels));
            Walk(AsyncApiConstants.Components, () => Walk(doc.Components));
            Walk(AsyncApiConstants.Tags, () => Walk(doc.Tags));
            Walk(AsyncApiConstants.ExternalDocs, () => Walk(doc.ExternalDocs));
            Walk(doc as IAsyncApiExtensible);
        }

        internal void Walk(AsyncApiInfo info)
        {
            if (info == null)
            {
                return;
            }

            visitor.Visit(info);

            if (info != null)
            {
                Walk(AsyncApiConstants.Contact, () => Walk(info.Contact));
                Walk(AsyncApiConstants.License, () => Walk(info.License));
            }

            Walk(info as IAsyncApiExtensible);
        }

        internal void Walk(AsyncApiServer server)
        {
            if (server == null)
            {
                return;
            }

            visitor.Visit(server);
            Walk(AsyncApiConstants.Variables, () => Walk(server.Variables));
            Walk(AsyncApiConstants.Security, () => Walk(server.Security));
            // TODO: Figure out bindings
            visitor.Visit(server as IAsyncApiExtensible);
        }

        internal void Walk(IList<AsyncApiSecurityRequirement> securityRequirements)
        {
            if (securityRequirements == null)
            {
                return;
            }

            visitor.Visit(securityRequirements);

            // Visit Examples
            if (securityRequirements != null)
            {
                for (int i = 0; i < securityRequirements.Count; i++)
                {
                    Walk(i.ToString(), () => Walk(securityRequirements[i]));
                }
            }
        }

        internal void Walk(AsyncApiSecurityRequirement securityRequirement)
        {
            if (securityRequirement is null)
            {
                return;
            }

            visitor.Visit(securityRequirement);
            Walk(securityRequirement as IAsyncApiExtensible);
        }

        internal void Walk(IDictionary<string, AsyncApiServer> servers)
        {
            if (servers == null)
            {
                return;
            }

            visitor.Visit(servers);

            if (servers != null)
            {
                foreach (var variable in servers)
                {
                    visitor.CurrentKeys.Server = variable.Key;
                    Walk(variable.Key, () => Walk(variable.Value));
                    visitor.CurrentKeys.Server = null;
                }
            }
        }

        internal void Walk(IDictionary<string, AsyncApiServerVariable> serverVariables)
        {
            if (serverVariables == null)
            {
                return;
            }

            visitor.Visit(serverVariables);

            if (serverVariables != null)
            {
                foreach (var variable in serverVariables)
                {
                    visitor.CurrentKeys.ServerVariable = variable.Key;
                    Walk(variable.Key, () => Walk(variable.Value));
                    visitor.CurrentKeys.ServerVariable = null;
                }
            }
        }

        internal void Walk(AsyncApiLicense license)
        {
            if (license == null)
            {
                return;
            }

            visitor.Visit(license);
        }

        internal void Walk(AsyncApiContact contact)
        {
            if (contact == null)
            {
                return;
            }

            visitor.Visit(contact);
        }

        private void Walk(string context, Action walk)
        {
            visitor.Enter(context.Replace("/", "~1"));
            walk();
            visitor.Exit();
        }

        internal void Walk(IAsyncApiExtensible asyncApiExtensible)
        {
            if (asyncApiExtensible == null)
            {
                return;
            }

            visitor.Visit(asyncApiExtensible);

            if (asyncApiExtensible != null)
            {
                foreach (var item in asyncApiExtensible.Extensions)
                {
                    visitor.CurrentKeys.Extension = item.Key;
                    Walk(item.Key, () => Walk(item.Value));
                    visitor.CurrentKeys.Extension = null;
                }
            }
        }

        private bool ProcessAsReference(IAsyncApiReferenceable referenceable, bool isComponent = false)
        {
            var isReference = referenceable.Reference != null && !isComponent;
            if (isReference)
            {
                Walk(referenceable);
            }

            return isReference;
        }

        internal void Walk(IAsyncApiReferenceable referenceable)
        {
            visitor.Visit(referenceable);
        }
    }

    public class CurrentKeys
    {

    }
    /// <summary>
    /// Open API visitor base provides common logic for concrete visitors
    /// </summary>
    public abstract class AsyncApiVisitorBase
    {
        private readonly Stack<string> path = new Stack<string>();

        /// <summary>
        /// Properties available to identify context of where an object is within OpenAPI Document
        /// </summary>
        public CurrentKeys CurrentKeys { get; } = new CurrentKeys();

        /// <summary>
        /// Allow Rule to indicate validation error occured at a deeper context level.  
        /// </summary>
        /// <param name="segment">Identifier for context</param>
        public void Enter(string segment)
        {
            this.path.Push(segment);
        }

        /// <summary>
        /// Exit from path context elevel.  Enter and Exit calls should be matched.
        /// </summary>
        public void Exit()
        {
            this.path.Pop();
        }

        /// <summary>
        /// Pointer to source of validation error in document
        /// </summary>
        public string PathString
        {
            get
            {
                return "#/" + String.Join("/", path.Reverse());
            }
        }

        /// <summary>
        /// Visits <see cref="AsyncApiDocument"/>
        /// </summary>
        public virtual void Visit(AsyncApiDocument doc)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiInfo"/>
        /// </summary>
        public virtual void Visit(AsyncApiInfo info)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiContact"/>
        /// </summary>
        public virtual void Visit(AsyncApiContact contact)
        {
        }


        /// <summary>
        /// Visits <see cref="AsyncApiLicense"/>
        /// </summary>
        public virtual void Visit(AsyncApiLicense license)
        {
        }

        /// <summary>
        /// Visits list of <see cref="AsyncApiServer"/>
        /// </summary>
        public virtual void Visit(IList<AsyncApiServer> servers)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiServer"/>
        /// </summary>
        public virtual void Visit(AsyncApiServer server)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiServerVariable"/>
        /// </summary>
        public virtual void Visit(AsyncApiServerVariable serverVariable)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiOperation"/>
        /// </summary>
        public virtual void Visit(AsyncApiOperation operation)
        {
        }

        /// <summary>
        /// Visits list of <see cref="AsyncApiParameter"/>
        /// </summary>
        public virtual void Visit(IList<AsyncApiParameter> parameters)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiParameter"/>
        /// </summary>
        public virtual void Visit(AsyncApiParameter parameter)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiComponents"/>
        /// </summary>
        public virtual void Visit(AsyncApiComponents components)
        {
        }


        /// <summary>
        /// Visits <see cref="AsyncApiComponents"/>
        /// </summary>
        public virtual void Visit(AsyncApiExternalDocumentation externalDocs)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiSchema"/>
        /// </summary>
        public virtual void Visit(AsyncApiSchema schema)
        {
        }


        /// <summary>
        /// Visits <see cref="AsyncApiTag"/>
        /// </summary>
        public virtual void Visit(AsyncApiTag tag)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiOAuthFlow"/>
        /// </summary>
        public virtual void Visit(AsyncApiOAuthFlow asyncApiOAuthFlow)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiSecurityRequirement"/>
        /// </summary>
        public virtual void Visit(AsyncApiSecurityRequirement securityRequirement)
        {
        }

        /// <summary>
        /// Visits <see cref="AsyncApiSecurityScheme"/>
        /// </summary>
        public virtual void Visit(AsyncApiSecurityScheme securityScheme)
        {
        }

        /// <summary>
        /// Visits list of <see cref="AsyncApiTag"/>
        /// </summary>
        public virtual void Visit(IList<AsyncApiTag> openApiTags)
        {
        }

        /// <summary>
        /// Visits list of <see cref="AsyncApiSecurityRequirement"/>
        /// </summary>
        public virtual void Visit(IList<AsyncApiSecurityRequirement> openApiSecurityRequirements)
        {
        }

        /// <summary>
        /// Visits <see cref="IAsyncApiExtensible"/>
        /// </summary>
        public virtual void Visit(IAsyncApiExtensible openApiExtensible)
        {
        }

        /// <summary>
        /// Visits <see cref="IAsyncApiExtension"/>
        /// </summary>
        public virtual void Visit(IAsyncApiExtension openApiExtension)
        {
        }

        /// <summary>
        /// Visits a dictionary of server variables
        /// </summary>
        public virtual void Visit(IDictionary<string, AsyncApiServerVariable> serverVariables)
        {
        }

        /// <summary>
        /// Visits IAsyncApiReferenceable instances that are references and not in components
        /// </summary>
        /// <param name="referenceable">referenced object</param>
        public virtual void Visit(IAsyncApiReferenceable referenceable)
        {
        }

        public virtual void Visit(IDictionary<string, AsyncApiServer> servers)
        {
        }
    }
}
