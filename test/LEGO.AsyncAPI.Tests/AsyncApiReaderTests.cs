// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json.Nodes;
    using LEGO.AsyncAPI.Exceptions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Models.Interfaces;
    using LEGO.AsyncAPI.Readers;
    using NUnit.Framework;

    public class AsyncApiReaderTests
    {
        [Test]
        public void Read_WithMissingEverything_DeserializesWithErrors()
        {
            var yaml = "asyncapi: 2.6.0";
            var reader = new AsyncApiStringReader();
            var doc = reader.Read(yaml, out var diagnostic);
        }

        [Test]
        public void Read_WithExtensionParser_Parses()
        {
            var extensionName = "x-someValue";

            var yaml =
              $"""
               asyncapi: 2.3.0
               info:
                 title: test
                 version: 1.0.0
                 contact:
                   name: API Support
                   url: https://www.example.com/support
                   email: support@example.com
               channels:
                 workspace:
                   {extensionName}: onetwothreefour
               """;

            Func<AsyncApiAny, IAsyncApiExtension> valueExtensionParser = (any) =>
            {
                if (any.TryGetValue<string>(out var value))
                {
                    if (value == "onetwothreefour")
                    {
                        return new AsyncApiAny(1234);
                    }
                }

                return new AsyncApiAny("No value provided");
            };

            var settings = new AsyncApiReaderSettings
            {
                ExtensionParsers = new Dictionary<string, Func<AsyncApiAny, IAsyncApiExtension>>
                {
                    { extensionName, valueExtensionParser },
                },
            };

            var reader = new AsyncApiStringReader(settings);
            var doc = reader.Read(yaml, out var diagnostic);
            Assert.AreEqual((doc.Channels["workspace"].Extensions[extensionName] as AsyncApiAny).GetValue<int>(), 1234);
        }

        [Test]
        public void Read_WithThrowingExtensionParser_AddsToDiagnostics()
        {
            var extensionName = "x-fail";
            var yaml = $"""
                        asyncapi: 2.3.0
                        info:
                          title: test
                          version: 1.0.0
                          contact:
                            name: API Support
                            url: https://www.example.com/support
                            email: support@example.com
                        channels:
                          workspace:
                            {extensionName}: onetwothreefour

                        """;
            Func<AsyncApiAny, IAsyncApiExtension> failingExtensionParser = (any) =>
            {
                throw new AsyncApiException("Failed to parse");
            };

            var settings = new AsyncApiReaderSettings
            {
                ExtensionParsers = new Dictionary<string, Func<AsyncApiAny, IAsyncApiExtension>>
                {
                    { extensionName, failingExtensionParser },
                },
            };

            var reader = new AsyncApiStringReader(settings);
            var doc = reader.Read(yaml, out var diagnostic);

            Assert.IsNotEmpty(diagnostic.Errors);

            var error = diagnostic.Errors.First();
            Assert.AreEqual("#/channels/workspace/x-fail", error.Pointer);
            Assert.AreEqual("Failed to parse", error.Message);
        }

      [Test]
      public void Read_WithBasicPlusContact_Deserializes()
      {
        var yaml =
          """
          asyncapi: 2.3.0
          info:
            title: test
            version: 1.0.0
            contact:
              name: API Support
              url: https://www.example.com/support
              email: support@example.com
          channels:
            workspace:
              x-eventarchetype: objectchanged
          """;

        var reader = new AsyncApiStringReader();
        var doc = reader.Read(yaml, out var diagnostic);
        Assert.AreEqual("support@example.com", doc.Info.Contact.Email);
        Assert.AreEqual(new Uri("https://www.example.com/support"), doc.Info.Contact.Url);
        Assert.AreEqual("API Support", doc.Info.Contact.Name);
      }

      [Test]
      public void Read_WithBasicPlusExternalDocs_Deserializes()
      {
        var yaml =
          """
          asyncapi: 2.3.0
          info:
            title: test
            version: 1.0.0
          channels:
            workspace:
              publish:
                bindings:
                  http:
                    type: response
                message:
                  $ref: '#/components/messages/WorkspaceEventPayload'
          components:
            messages:
              WorkspaceEventPayload:
                schemaFormat: application/schema+yaml;version=draft-07
                externalDocs:
                  description: Find more info here
                  url: https://example.com
          """;

        var reader = new AsyncApiStringReader();
        var doc = reader.Read(yaml, out var diagnostic);
        var message = doc.Channels["workspace"].Publish.Message;
        Assert.AreEqual(new Uri("https://example.com"), message.First().ExternalDocs.Url);
        Assert.AreEqual("Find more info here", message.First().ExternalDocs.Description);
      }

      [Test]
      public void Read_WithBasicPlusTag_Deserializes()
      {
        var yaml =
          """
          asyncapi: 2.3.0
          info:
            title: test
            version: 1.0.0
          channels:
            workspace:
              x-eventarchetype: objectchanged
          tags:
            - name: user
              description: User-related messages
          """;

        var reader = new AsyncApiStringReader();
        var doc = reader.Read(yaml, out var diagnostic);
        var tag = doc.Tags.First();
        Assert.AreEqual("user", tag.Name);
        Assert.AreEqual("User-related messages", tag.Description);
      }

      [Test]
      public void Read_WithBasicPlusServerDeserializes()
      {
        var yaml =
          """
          asyncapi: 2.3.0
          info:
            title: test
            version: 1.0.0
          channels:
            workspace:
              x-eventarchetype: objectchanged
          servers:
            production:
              url: 'pulsar+ssl://prod.events.managed.io:1234'
              protocol: pulsar+ssl
              description: Pulsar broker
          """;

        var reader = new AsyncApiStringReader();
        var doc = reader.Read(yaml, out var diagnostic);
        var server = doc.Servers.First();
        Assert.AreEqual("production", server.Key);
        Assert.AreEqual("pulsar+ssl://prod.events.managed.io:1234", server.Value.Url);
        Assert.AreEqual("pulsar+ssl", server.Value.Protocol);
        Assert.AreEqual("Pulsar broker", server.Value.Description);
      }

      [Test]
      public void Read_WithBasicPlusServerVariablesDeserializes()
      {
        var yaml =
          """
          asyncapi: 2.3.0
          info:
            title: test
            version: 1.0.0
          channels:
            workspace:
              x-eventarchetype: objectchanged
          servers:
            production:
              url: 'pulsar+ssl://prod.events.managed.io:{port}'
              protocol: pulsar+ssl
              description: Pulsar broker
              variables:
                port:
                  description: Secure connection (TLS) is available through port 8883.
                  default: '1883'
                  enum:
                    - '1883'
                    - '8883'
          """;

        var reader = new AsyncApiStringReader();
        var doc = reader.Read(yaml, out var diagnostic);
        var server = doc.Servers.First();
        var variable = server.Value.Variables.First();
        Assert.AreEqual("production", server.Key);
        Assert.AreEqual("port", variable.Key);
        Assert.AreEqual("Secure connection (TLS) is available through port 8883.", variable.Value.Description);
      }

      [Test]
      public void Read_WithBasicPlusCorrelationIDDeserializes()
      {
        var yaml =
          """
          asyncapi: 2.3.0
          info:
            title: test
            version: 1.0.0
          channels:
            workspace:
              publish:
                bindings:
                  http:
                    type: response
                message:
                  $ref: '#/components/messages/WorkspaceEventPayload'
          components:
            messages:
              WorkspaceEventPayload:
                schemaFormat: application/schema+yaml;version=draft-07
                correlationId:
                  description: Default Correlation ID
                  location: $message.header#/correlationId
          """;

        var reader = new AsyncApiStringReader();
        var doc = reader.Read(yaml, out var diagnostic);
        var message = doc.Channels["workspace"].Publish.Message;
        Assert.AreEqual("Default Correlation ID", message.First().CorrelationId.Description);
        Assert.AreEqual("$message.header#/correlationId", message.First().CorrelationId.Location);
      }

        [Test]
        public void Read_WithOneOfMessage_Reads()
        {
            var yaml = """
                       asyncapi: 2.3.0
                       info:
                         title: test
                         version: 1.0.0
                       channels:
                         workspace:
                           publish:
                             bindings:
                               http:
                                 type: response
                             message:
                               oneOf:
                                   - $ref: '#/components/messages/WorkspaceEventPayload'
                       components:
                         messages:
                           WorkspaceEventPayload:
                             schemaFormat: application/schema+yaml;version=draft-07
                             correlationId:
                               description: Default Correlation ID
                               location: $message.header#/correlationId
                               
                       """;

            var reader = new AsyncApiStringReader();
            var doc = reader.Read(yaml, out var diagnostic);
            var message = doc.Channels["workspace"].Publish.Message.First();
            Assert.AreEqual("Default Correlation ID", message.CorrelationId.Description);
            Assert.AreEqual("$message.header#/correlationId", message.CorrelationId.Location);
        }

        [Test]
      public void Read_WithBasicPlusSecuritySchemeDeserializes()
      {
        var yaml = """
                   asyncapi: 2.3.0
                   info:
                     title: test
                     version: 1.0.0
                   channels:
                     workspace:
                       publish:
                         bindings:
                           http:
                             type: response
                         message:
                           $ref: '#/components/messages/WorkspaceEventPayload'
                   components:
                     messages:
                       WorkspaceEventPayload:
                         schemaFormat: application/schema+yaml;version=draft-07
                     securitySchemes:
                       saslScram:
                         type: scramSha256
                         description: Provide your username and password for SASL/SCRAM authentication

                   """;
        var reader = new AsyncApiStringReader();
        var doc = reader.Read(yaml, out var diagnostic);
        var scheme = doc.Components.SecuritySchemes.First();
        Assert.AreEqual("saslScram", scheme.Key);
        Assert.AreEqual(SecuritySchemeType.ScramSha256, scheme.Value.Type);
        Assert.AreEqual("Provide your username and password for SASL/SCRAM authentication", scheme.Value.Description);
      }

      [Test]
      public void Read_WithBasicPlusOAuthFlowDeserializes()
      {
        var yaml = """
                   asyncapi: 2.3.0
                   info:
                     title: test
                     version: 1.0.0
                   channels:
                     workspace:
                       x-something: yes
                   components:
                     securitySchemes:
                       oauth2:
                         type: oauth2
                         flows:
                           implicit:
                             authorizationUrl: https://example.com/api/oauth/dialog
                             scopes:
                               write:pets: modify pets in your account
                               read:pets: read your pets

                   """;
        var reader = new AsyncApiStringReader();
        var doc = reader.Read(yaml, out var diagnostic);
        var scheme = doc.Components.SecuritySchemes.First();
        var flow = scheme.Value.Flows;
        Assert.AreEqual("oauth2", scheme.Key);
        Assert.AreEqual(SecuritySchemeType.OAuth2, scheme.Value.Type);
        Assert.AreEqual(new Uri("https://example.com/api/oauth/dialog"), flow.Implicit.AuthorizationUrl);
        Assert.IsTrue(flow.Implicit.Scopes.ContainsKey("write:pets"));
        Assert.IsTrue(flow.Implicit.Scopes.ContainsKey("read:pets"));
      }

        [Test]
        public void Read_WithServerReference_ResolvesReference()
        {
            var yaml = """
                       asyncapi: 2.3.0
                       info:
                         title: test
                         version: 1.0.0
                       servers:
                         production:
                           $ref: '#/components/servers/production'
                       channels:
                         workspace:
                           x-something: yes
                       components:
                         servers:
                           production:
                               url: 'pulsar+ssl://prod.events.managed.io:1234'
                               protocol: pulsar+ssl
                               description: Pulsar broker
                               security:
                                 - petstore_auth:
                                   - write:pets
                                   - read:pets
                         securitySchemes:
                           petstore_auth:
                             type: oauth2
                             flows:
                               implicit:
                                 authorizationUrl: https://example.com/api/oauth/dialog
                                 scopes:
                                   write:pets: modify pets in your account
                                   read:pets: read your pets

                       """;
            var reader = new AsyncApiStringReader();
            var doc = reader.Read(yaml, out var diagnostic);
            Assert.AreEqual("pulsar+ssl://prod.events.managed.io:1234", doc.Servers.First().Value.Url);
        }

        [Test]
        public void Read_WithChannelReference_ResolvesReference()
        {
            var yaml = """
                       asyncapi: 2.3.0
                       info:
                         title: test
                         version: 1.0.0
                       servers:
                         production:
                           $ref: '#/components/servers/production'
                       channels:
                         workspace:
                           $ref: '#/components/channels/workspace'
                       components:
                         channels:
                           workspace:
                               publish:
                                 message:
                                   $ref: '#/components/messages/WorkspaceEventPayload'
                         servers:
                           production:
                               url: 'pulsar+ssl://prod.events.managed.io:1234'
                               protocol: pulsar+ssl
                               description: Pulsar broker
                               security:
                                 - petstore_auth:
                                   - write:pets
                                   - read:pets
                         messages:
                           WorkspaceEventPayload:
                             schemaFormat: 'application/schema+yaml;version=draft-07'
                         securitySchemes:
                           petstore_auth:
                             type: oauth2
                             flows:
                               implicit:
                                 authorizationUrl: https://example.com/api/oauth/dialog
                                 scopes:
                                   write:pets: modify pets in your account
                                   read:pets: read your pets

                       """;
            var reader = new AsyncApiStringReader();
            var doc = reader.Read(yaml, out var diagnostic);
            Assert.AreEqual("application/schema+yaml;version=draft-07", doc.Channels.First().Value.Publish.Message.First().SchemaFormat);
        }

        [Test]
        public void Read_WithBasicPlusMessageTraitsDeserializes()
        {
            var yaml = """
                       asyncapi: 2.3.0
                       info:
                         title: test
                         version: 1.0.0
                       channels:
                         workspace:
                           publish:
                             bindings:
                               http:
                                 type: response
                             message:
                               $ref: '#/components/messages/WorkspaceEventPayload'
                       components:
                         messages:
                           WorkspaceEventPayload:
                             schemaFormat: application/schema+yaml;version=draft-07
                             externalDocs:
                               description: Find more info here
                               url: https://example.com
                             traits:
                               - $ref: '#/components/messageTraits/commonHeaders'
                         messageTraits:
                           commonHeaders:
                             description: a common headers for common things
                             headers:
                               type: object
                               properties:
                                 my-app-header:
                                   type: integer
                                   minimum: 0
                                   maximum: 100

                       """;
            var reader = new AsyncApiStringReader();
            var doc = reader.Read(yaml, out var diagnostic);

            Assert.AreEqual(1, doc.Channels.First().Value.Publish.Message.First().Traits.Count);
            Assert.AreEqual("a common headers for common things", doc.Channels.First().Value.Publish.Message.First().Traits.First().Description);
        }

        /// <summary>
        /// Regression test.
        /// Bug: Serializing properties multiple times - specifically Schema.OneOf was serialized into OneOf and Then.
        /// </summary>
        [Test]
        public void Serialize_withOneOfSchema_DoesNotWriteThen()
        {
            var yaml = """
                       asyncapi: 2.3.0
                       info:
                         title: test
                         version: 1.0.0
                       defaultContentType: application/json
                       channels:
                         channel1:
                           publish:
                             operationId: channel1
                             summary: tthe first channel
                             description: a channel of great importance
                             message:
                               $ref: '#/components/messages/item1'
                       components:
                         schemas:
                           item2:
                             type: object
                             properties:
                               icon:
                                 description: Theme icon
                                 oneOf:
                                 - type: 'null'
                                 - $ref: '#/components/schemas/item3'
                           item3:
                             type: object
                             properties:
                               title:
                                 type: string
                                 description: The title.
                                 format: string
                         messages:
                           item1:
                             payload:
                               $ref: '#/components/schemas/item2'
                             name: item1
                             title: item 1
                             summary: the first item
                             description: a first item for firsting the items
                       """;

            var reader = new AsyncApiStringReader();
            var doc = reader.Read(yaml, out var diagnostic);

            var yamlAgain = doc.Serialize(AsyncApiVersion.AsyncApi2_0, AsyncApiFormat.Yaml);
            Assert.True(!yamlAgain.Contains("then:"));
        }

        [Test]
        public void Read_WithBasicPlusSecurityRequirementsDeserializes()
        {
            var yaml = """
                       asyncapi: 2.3.0
                       info:
                         title: test
                         version: 1.0.0
                       servers:
                         production:
                           url: 'pulsar+ssl://prod.events.managed.io:1234'
                           protocol: pulsar+ssl
                           description: Pulsar broker
                           security:
                             - petstore_auth:
                               - write:pets
                               - read:pets
                       channels:
                         workspace:
                           x-something: yes
                       components:
                         securitySchemes:
                           petstore_auth:
                             type: oauth2
                             flows:
                               implicit:
                                 authorizationUrl: https://example.com/api/oauth/dialog
                                 scopes:
                                   write:pets: modify pets in your account
                                   read:pets: read your pets

                       """;
            var reader = new AsyncApiStringReader();
            var doc = reader.Read(yaml, out var diagnostic);
            var requirement = doc.Servers.First().Value.Security.First().First();
            Assert.AreEqual(SecuritySchemeType.OAuth2, requirement.Key.Type);
            Assert.IsTrue(requirement.Value.Contains("write:pets"));
            Assert.IsTrue(requirement.Value.Contains("read:pets"));
      }
    }
}
