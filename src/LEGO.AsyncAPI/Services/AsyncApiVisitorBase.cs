// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    /// <summary>
    /// AsyncApi visitor base provides common logic for concrete visitors
    /// </summary>
    public abstract class AsyncApiVisitorBase
    {
        private readonly Stack<string> path = new Stack<string>();

        /// <summary>
        /// Properties available to identify context of where an object is within AsyncApi Document
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

        public virtual void Visit(IDictionary<string, IAsyncApiAny> anys)
        { }

        public virtual void Visit(IList<AsyncApiMessageTrait> traits)
        { }
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

        public virtual void Visit(IDictionary<string, AsyncApiParameter> parameters)
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

        public virtual void Visit(IList<AsyncApiMessage> messages)
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
        /// Visits <see cref="AsyncApiBindings<TBinding>"/>
        /// </summary>
        public virtual void Visit<TBinding>(AsyncApiBindings<TBinding> bindings)
            where TBinding : class, IBinding
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
        public virtual void Visit(IList<AsyncApiTag> AsyncApiTags)
        {
        }

        /// <summary>
        /// Visits list of <see cref="AsyncApiSecurityRequirement"/>
        /// </summary>
        public virtual void Visit(IList<AsyncApiSecurityRequirement> AsyncApiSecurityRequirements)
        {
        }

        /// <summary>
        /// Visits <see cref="IAsyncApiExtensible"/>
        /// </summary>
        public virtual void Visit(IAsyncApiExtensible AsyncApiExtensible)
        {
        }

        public virtual void Visit(AsyncApiCorrelationId correlationId)
        { }

        public virtual void Visit(AsyncApiMessageTrait trait)
        { }

        /// <summary>
        /// Visits <see cref="IAsyncApiExtension"/>
        /// </summary>
        public virtual void Visit(IAsyncApiExtension AsyncApiExtension)
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

        public virtual void Visit(AsyncApiMessageExample messageExample)
        {
        }

        public virtual void Visit(IList<AsyncApiMessageExample> messageExamples)
        {
        }
    }
}
