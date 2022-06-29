// <copyright file="AsyncApiReaderTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Writers;

namespace LEGO.AsyncAPI.Tests
{
    using System;
    using System.Linq;
    using LEGO.AsyncAPI.Models.Any;
    using NUnit.Framework;
    using LEGO.AsyncAPI.Readers;

    public class AsyncApiReaderTests
         {
      [Test]
      public void Read_WithFullSpec_Deserializes()
      {
        var yaml = @"asyncapi: 2.3.0
info:
  title: AMMA
  version: 1.0.0
  x-audience: component-internal
  x-application-id: APP-12345
  description: |
    Sending AMMA metadata events to the topic.
  license:
    name: Apache 2.0
    url: 'https://www.apache.org/licenses/LICENSE-2.0'
servers:
  production:
    url: 'pulsar+ssl://prod.events.managed.async.api.legogroup.io:6651'
    protocol: pulsar+ssl
    description: Pulsar broker
channels:
  workspace:
    x-eventarchetype: objectchanged
    x-classification: green
    x-datalakesubscription: true
    publish:
      bindings:
        http:
          type: response
      message:
        $ref: '#/components/messages/WorkspaceEventPayload'
  api:
    x-eventarchetype: objectchanged
    x-classification: green
    x-datalakesubscription: true
    publish:
      bindings:
        http:
          type: response
      message:
        $ref: '#/components/messages/APIEventPayload'
components:
  messages:
    WorkspaceEventPayload:
      schemaFormat: application/schema+yaml;version=draft-07
      summary: Metadata about a workspace that has been created, updated or deleted.
      payload:
        type: object
        properties:
          key:
            type: string
            description: Key of the message.
          event:
            type: string
            description: Event type.
          payload:
            type: object
            properties:
              workspace:
                type: string
                description: Name of the workspace.
              href:
                type: string
                description: Send an API request to this url for detailed data on the referenced workspace.
                
    APIEventPayload:
      schemaFormat: application/schema+yaml;version=draft-07
      summary: Metadata about an API that has been created, updated or deleted.
      payload:
        type: object
        properties:
          key:
            type: string
            description: Key of the message.
          event:
            type: string
            description: Event type.
          payload:
            type: object
            properties:
              workspace:
                type: string
                description: Name of the workspace.
              api:
                type: string
                description: Name of the API.
              href:
                type: string
                description: Send an API request to this url for detailed data on the referenced API.
";
        var reader = new AsyncApiStringReader();
        var doc = reader.Read(yaml, out var diagnostic);
        Assert.AreEqual((doc.Channels["workspace"].Extensions["x-eventarchetype"] as AsyncApiString).Value,
          "objectchanged");
        Assert.AreEqual((doc.Channels["workspace"].Extensions["x-classification"] as AsyncApiString).Value, "green");
        Assert.AreEqual((doc.Channels["workspace"].Extensions["x-datalakesubscription"] as AsyncApiBoolean).Value,
          true);
        var message = doc.Channels["workspace"].Publish.Message;
        Assert.AreEqual(message.SchemaFormat, "application/schema+yaml;version=draft-07");
        Assert.AreEqual(message.Summary, "Metadata about a workspace that has been created, updated or deleted.");
        var payload = doc.Channels["workspace"].Publish.Message.Payload;
        Assert.NotNull(payload);
        Assert.AreEqual(typeof(AsyncApiObject), payload.GetType());
      }

      [Test]
      public void Read_WithBasicPlusContact_Deserializes()
      {
        var yaml = @"asyncapi: 2.3.0
info:
  title: AMMA
  version: 1.0.0
  contact:  
    name: API Support
    url: https://www.example.com/support
    email: support@example.com
channels:
  workspace:
    x-eventarchetype: objectchanged
";
        var reader = new AsyncApiStringReader();
        var doc = reader.Read(yaml, out var diagnostic);
        Assert.AreEqual("support@example.com", doc.Info.Contact.Email);
        Assert.AreEqual(new Uri("https://www.example.com/support"), doc.Info.Contact.Url);
        Assert.AreEqual("API Support", doc.Info.Contact.Name);
      }

      [Test]
      public void Read_WithBasicPlusExternalDocs_Deserializes()
      {
        var yaml = @"asyncapi: 2.3.0
info:
  title: AMMA
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
";
        var reader = new AsyncApiStringReader();
        var doc = reader.Read(yaml, out var diagnostic);
        var message = doc.Channels["workspace"].Publish.Message;
        Assert.AreEqual(new Uri("https://example.com"), message.ExternalDocs.Url);
        Assert.AreEqual("Find more info here", message.ExternalDocs.Description);
      }

      [Test]
      public void Read_WithBasicPlusTag_Deserializes()
      {
        var yaml = @"asyncapi: 2.3.0
info:
  title: AMMA
  version: 1.0.0
channels:
  workspace:
    x-eventarchetype: objectchanged
tags:
  - name: user
    description: User-related messages       
";
        var reader = new AsyncApiStringReader();
        var doc = reader.Read(yaml, out var diagnostic);
        var tag = doc.Tags.First();
        Assert.AreEqual("user", tag.Name);
        Assert.AreEqual("User-related messages", tag.Description);
      }

      [Test]
      public void Read_WithBasicPlusServerDeserializes()
      {
        var yaml = @"asyncapi: 2.3.0
info:
  title: AMMA
  version: 1.0.0
channels:
  workspace:
    x-eventarchetype: objectchanged
servers:
  production:
    url: 'pulsar+ssl://prod.events.managed.io:1234'
    protocol: pulsar+ssl
    description: Pulsar broker   
";
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
        var yaml = @"asyncapi: 2.3.0
info:
  title: AMMA
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
";
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
        var yaml = @"asyncapi: 2.3.0
info:
  title: AMMA
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
";
        var reader = new AsyncApiStringReader();
        var doc = reader.Read(yaml, out var diagnostic);
        var message = doc.Channels["workspace"].Publish.Message;
        Assert.AreEqual("Default Correlation ID", message.CorrelationId.Description);
        Assert.AreEqual("$message.header#/correlationId", message.CorrelationId.Location);
      }
      
      [Test]
      public void Read_WithBasicPlusSecuritySchemeDeserializes()
      {
        var yaml = @"asyncapi: 2.3.0
info:
  title: AMMA
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
";
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
        var yaml = @"asyncapi: 2.3.0
info:
  title: AMMA
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
";
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

    }
}