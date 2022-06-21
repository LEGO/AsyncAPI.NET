﻿// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Writers;

    /// <summary>
    /// Holds a set of reusable objects for different aspects of the AsyncAPI specification.
    /// </summary>
    /// <remarks>
    /// All objects defined within the components object will have no effect on the API unless they are explicitly referenced from properties outside the components object.
    /// </remarks>
    public class AsyncApiComponents : IAsyncApiExtensible, IAsyncApiSerializable
    {
        public IDictionary<string, AsyncApiSchema> Schemas { get; set; } = new Dictionary<string, AsyncApiSchema>();

        public IDictionary<string, AsyncApiServer> Servers { get; set; } = new Dictionary<string, AsyncApiServer>();

        public IDictionary<string, AsyncApiChannel> Channels { get; set; } = new Dictionary<string, AsyncApiChannel>();

        public IDictionary<string, AsyncApiMessage> Messages { get; set; } = new Dictionary<string, AsyncApiMessage>();
        
        public IDictionary<string, SecurityScheme> SecuritySchemes { get; set; } = new Dictionary<string, SecurityScheme>();
        
        public IDictionary<string, AsyncApiParameter> Parameters { get; set; } = new Dictionary<string, AsyncApiParameter>();
        
        public IDictionary<string, CorrelationId> CorrelationIds { get; set; } = new Dictionary<string, CorrelationId>();

        public IDictionary<string, AsyncApiOperationTrait> OperationTraits { get; set; } = new Dictionary<string, AsyncApiOperationTrait>();
        
        public IDictionary<string, MessageTrait> MessageTraits { get; set; } = new Dictionary<string, MessageTrait>();
        
        public IDictionary<string, IServerBinding> ServerBindings { get; set; } = new Dictionary<string, IServerBinding>();
        
        public IDictionary<string, IChannelBinding> ChannelBindings { get; set; } = new Dictionary<string, IChannelBinding>();
        
        public IDictionary<string, IOperationBinding> OperationBindings { get; set; } = new Dictionary<string, IOperationBinding>();

        public IDictionary<string, IMessageBinding> MessageBindings { get; set; } = new Dictionary<string, IMessageBinding>();

        public IDictionary<string, IAsyncApiExtension> Extensions { get; set; } = new Dictionary<string, IAsyncApiExtension>();

        public void SerializeV2(IAsyncApiWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            // If references have been inlined we don't need the to render the components section
            // however if they have cycles, then we will need a component rendered
            if (writer.GetSettings().ReferenceInline != ReferenceInlineSetting.DoNotInlineReferences)
            {
                var loops = writer.GetSettings().LoopDetector.Loops;
                writer.WriteStartObject();
                if (loops.TryGetValue(typeof(AsyncApiSchema), out List<object> schemas))
                {
                    var openApiSchemas = schemas.Cast<AsyncApiSchema>().Distinct().ToList()
                        .ToDictionary<AsyncApiSchema, string>(k => k.Reference.Id);

                    writer.WriteOptionalMap(
                       AsyncApiConstants.Schemas,
                       this.Schemas,
                       (w, key, component) =>
                        {
                           component.SerializeV2WithoutReference(w);
                        });
                }

                writer.WriteEndObject();
                return;
            }

            writer.WriteStartObject();

            // Serialize each referenceable object as full object without reference if the reference in the object points to itself.
            // If the reference exists but points to other objects, the object is serialized to just that reference.

            // schemas
            writer.WriteOptionalMap(
                AsyncApiConstants.Schemas,
                this.Schemas,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.Schema &&
                        component.Reference.Id == key)
                    {
                        component.SerializeV2WithoutReference(w);
                    }
                    else
                    {
                        component.SerializeV2(w);
                    }
                });

            // servers
            writer.WriteOptionalMap(
                AsyncApiConstants.Servers,
                this.Servers,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.Server &&
                        component.Reference.Id == key)
                    {
                        component.SerializeV2WithoutReference(w);
                    }
                    else
                    {
                        component.SerializeV2(w);
                    }
                });

            // channels
            writer.WriteOptionalMap(
                AsyncApiConstants.Channels,
                this.Channels,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.Channel &&
                        component.Reference.Id == key)
                    {
                        component.SerializeV2WithoutReference(w);
                    }
                    else
                    {
                        component.SerializeV2(w);
                    }
                });

            // messages
            writer.WriteOptionalMap(
                AsyncApiConstants.Message,
                this.Messages,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.Message &&
                        component.Reference.Id == key)
                    {
                        component.SerializeV2WithoutReference(w);
                    }
                    else
                    {
                        component.SerializeV2(w);
                    }
                });

            // securitySchemes
            writer.WriteOptionalMap(
                AsyncApiConstants.SecuritySchemes,
                this.SecuritySchemes,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.SecurityScheme &&
                        component.Reference.Id == key)
                    {
                        component.SerializeV2WithoutReference(w);
                    }
                    else
                    {
                        component.SerializeV2(w);
                    }
                });

            // parameters
            writer.WriteOptionalMap(
                AsyncApiConstants.Parameters,
                this.Parameters,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.Parameter &&
                        component.Reference.Id == key)
                    {
                        component.SerializeV2WithoutReference(w);
                    }
                    else
                    {
                        component.SerializeV2(w);
                    }
                });

            // correlationIds
            writer.WriteOptionalMap(
                AsyncApiConstants.CorrelationId,
                this.CorrelationIds,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.CorrelationId &&
                        component.Reference.Id == key)
                    {
                        component.SerializeV2WithoutReference(w);
                    }
                    else
                    {
                        component.SerializeV2(w);
                    }
                });

            // operationTraits
            writer.WriteOptionalMap(
                AsyncApiConstants.OperationTrait,
                this.OperationTraits,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.OperationTrait &&
                        component.Reference.Id == key)
                    {
                        component.SerializeV2WithoutReference(w);
                    }
                    else
                    {
                        component.SerializeV2(w);
                    }
                });

            // messageTraits
            writer.WriteOptionalMap(
                AsyncApiConstants.MessageTrait,
                this.MessageTraits,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.MessageTrait &&
                        component.Reference.Id == key)
                    {
                        component.SerializeV2WithoutReference(w);
                    }
                    else
                    {
                        component.SerializeV2(w);
                    }
                });

            // serverBindings
            writer.WriteOptionalMap(
                AsyncApiConstants.ServerBinding,
                this.ServerBindings,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.ServerBinding &&
                        component.Reference.Id == key)
                    {
                        component.SerializeV2WithoutReference(w);
                    }
                    else
                    {
                        component.SerializeV2(w);
                    }
                });

            // channelBindings
            writer.WriteOptionalMap(
                AsyncApiConstants.ChannelBinding,
                this.ChannelBindings,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.ChannelBinding &&
                        component.Reference.Id == key)
                    {
                        component.SerializeV2WithoutReference(w);
                    }
                    else
                    {
                        component.SerializeV2(w);
                    }
                });

            // operationBindings
            writer.WriteOptionalMap(
                AsyncApiConstants.OperationBinding,
                this.OperationBindings,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.OperationBinding &&
                        component.Reference.Id == key)
                    {
                        component.SerializeV2WithoutReference(w);
                    }
                    else
                    {
                        component.SerializeV2(w);
                    }
                });

            // messageBindings
            writer.WriteOptionalMap(
                AsyncApiConstants.MessageBinding,
                this.MessageBindings,
                (w, key, component) =>
                {
                    if (component.Reference != null &&
                        component.Reference.Type == ReferenceType.MessageBinding &&
                        component.Reference.Id == key)
                    {
                        component.SerializeV2WithoutReference(w);
                    }
                    else
                    {
                        component.SerializeV2(w);
                    }
                });

            // extensions
            writer.WriteExtensions(this.Extensions, AsyncApiVersion.AsyncApi2_3_0);

            writer.WriteEndObject();
        }
    }
}