﻿namespace LEGO.AsyncAPI.Services
{
    using System;
    using System.Collections.Generic;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;

    public class AsyncApiWalker
    {
        private readonly AsyncApiVisitorBase visitor;
        private readonly Stack<AsyncApiSchema> schemaLoop = new ();

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

            visitor.Visit(components);

            if (components == null)
            {
                return;
            }

            Walk(AsyncApiConstants.Schemas, () =>
            {
                if (components.Schemas != null)
                {
                    foreach (var item in components.Schemas)
                    {
                        Walk(item.Key, () => Walk(item.Value, isComponent: true));
                    }
                }
            });

            Walk(AsyncApiConstants.Channels, () =>
            {
                if (components.Channels != null)
                {
                    foreach (var item in components.Channels)
                    {
                        Walk(item.Key, () => Walk(item.Value, isComponent: true));
                    }
                }
            });

            Walk(AsyncApiConstants.Parameters, () =>
            {
                if (components.Parameters != null)
                {
                    foreach (var item in components.Parameters)
                    {
                        Walk(item.Key, () => Walk(item.Value, isComponent: true));
                    }
                }
            });

            Walk(AsyncApiConstants.Messages, () =>
            {
                if (components.Messages != null)
                {
                    foreach (var item in components.Messages)
                    {
                        Walk(item.Key, () => Walk(item.Value, isComponent: true));
                    }
                }
            });

            Walk(AsyncApiConstants.Servers, () =>
            {
                if (components.Servers != null)
                {
                    foreach (var item in components.Servers)
                    {
                        Walk(item.Key, () => Walk(item.Value, isComponent: true));
                    }
                }
            });

            Walk(AsyncApiConstants.CorrelationIds, () =>
            {
                if (components.CorrelationIds != null)
                {
                    foreach (var item in components.CorrelationIds)
                    {
                        Walk(item.Key, () => Walk(item.Value, isComponent: true));
                    }
                }
            });

            Walk(AsyncApiConstants.MessageTraits, () =>
            {
                if (components.MessageTraits != null)
                {
                    foreach (var item in components.MessageTraits)
                    {
                        Walk(item.Key, () => Walk(item.Value, isComponent: true));
                    }
                }
            });

            Walk(AsyncApiConstants.OperationTraits, () =>
            {
                if (components.OperationTraits != null)
                {
                    foreach (var item in components.OperationTraits)
                    {
                        Walk(item.Key, () => Walk(item.Value, isComponent: true));
                    }
                }
            });
            
            Walk(AsyncApiConstants.SecuritySchemes, () =>
            {
                if (components.SecuritySchemes != null)
                {
                    foreach (var item in components.SecuritySchemes)
                    {
                        Walk(item.Key, () => Walk(item.Value, isComponent: true));
                    }
                }
            });
            
            Walk(components as IAsyncApiExtensible);
        }
        internal void Walk(AsyncApiOAuthFlows flows)
        {
            if (flows == null)
            {
                return;
            }
            
            visitor.Visit(flows);
            Walk(flows as IAsyncApiExtensible);
        }

        internal void Walk(AsyncApiOAuthFlow oAuthFlow)
        {
            if (oAuthFlow == null)
            {
                return;
            }

            visitor.Visit(oAuthFlow);
            Walk(oAuthFlow as IAsyncApiExtensible);
        }
        internal void Walk(AsyncApiSecurityScheme securityScheme, bool isComponent = false)
        {
            if (securityScheme == null || ProcessAsReference(securityScheme, isComponent))
            {
                return;
            }
            
            visitor.Visit(securityScheme);

            if (securityScheme != null)
            {
                Walk(AsyncApiConstants.Flows, () => Walk(securityScheme.Flows));
            }
            
            Walk(securityScheme as IAsyncApiExtensible);
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

            visitor.Visit(externalDocs);

            Walk(externalDocs as IAsyncApiExtensible);
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
            if (parameter == null || ProcessAsReference(parameter, isComponent))
            {
                return;
            }
            
            this.visitor.Visit(parameter);

            if (parameter != null)
            {
                this.Walk(AsyncApiConstants.Schema, () => Walk(parameter.Schema));
            }
            
            this.Walk(parameter as IAsyncApiExtensible);
        }

        internal void Walk(AsyncApiSchema schema, bool isComponent = false)
        {
            if (schema == null || ProcessAsReference(schema, isComponent))
            {
                return;
            }

            if (schemaLoop.Contains(schema))
            {
                return;  // Loop detected, this schema has already been walked.
            }
            else
            {
                schemaLoop.Push(schema);
            }

            visitor.Visit(schema);

            if (schema.Items != null)
            {
                Walk("items", () => Walk(schema.Items));
            }

            if (schema.Default != null)
            {
                Walk(AsyncApiConstants.Default, () => Walk(schema.Default));
            }

            if (schema.AllOf != null)
            {
                foreach (var item in schema.AllOf)
                {
                    Walk("allOf", () => Walk(item));
                }
            }

            if (schema.AnyOf != null)
            {
                foreach (var item in schema.AnyOf)
                {
                    Walk("anyOf", () => Walk(item));
                }
            }

            if (schema.Not != null)
            {
                Walk("not", () => Walk(schema.Not));
            }
            if (schema.Contains != null)
            {
                Walk("contains", () => Walk(schema.Contains));
            }
            if (schema.If != null)
            {
                Walk("if", () => Walk(schema.If));
            }
            if (schema.Then != null)
            {
                Walk("then", () => Walk(schema.Then));
            }
            if (schema.Else != null)
            {
                Walk("else", () => Walk(schema.Else));
            }
            if (schema.OneOf != null)
            {
                foreach (var item in schema.OneOf)
                {
                    Walk("oneOf", () => Walk(item));
                }
            }

            if (schema.Properties != null)
            {
                Walk("properties", () =>
                {
                    foreach (var item in schema.Properties)
                    {
                        Walk(item.Key, () => Walk(item.Value));
                    }
                });
            }

            if (schema.AdditionalProperties != null)
            {
                Walk("additionalProperties", () => Walk(schema.AdditionalProperties));
            }

            if (schema.PropertyNames != null)
            {
                Walk("propertyNames", () => Walk(schema.PropertyNames));
            }

            if (schema.Enum != null)
            {
                foreach (var item in schema.Enum)
                {
                    Walk("enum", () => Walk(item));
                }
            }

            if (schema.Examples != null)
            {
                foreach (var item in schema.Examples)
                {
                    Walk("examples", () => Walk(item));
                }
            }

            if (schema.Const != null)
            {
                Walk("const", () => Walk(schema.Const));
            }

            Walk(AsyncApiConstants.ExternalDocs, () => Walk(schema.ExternalDocs));

            Walk(schema as IAsyncApiExtensible);

            schemaLoop.Pop();
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
                Walk(AsyncApiConstants.Payload, () => Walk(message.Payload));
                Walk(AsyncApiConstants.CorrelationId, () => Walk(message.CorrelationId));
                Walk(AsyncApiConstants.Tags, () => Walk(message.Tags));
                Walk(AsyncApiConstants.Examples, () => Walk(message.Examples));
                Walk(AsyncApiConstants.ExternalDocs, () => Walk(message.ExternalDocs));
                Walk(AsyncApiConstants.Traits, () => Walk(message.Traits));
                // TODO: Figure out bindings.
            }

            this.Walk(message as IAsyncApiExtensible);
        }
        
        private void Walk(IList<AsyncApiMessageTrait> traits)
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

        internal void Walk(AsyncApiMessageTrait trait, bool isComponent = false)
        {
            if (trait == null || ProcessAsReference(trait, isComponent))
            {
                return;
            }

            visitor.Visit(trait);

            if (trait != null)
            {
                Walk(AsyncApiConstants.Headers, () => Walk(trait.Headers));
                Walk(AsyncApiConstants.CorrelationId, () => Walk(trait.CorrelationId));
                Walk(AsyncApiConstants.Tags, () => Walk(trait.Tags));
                Walk(AsyncApiConstants.Examples, () => Walk(trait.Examples));
                Walk(AsyncApiConstants.ExternalDocs, () => Walk(trait.ExternalDocs));
                // Todo: Figure out bindings;
            }
            
            this.Walk(trait as IAsyncApiExtensible);
        }

        internal void Walk(IList<AsyncApiMessageExample> examples)
        {
            if (examples == null)
            {
                return;
            }

            visitor.Visit(examples);

            if (examples != null)
            {
                for (int i = 0; i < examples.Count; i++)
                {
                    Walk(i.ToString(), () => Walk(examples[i]));
                }
            }
        }

        internal void Walk(AsyncApiMessageExample example)
        {
            if (example == null)
            {
                return;
            }
            
            visitor.Visit(example);

            if (example != null)
            {
                Walk(AsyncApiConstants.Headers, () => Walk(example.Headers));
                Walk(AsyncApiConstants.Payload, () => Walk(example.Payload));
            }

            this.Walk(example as IAsyncApiExtensible);
        }

        internal void Walk(IDictionary<string, IAsyncApiAny> anys)
        {
            if (anys == null)
            {
                return;
            }
            
            visitor.Visit(anys);

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
            if (correlationId == null || ProcessAsReference(correlationId, isComponent))
            {
                return;
            }

            visitor.Visit(correlationId);

            this.Walk(correlationId as IAsyncApiExtensible);
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
        
        internal void Walk(IAsyncApiExtension extension)
        {
            if (extension == null)
            {
                return;
            }

            visitor.Visit(extension);
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
                case AsyncApiDocument e: Walk(e); break;
                case AsyncApiLicense e: Walk(e); break;
                case AsyncApiInfo e: Walk(e); break;
                case AsyncApiComponents e: Walk(e); break;
                case AsyncApiContact e: Walk(e); break;
                case AsyncApiCorrelationId e: Walk(e); break;
                case AsyncApiMessageExample e: Walk(e); break;
                case AsyncApiChannel e: Walk(e); break;
                case AsyncApiExternalDocumentation e: Walk(e); break;
                case AsyncApiMessage e: Walk(e); break;
                case AsyncApiMessageTrait e: Walk(e); break;
                case AsyncApiOperationTrait e: Walk(e); break;
                case AsyncApiOAuthFlows e: Walk(e); break;
                case AsyncApiOAuthFlow e: Walk(e); break;
                case AsyncApiOperation e: Walk(e); break;
                case AsyncApiParameter e: Walk(e); break;
                case AsyncApiSchema e: Walk(e); break;
                case AsyncApiSecurityRequirement e: Walk(e); break;
                case AsyncApiSecurityScheme e: Walk(e); break;
                case AsyncApiServer e: Walk(e); break;
                case AsyncApiServerVariable e: Walk(e); break;
                case AsyncApiTag e: Walk(e); break;
                case IList<AsyncApiTag> e: Walk(e); break; 
                case IDictionary<string, AsyncApiServerVariable> e: Walk(e);
                    break;
                case IAsyncApiExtensible e: Walk(e); break;
                case IAsyncApiExtension e: Walk(e); break;
            }
        }
    }
}
