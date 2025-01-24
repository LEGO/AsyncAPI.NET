// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Services
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;

    public class AsyncApiWalker
    {
        private readonly AsyncApiVisitorBase visitor;
        private readonly Stack<AsyncApiJsonSchema> schemaLoop = new();

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
            this.Walk(AsyncApiConstants.Tags, () => this.Walk(doc.Tags));
            this.Walk(AsyncApiConstants.ExternalDocs, () => this.Walk(doc.ExternalDocs));
            this.Walk(doc as IAsyncApiExtensible);
        }

        internal void Walk(AsyncApiComponents components)
        {
            if (components == null)
            {
                return;
            }

            this.visitor.Visit(components);

            if (components == null)
            {
                return;
            }

            this.Walk(AsyncApiConstants.Schemas, () =>
            {
                if (components.Schemas != null)
                {
                    foreach (var item in components.Schemas)
                    {
                        this.Walk(item.Key, () => this.Walk(item.Value, isComponent: true));
                    }
                }
            });

            this.Walk(AsyncApiConstants.Channels, () =>
            {
                if (components.Channels != null)
                {
                    foreach (var item in components.Channels)
                    {
                        this.Walk(item.Key, () => this.Walk(item.Value, isComponent: true));
                    }
                }
            });

            this.Walk(AsyncApiConstants.ServerBindings, () =>
            {
                if (components.ServerBindings != null)
                {
                    foreach (var item in components.ServerBindings)
                    {
                        this.Walk(item.Key, () => this.Walk(item.Value, isComponent: true));
                    }
                }
            });

            this.Walk(AsyncApiConstants.ChannelBindings, () =>
            {
                if (components.ChannelBindings != null)
                {
                    foreach (var item in components.ChannelBindings)
                    {
                        this.Walk(item.Key, () => this.Walk(item.Value, isComponent: true));
                    }
                }
            });

            this.Walk(AsyncApiConstants.OperationBindings, () =>
            {
                if (components.OperationBindings != null)
                {
                    foreach (var item in components.OperationBindings)
                    {
                        this.Walk(item.Key, () => this.Walk(item.Value, isComponent: true));
                    }
                }
            });

            this.Walk(AsyncApiConstants.MessageBindings, () =>
            {
                if (components.MessageBindings != null)
                {
                    foreach (var item in components.MessageBindings)
                    {
                        this.Walk(item.Key, () => this.Walk(item.Value, isComponent: true));
                    }
                }
            });

            this.Walk(AsyncApiConstants.Parameters, () =>
            {
                if (components.Parameters != null)
                {
                    foreach (var item in components.Parameters)
                    {
                        this.Walk(item.Key, () => this.Walk(item.Value, isComponent: true));
                    }
                }
            });

            this.Walk(AsyncApiConstants.Messages, () =>
            {
                if (components.Messages != null)
                {
                    foreach (var item in components.Messages)
                    {
                        this.Walk(item.Key, () => this.Walk(item.Value, isComponent: true));
                    }
                }
            });

            this.Walk(AsyncApiConstants.Servers, () =>
            {
                if (components.Servers != null)
                {
                    foreach (var item in components.Servers)
                    {
                        this.Walk(item.Key, () => this.Walk(item.Value, isComponent: true));
                    }
                }
            });

            this.Walk(AsyncApiConstants.CorrelationIds, () =>
            {
                if (components.CorrelationIds != null)
                {
                    foreach (var item in components.CorrelationIds)
                    {
                        this.Walk(item.Key, () => this.Walk(item.Value, isComponent: true));
                    }
                }
            });

            this.Walk(AsyncApiConstants.MessageTraits, () =>
            {
                if (components.MessageTraits != null)
                {
                    foreach (var item in components.MessageTraits)
                    {
                        this.Walk(item.Key, () => this.Walk(item.Value, isComponent: true));
                    }
                }
            });

            this.Walk(AsyncApiConstants.OperationTraits, () =>
            {
                if (components.OperationTraits != null)
                {
                    foreach (var item in components.OperationTraits)
                    {
                        this.Walk(item.Key, () => this.Walk(item.Value, isComponent: true));
                    }
                }
            });

            this.Walk(AsyncApiConstants.SecuritySchemes, () =>
            {
                if (components.SecuritySchemes != null)
                {
                    foreach (var item in components.SecuritySchemes)
                    {
                        this.Walk(item.Key, () => this.Walk(item.Value, isComponent: true));
                    }
                }
            });

            this.Walk(components as IAsyncApiExtensible);
        }

        internal void Walk(AsyncApiOAuthFlows flows)
        {
            if (flows == null)
            {
                return;
            }

            this.visitor.Visit(flows);
            this.Walk(flows as IAsyncApiExtensible);
        }

        internal void Walk(AsyncApiOAuthFlow oAuthFlow)
        {
            if (oAuthFlow == null)
            {
                return;
            }

            this.visitor.Visit(oAuthFlow);
            this.Walk(oAuthFlow as IAsyncApiExtensible);
        }

        internal void Walk(AsyncApiSecurityScheme securityScheme, bool isComponent = false)
        {
            if (securityScheme is AsyncApiSecuritySchemeReference)
            {
                this.Walk(securityScheme as IAsyncApiReferenceable);
                return;
            }

            this.visitor.Visit(securityScheme);

            if (securityScheme != null)
            {
                this.Walk(AsyncApiConstants.Flows, () => this.Walk(securityScheme.Flows));
            }

            this.Walk(securityScheme as IAsyncApiExtensible);
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

        internal void Walk(AsyncApiExternalDocumentation externalDocs)
        {
            if (externalDocs == null)
            {
                return;
            }

            this.visitor.Visit(externalDocs);

            this.Walk(externalDocs as IAsyncApiExtensible);
        }

        internal void Walk(AsyncApiChannel channel, bool isComponent = false)
        {
            if (channel is AsyncApiChannelReference)
            {
                this.Walk(channel as IAsyncApiReferenceable);
                return;
            }

            this.visitor.Visit(channel);

            if (channel != null)
            {
                this.Walk(AsyncApiConstants.Subscribe, () => this.Walk(channel.Subscribe));
                this.Walk(AsyncApiConstants.Publish, () => this.Walk(channel.Publish));

                this.Walk(AsyncApiConstants.Bindings, () => this.Walk(channel.Bindings));
                this.Walk(AsyncApiConstants.Parameters, () => this.Walk(channel.Parameters));
            }

            this.Walk(channel as IAsyncApiExtensible);
        }

        private void Walk(IDictionary<string, AsyncApiParameter> parameters)
        {
            if (parameters == null)
            {
                return;
            }

            this.visitor.Visit(parameters);

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    this.visitor.CurrentKeys.Parameter = parameter.Key;
                    this.Walk(parameter.Key, () => this.Walk(parameter.Value));
                    this.visitor.CurrentKeys.Parameter = null;
                }
            }
        }

        internal void Walk(AsyncApiParameter parameter, bool isComponent = false)
        {
            if (parameter is AsyncApiParameterReference reference)
            {
                this.Walk(reference as IAsyncApiReferenceable);
                return;
            }

            this.visitor.Visit(parameter);

            if (parameter != null)
            {
                this.Walk(AsyncApiConstants.Schema, () => this.Walk(parameter.Schema));
            }

            this.Walk(parameter as IAsyncApiExtensible);
        }

        internal void Walk(AsyncApiAvroSchemaPayload payload)
        {
            this.visitor.Visit(payload);
        }

        internal void Walk(AsyncApiJsonSchema schema, bool isComponent = false)
        {
            if (schema == null || this.ProcessAsReference(schema, isComponent))
            {
                return;
            }

            if (this.schemaLoop.Contains(schema))
            {
                return;  // Loop detected, this schema has already been walked.
            }
            else
            {
                this.schemaLoop.Push(schema);
            }

            this.visitor.Visit(schema);

            if (schema.Items != null)
            {
                this.Walk("items", () => this.Walk(schema.Items));
            }

            if (schema.Default != null)
            {
                this.Walk(AsyncApiConstants.Default, () => this.Walk(schema.Default));
            }

            if (schema.AllOf != null)
            {
                foreach (var item in schema.AllOf)
                {
                    this.Walk("allOf", () => this.Walk(item));
                }
            }

            if (schema.AnyOf != null)
            {
                foreach (var item in schema.AnyOf)
                {
                    this.Walk("anyOf", () => this.Walk(item));
                }
            }

            if (schema.Not != null)
            {
                this.Walk("not", () => this.Walk(schema.Not));
            }

            if (schema.Contains != null)
            {
                this.Walk("contains", () => this.Walk(schema.Contains));
            }

            if (schema.If != null)
            {
                this.Walk("if", () => this.Walk(schema.If));
            }

            if (schema.Then != null)
            {
                this.Walk("then", () => this.Walk(schema.Then));
            }

            if (schema.Else != null)
            {
                this.Walk("else", () => this.Walk(schema.Else));
            }

            if (schema.OneOf != null)
            {
                foreach (var item in schema.OneOf)
                {
                    this.Walk("oneOf", () => this.Walk(item));
                }
            }

            if (schema.Properties != null)
            {
                this.Walk("properties", () =>
                {
                    foreach (var item in schema.Properties)
                    {
                        this.Walk(item.Key, () => this.Walk(item.Value));
                    }
                });
            }

            if (schema.AdditionalProperties != null)
            {
                this.Walk("additionalProperties", () => this.Walk(schema.AdditionalProperties));
            }

            if (schema.PatternProperties != null)
            {
                this.Walk("patternProperties", () =>
                {
                    foreach (var item in schema.PatternProperties)
                    {
                        this.Walk(item.Key, () => this.Walk(item.Value));
                    }
                });
            }

            if (schema.PropertyNames != null)
            {
                this.Walk("propertyNames", () => this.Walk(schema.PropertyNames));
            }

            if (schema.Enum != null)
            {
                foreach (var item in schema.Enum)
                {
                    this.Walk("enum", () => this.Walk(item));
                }
            }

            if (schema.Examples != null)
            {
                foreach (var item in schema.Examples)
                {
                    this.Walk("examples", () => this.Walk(item));
                }
            }

            if (schema.Const != null)
            {
                this.Walk("const", () => this.Walk(schema.Const));
            }

            this.Walk(AsyncApiConstants.ExternalDocs, () => this.Walk(schema.ExternalDocs));

            this.Walk(schema as IAsyncApiExtensible);

            this.schemaLoop.Pop();
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
                this.Walk(AsyncApiConstants.Tags, () => this.Walk(operation.Tags));
                this.Walk(AsyncApiConstants.ExternalDocs, () => this.Walk(operation.ExternalDocs));
                this.Walk(AsyncApiConstants.Traits, () => this.Walk(operation.Traits));
                this.Walk(AsyncApiConstants.Message, () => this.Walk(operation.Message));
                this.Walk(AsyncApiConstants.Bindings, () => this.Walk(operation.Bindings));
            }

            this.Walk(operation as IAsyncApiExtensible);
        }

        private void Walk(IList<AsyncApiOperationTrait> traits)
        {
            if (traits == null)
            {
                return;
            }

            this.visitor.Visit(traits);

            // Visit traits
            if (traits != null)
            {
                for (int i = 0; i < traits.Count; i++)
                {
                    this.Walk(i.ToString(), () => this.Walk(traits[i]));
                }
            }
        }

        internal void Walk(AsyncApiOperationTrait trait, bool isComponent = false)
        {
            if (trait == null || this.ProcessAsReference(trait, isComponent))
            {
                return;
            }

            this.visitor.Visit(trait);

            if (trait != null)
            {
                this.Walk(AsyncApiConstants.ExternalDocs, () => this.Walk(trait.ExternalDocs));
                this.Walk(AsyncApiConstants.Tags, () => this.Walk(trait.Tags));
                this.Walk(AsyncApiConstants.Bindings, () => this.Walk(trait.Bindings));
            }

            this.Walk(trait as IAsyncApiExtensible);
        }

        internal void Walk(AsyncApiMessage message, bool isComponent = false)
        {
            if (message is AsyncApiMessageReference reference)
            {
                this.Walk(reference as IAsyncApiReferenceable);
                return;
            }

            this.visitor.Visit(message);

            if (message != null)
            {
                this.Walk(AsyncApiConstants.Headers, () => this.Walk(message.Headers));
                if (message.Payload is AsyncApiJsonSchemaPayload payload)
                {
                    this.Walk(AsyncApiConstants.Payload, () => this.Walk((AsyncApiJsonSchema)payload));
                }

                if (message.Payload is AsyncApiAvroSchemaPayload avroPayload)
                {
                    this.Walk(AsyncApiConstants.Payload, () => this.Walk(avroPayload));
                }

                this.Walk(AsyncApiConstants.CorrelationId, () => this.Walk(message.CorrelationId));
                this.Walk(AsyncApiConstants.Tags, () => this.Walk(message.Tags));
                this.Walk(AsyncApiConstants.Examples, () => this.Walk(message.Examples));
                this.Walk(AsyncApiConstants.ExternalDocs, () => this.Walk(message.ExternalDocs));
                this.Walk(AsyncApiConstants.Traits, () => this.Walk(message.Traits));
                this.Walk(AsyncApiConstants.Bindings, () => this.Walk(message.Bindings));
            }

            this.Walk(message as IAsyncApiExtensible);
        }

        private void Walk(IList<AsyncApiMessageTrait> traits)
        {
            if (traits == null)
            {
                return;
            }

            this.visitor.Visit(traits);

            // Visit traits
            if (traits != null)
            {
                for (int i = 0; i < traits.Count; i++)
                {
                    this.Walk(i.ToString(), () => this.Walk(traits[i]));
                }
            }
        }

        internal void Walk(AsyncApiMessageTrait trait, bool isComponent = false)
        {
            if (trait == null || this.ProcessAsReference(trait, isComponent))
            {
                return;
            }

            this.visitor.Visit(trait);

            if (trait != null)
            {
                this.Walk(AsyncApiConstants.Headers, () => this.Walk(trait.Headers));
                this.Walk(AsyncApiConstants.CorrelationId, () => this.Walk(trait.CorrelationId));
                this.Walk(AsyncApiConstants.Tags, () => this.Walk(trait.Tags));
                this.Walk(AsyncApiConstants.Examples, () => this.Walk(trait.Examples));
                this.Walk(AsyncApiConstants.ExternalDocs, () => this.Walk(trait.ExternalDocs));
                this.Walk(AsyncApiConstants.Bindings, () => this.Walk(trait.Bindings));
            }

            this.Walk(trait as IAsyncApiExtensible);
        }

        internal void Walk(AsyncApiBindings<IServerBinding> serverBindings, bool isComponent = false)
        {
            if (serverBindings == null || this.ProcessAsReference(serverBindings, isComponent))
            {
                return;
            }

            this.visitor.Visit(serverBindings);
            if (serverBindings != null)
            {
                foreach (var binding in serverBindings)
                {
                    this.visitor.CurrentKeys.ServerBinding = binding.Key;
                    this.Walk(binding.Key, () => this.Walk(binding.Value));
                    this.visitor.CurrentKeys.ServerBinding = null;
                }
            }
        }

        internal void Walk(IServerBinding binding)
        {
            if (binding == null)
            {
                return;
            }

            this.visitor.Visit(binding);
        }

        internal void Walk(AsyncApiBindings<IChannelBinding> channelBindings, bool isComponent = false)
        {
            if (channelBindings == null || this.ProcessAsReference(channelBindings, isComponent))
            {
                return;
            }

            this.visitor.Visit(channelBindings);
            if (channelBindings != null)
            {
                foreach (var binding in channelBindings)
                {
                    this.visitor.CurrentKeys.ChannelBinding = binding.Key;
                    this.Walk(binding.Key, () => this.Walk(binding.Value));
                    this.visitor.CurrentKeys.ChannelBinding = null;
                }
            }
        }

        internal void Walk(IChannelBinding binding)
        {
            if (binding == null)
            {
                return;
            }

            this.visitor.Visit(binding);
        }

        internal void Walk(AsyncApiBindings<IOperationBinding> operationBindings, bool isComponent = false)
        {
            if (operationBindings == null || this.ProcessAsReference(operationBindings, isComponent))
            {
                return;
            }

            this.visitor.Visit(operationBindings);
            if (operationBindings != null)
            {
                foreach (var binding in operationBindings)
                {
                    this.visitor.CurrentKeys.OperationBinding = binding.Key;
                    this.Walk(binding.Key, () => this.Walk(binding.Value));
                    this.visitor.CurrentKeys.OperationBinding = null;
                }
            }
        }

        internal void Walk(IOperationBinding binding)
        {
            if (binding == null)
            {
                return;
            }

            this.visitor.Visit(binding);
        }

        internal void Walk(AsyncApiBindings<IMessageBinding> messageBindings, bool isComponent = false)
        {
            if (messageBindings == null || this.ProcessAsReference(messageBindings, isComponent))
            {
                return;
            }

            this.visitor.Visit(messageBindings);
            if (messageBindings != null)
            {
                foreach (var binding in messageBindings)
                {
                    this.visitor.CurrentKeys.MessageBinding = binding.Key;
                    this.Walk(binding.Key, () => this.Walk(binding.Value));
                    this.visitor.CurrentKeys.MessageBinding = null;
                }
            }
        }

        internal void Walk(IMessageBinding binding)
        {
            if (binding == null)
            {
                return;
            }

            this.visitor.Visit(binding);
        }

        internal void Walk(IList<AsyncApiMessageExample> examples)
        {
            if (examples == null)
            {
                return;
            }

            this.visitor.Visit(examples);

            if (examples != null)
            {
                for (int i = 0; i < examples.Count; i++)
                {
                    this.Walk(i.ToString(), () => this.Walk(examples[i]));
                }
            }
        }

        internal void Walk(AsyncApiMessageExample example)
        {
            if (example == null)
            {
                return;
            }

            this.visitor.Visit(example);

            if (example != null)
            {
                this.Walk(AsyncApiConstants.Headers, () => this.Walk(example.Headers));
                this.Walk(AsyncApiConstants.Payload, () => this.Walk(example.Payload));
            }

            this.Walk(example as IAsyncApiExtensible);
        }

        internal void Walk(IDictionary<string, AsyncApiAny> anys)
        {
            if (anys == null)
            {
                return;
            }

            this.visitor.Visit(anys);

            if (anys != null)
            {
                foreach (var any in anys)
                {
                    this.visitor.CurrentKeys.Anys = any.Key;
                    this.Walk(any.Key, () => this.Walk(any.Value));
                    this.visitor.CurrentKeys.Anys = null;
                }
            }
        }

        internal void Walk(AsyncApiCorrelationId correlationId, bool isComponent = false)
        {
            if (correlationId is AsyncApiCorrelationIdReference)
            {
                this.Walk(correlationId as IAsyncApiReferenceable);
                return;
            }

            this.visitor.Visit(correlationId);

            this.Walk(correlationId as IAsyncApiExtensible);
        }

        internal void Walk(AsyncApiTag tag)
        {
            if (tag == null)
            {
                return;
            }

            this.visitor.Visit(tag);
            this.visitor.Visit(tag.ExternalDocs);
            this.visitor.Visit(tag as IAsyncApiExtensible);
        }

        internal void Walk(IList<AsyncApiTag> tags)
        {
            if (tags == null)
            {
                return;
            }

            this.visitor.Visit(tags);

            // Visit tags
            if (tags != null)
            {
                for (int i = 0; i < tags.Count; i++)
                {
                    this.Walk(i.ToString(), () => this.Walk(tags[i]));
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
            if (server is AsyncApiServerReference)
            {
                this.Walk(server as IAsyncApiReferenceable);
                return;
            }

            this.visitor.Visit(server);
            this.Walk(AsyncApiConstants.Variables, () => this.Walk(server.Variables));
            this.Walk(AsyncApiConstants.Security, () => this.Walk(server.Security));
            this.Walk(AsyncApiConstants.Bindings, () => this.Walk(server.Bindings));
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

        internal void Walk(IList<AsyncApiMessage> messages)
        {
            if (messages == null)
            {
                return;
            }

            this.visitor.Visit(messages);

            // Visit traits
            if (messages != null)
            {
                for (int i = 0; i < messages.Count; i++)
                {
                    this.Walk(i.ToString(), () => this.Walk(messages[i]));
                }
            }
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

        internal void Walk(AsyncApiServerVariable serverVariable)
        {
            if (serverVariable == null)
            {
                return;
            }

            this.visitor.Visit(serverVariable);
            this.Walk(serverVariable as IAsyncApiExtensible);
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

        internal void Walk(AsyncApiAny any)
        {
            if (any == null)
            {
                return;
            }

            this.visitor.Visit(any);
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
                    this.Walk(item.Key, () => this.Walk(item.Value));
                    this.visitor.CurrentKeys.Extension = null;
                }
            }
        }

        internal void Walk(IAsyncApiExtension extension)
        {
            if (extension == null)
            {
                return;
            }

            this.visitor.Visit(extension);
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

        public void Walk(IAsyncApiElement element)
        {
            if (element == null)
            {
                return;
            }

            switch (element)
            {
                case AsyncApiDocument e: this.Walk(e); break;
                case AsyncApiLicense e: this.Walk(e); break;
                case AsyncApiInfo e: this.Walk(e); break;
                case AsyncApiComponents e: this.Walk(e); break;
                case AsyncApiContact e: this.Walk(e); break;
                case AsyncApiCorrelationId e: this.Walk(e); break;
                case AsyncApiMessageExample e: this.Walk(e); break;
                case AsyncApiChannel e: this.Walk(e); break;
                case AsyncApiExternalDocumentation e: this.Walk(e); break;
                case AsyncApiMessage e: this.Walk(e); break;
                case AsyncApiMessageTrait e: this.Walk(e); break;
                case AsyncApiOperationTrait e: this.Walk(e); break;
                case AsyncApiOAuthFlows e: this.Walk(e); break;
                case AsyncApiOAuthFlow e: this.Walk(e); break;
                case AsyncApiOperation e: this.Walk(e); break;
                case AsyncApiParameter e: this.Walk(e); break;
                case AsyncApiJsonSchema e: this.Walk(e); break;
                case AsyncApiSecurityRequirement e: this.Walk(e); break;
                case AsyncApiSecurityScheme e: this.Walk(e); break;
                case AsyncApiServer e: this.Walk(e); break;
                case AsyncApiServerVariable e: this.Walk(e); break;
                case AsyncApiTag e: this.Walk(e); break;
                case IList<AsyncApiTag> e: this.Walk(e); break;
                case IDictionary<string, AsyncApiServerVariable> e:
                    this.Walk(e);
                    break;
                case IAsyncApiExtensible e: this.Walk(e); break;
                case IAsyncApiExtension e: this.Walk(e); break;
            }
        }
    }
}
