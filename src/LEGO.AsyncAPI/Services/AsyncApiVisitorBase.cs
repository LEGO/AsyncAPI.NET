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

            this.Walk(AsyncApiConstants.Info, () => this.Walk(doc.Info));
            this.Walk(AsyncApiConstants.Servers, () => this.Walk(doc.Servers));
            this.Walk(AsyncApiConstants.Channels, () => this.Walk(doc.Channels));
            this.Walk(AsyncApiConstants.Components, () => this.Walk(doc.Components));
            Walk(AsyncApiConstants.Tags, () => Walk(doc.Tags));
            this.Walk(AsyncApiConstants.ExternalDocs, () => this.Walk(doc.ExternalDocs));
            this.Walk(doc as IAsyncApiExtensible);
        }

        internal void Walk(IDictionary<string, AsyncApiChannel> channels)
        {
            if (channels == null)
            {
                return;
            }

            this.visitor.Visit(channels);

            if (channels != null)
            {
                foreach (var channel in channels)
                {
                    this.visitor.CurrentKeys.Channel = channel.Key;
                    this.Walk(channel.Key, () => this.Walk(channel.Value));
                    this.visitor.CurrentKeys.Channel = null;
                }
            }
        }

        internal void Walk(AsyncApiChannel channel, bool isComponent = false)
        {
            if (channel == null || ProcessAsReference(channel, isComponent))
            {
                return;
            }

            this.visitor.Visit(channel);

            if (channel != null)
            {
                this.Walk(AsyncApiConstants.Subscribe, () => this.Walk(channel.Subscribe));
                this.Walk(AsyncApiConstants.Publish, () => this.Walk(channel.Publish));
                // TODO: Figure out bindings
                Walk(AsyncApiConstants.Parameters, () => Walk(channel.Parameters));
            }

            this.Walk(channel as IAsyncApiExtensible);
        }

        internal void Walk(AsyncApiOperation operation)
        {
            if (operation == null)
            {
                return;
            }

            this.visitor.Visit(operation);

            if (operation != null)
            {
                Walk(AsyncApiConstants.Tags, () => this.Walk(operation.Tags));
                Walk(AsyncApiConstants.ExternalDocs, () => this.Walk(operation.ExternalDocs));
                Walk(AsyncApiConstants.Traits, () => this.Walk(operation.Traits));
                //TODO: Figure out bindings
                Walk(AsyncApiConstants.Message, () => this.Walk(operation.Message));
            }

            this.Walk(operation as IAsyncApiExtensible);
        }

        private void Walk(IList<AsyncApiOperationTrait> traits)
        {
            if (traits == null)
            {
                return;
            }

            visitor.Visit(traits);

            // Visit traits
            if (traits != null)
            {
                for (int i = 0; i < traits.Count; i++)
                {
                    Walk(i.ToString(), () => Walk(traits[i]));
                }
            }
        }

        internal void Walk (AsyncApiOperationTrait trait, bool isComponent = false)
        {
            if (trait == null || ProcessAsReference(trait, isComponent))
            {
                return;
            }

            visitor.Visit(trait);

            if(trait != null)
            {
                Walk(AsyncApiConstants.ExternalDocs, () => Walk(trait.ExternalDocs));
                Walk(AsyncApiConstants.Tags, () => Walk(trait.Tags));
                // Todo: Figure out bindings;
            }

            this.Walk(trait as IAsyncApiExtensible);
        }

        internal void Walk (AsyncApiMessage message, bool isComponent = false)
        {
            if(message == null || ProcessAsReference(message, isComponent))
            {
                return;
            }

            this.visitor.Visit(message);

            if (message != null)
            {
                Walk(AsyncApiConstants.Headers, () => Walk(message.Headers));
                sdlfkjhsdf
            }

            this.Walk(message as IAsyncApiExtensible);
        }

        internal void Walk(AsyncApiTag tag)
        {
            if (tag == null)
            {
                return;
            }

            visitor.Visit(tag);
            visitor.Visit(tag.ExternalDocs);
            visitor.Visit(tag as IAsyncApiExtensible);
        }

        internal void Walk(IList<AsyncApiTag> tags)
        {
            if (tags == null)
            {
                return;
            }

            visitor.Visit(tags);

            // Visit tags
            if (tags != null)
            {
                for (int i = 0; i < tags.Count; i++)
                {
                    Walk(i.ToString(), () => Walk(tags[i]));
                }
            }
        }

        internal void Walk(AsyncApiInfo info)
        {
            if (info == null)
            {
                return;
            }

            this.visitor.Visit(info);

            if (info != null)
            {
                this.Walk(AsyncApiConstants.Contact, () => this.Walk(info.Contact));
                this.Walk(AsyncApiConstants.License, () => this.Walk(info.License));
            }

            this.Walk(info as IAsyncApiExtensible);
        }

        internal void Walk(AsyncApiServer server, bool isComponent = false)
        {
            if (server == null || ProcessAsReference(server, isComponent))
            {
                return;
            }

            this.visitor.Visit(server);
            this.Walk(AsyncApiConstants.Variables, () => this.Walk(server.Variables));
            this.Walk(AsyncApiConstants.Security, () => this.Walk(server.Security));
            // TODO: Figure out bindings
            this.visitor.Visit(server as IAsyncApiExtensible);
        }

        internal void Walk(IList<AsyncApiSecurityRequirement> securityRequirements)
        {
            if (securityRequirements == null)
            {
                return;
            }

            this.visitor.Visit(securityRequirements);

            // Visit Examples
            if (securityRequirements != null)
            {
                for (int i = 0; i < securityRequirements.Count; i++)
                {
                    this.Walk(i.ToString(), () => this.Walk(securityRequirements[i]));
                }
            }
        }

        internal void Walk(AsyncApiSecurityRequirement securityRequirement)
        {
            if (securityRequirement is null)
            {
                return;
            }

            this.visitor.Visit(securityRequirement);
            this.Walk(securityRequirement as IAsyncApiExtensible);
        }

        internal void Walk(IDictionary<string, AsyncApiServer> servers)
        {
            if (servers == null)
            {
                return;
            }

            this.visitor.Visit(servers);

            if (servers != null)
            {
                foreach (var server in servers)
                {
                    this.visitor.CurrentKeys.Server = server.Key;
                    this.Walk(server.Key, () => this.Walk(server.Value));
                    this.visitor.CurrentKeys.Server = null;
                }
            }
        }

        internal void Walk(IDictionary<string, AsyncApiServerVariable> serverVariables)
        {
            if (serverVariables == null)
            {
                return;
            }

            this.visitor.Visit(serverVariables);

            if (serverVariables != null)
            {
                foreach (var variable in serverVariables)
                {
                    this.visitor.CurrentKeys.ServerVariable = variable.Key;
                    this.Walk(variable.Key, () => this.Walk(variable.Value));
                    this.visitor.CurrentKeys.ServerVariable = null;
                }
            }
        }

        internal void Walk(AsyncApiLicense license)
        {
            if (license == null)
            {
                return;
            }

            this.visitor.Visit(license);
        }

        internal void Walk(AsyncApiContact contact)
        {
            if (contact == null)
            {
                return;
            }

            this.visitor.Visit(contact);
        }

        internal void Walk(IAsyncApiAny any)
        {
            if (any == null)
            {
                return;
            }

            visitor.Visit(any);
        }

        private void Walk(string context, Action walk)
        {
            this.visitor.Enter(context.Replace("/", "~1"));
            walk();
            this.visitor.Exit();
        }

        internal void Walk(IAsyncApiExtensible asyncApiExtensible)
        {
            if (asyncApiExtensible == null)
            {
                return;
            }

            this.visitor.Visit(asyncApiExtensible);

            if (asyncApiExtensible != null)
            {
                foreach (var item in asyncApiExtensible.Extensions)
                {
                    this.visitor.CurrentKeys.Extension = item.Key;
                    Walk(item.Key, () => Walk(item.Value));
                    this.visitor.CurrentKeys.Extension = null;
                }
            }
        }

        private bool ProcessAsReference(IAsyncApiReferenceable referenceable, bool isComponent = false)
        {
            var isReference = referenceable.Reference != null && !isComponent;
            if (isReference)
            {
                this.Walk(referenceable);
            }

            return isReference;
        }

        internal void Walk(IAsyncApiReferenceable referenceable)
        {
            this.visitor.Visit(referenceable);
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
                return "#/" + String.Join("/", this.path.Reverse());
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

        public virtual void Visit(AsyncApiMessage message)
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
        /// <param name="referencable">referenced object</param>
        public virtual void Visit(IAsyncApiReferenceable referencable)
        {
        }

        public virtual void Visit(IDictionary<string, AsyncApiServer> servers)
        {
        }

        public virtual void Visit(IDictionary<string, AsyncApiChannel> channels)
        {
        }

        public virtual void Visit(AsyncApiChannel channel)
        {
        }

        public virtual void Visit(IList<AsyncApiOperationTrait> traits)
        {
        }

        public virtual void Visit(AsyncApiOperationTrait traits)
        {
        }
    }
}
