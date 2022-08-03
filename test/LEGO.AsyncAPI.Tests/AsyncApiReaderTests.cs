// <copyright file="AsyncApiReaderTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using LEGO.AsyncAPI.Models;

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
        public void Read()
        {
            var json = @"{
  'asyncapi': '2.3.0',
  'info': {
    'x-application-id': 'APP-00881',
    'x-audience': 'company-internal',
    'x-skip-infrastructure': false,
    'title': 'CatalogEventAPI',
    'version': '1.0.0',
    'description': 'API for subscribing to changes in the Catalog. The Catalog is a variaty of editorial content.',
    'contact': {
      'name': 'Content Publishing Platform',
      'url': 'https://teams.microsoft.com/l/channel/19%3a2dd4ddc220714cb09df23ff8fca99229%40thread.tacv2/General?groupId=6ea24e60-1f97-4b85-acac-6408ac51248f&tenantId=1d063515-6cad-4195-9486-ea65df456faa',
      'email': '49ad63d3.o365.corp.LEGO.com@emea.teams.ms'
    }
  },
  'defaultContentType': 'application/json',
  'channels': {
    'ThemeChangedV1': {
      'x-eventdeduplication': true,
      'x-eventarchetype': 'objectchanged',
      'x-classification': 'green',
      'x-eventdurability': 'persistent',
      'x-datalakesubscription': true,
      'publish': {
        'operationId': 'ThemeChanged',
        'summary': 'Theme changes',
        'description': 'Themes published, unpublished or deleted.',
        'message': { '$ref': '#/components/messages/ThemeV1' }
      }
    },
    'CharacterChangedV1': {
      'x-eventdeduplication': true,
      'x-eventarchetype': 'objectchanged',
      'x-classification': 'green',
      'x-eventdurability': 'persistent',
      'x-datalakesubscription': true,
      'publish': {
        'operationId': 'CharacterChanged',
        'summary': 'Character changes',
        'description': 'Characters published, unpublished or deleted.',
        'message': { '$ref': '#/components/messages/CharacterV1' }
      }
    }
  },
  'components': {
    'schemas': {
      'themeDto': {
        'type': 'object',
        'properties': {
          'id': {
            'type': 'string',
            'description': 'Unique id for the specific translations'
          },
          'title': {
            'type': 'string',
            'description': 'Display title of the theme'
          },
          'description': {
            'type': 'string',
            'description': 'Richtext description. Can be null.'
          },
          'colors': {
            'description': 'Theme colors',
            'oneOf': [
              { 'type': 'null' },
              { '$ref': '#/components/schemas/themeColorsDto' }
            ]
          },
          'icon': {
            'description': 'Theme icon',
            'oneOf': [
              { 'type': 'null' },
              { '$ref': '#/components/schemas/themeIconDto' }
            ]
          },
          'logo': {
            'description': 'Theme Logo',
            'oneOf': [
              { 'type': 'null' },
              { '$ref': '#/components/schemas/imageDto' }
            ]
          },
          'bannerStandard': {
            'description': 'Banner standard',
            'oneOf': [
              { 'type': 'null' },
              { '$ref': '#/components/schemas/imageDto' }
            ]
          },
          'bannerWide': {
            'description': 'Benner wide',
            'oneOf': [
              { 'type': 'null' },
              { '$ref': '#/components/schemas/imageDto' }
            ]
          },
          'parentThemeId': {
            'type': 'string',
            'description': 'For sub-themes this property will contain the Id of the parent theme. Can be null.'
          },
          'parentImages': {
            'description': 'Images only available on parent themes',
            'oneOf': [
              { 'type': 'null' },
              { '$ref': '#/components/schemas/parentImagesDto' }
            ]
          },
          'invariantId': {
            'type': 'string',
            'description': 'Unique id across translations',
            'format': 'guid'
          },
          'contentLocale': {
            'type': 'string',
            'description': 'Localized language'
          },
          'marketLocale': {
            'type': 'string',
            'description': 'Published market'
          },
          'version': {
            'type': 'integer',
            'description': 'Published version',
            'format': 'int32'
          },
          'publishedAt': {
            'type': 'string',
            'description': 'Published at',
            'format': 'date-time'
          }
        }
      },
      'themeColorsDto': {
        'type': 'object',
        'properties': {
          'primary': {
            'description': 'Primary theme color',
            'oneOf': [
              { 'type': 'null' },
              { '$ref': '#/components/schemas/colorDto' }
            ]
          },
          'secondary': {
            'description': 'Secondary theme color',
            'oneOf': [
              { 'type': 'null' },
              { '$ref': '#/components/schemas/colorDto' }
            ]
          },
          'accentLight': {
            'description': 'Accent light color',
            'oneOf': [
              { 'type': 'null' },
              { '$ref': '#/components/schemas/colorDto' }
            ]
          },
          'accentDark': {
            'description': 'Accent dark color',
            'oneOf': [
              { 'type': 'null' },
              { '$ref': '#/components/schemas/colorDto' }
            ]
          }
        }
      },
      'colorDto': {
        'type': 'object',
        'properties': {
          'red': {
            'type': 'integer',
            'format': 'int32'
          },
          'green': {
            'type': 'integer',
            'format': 'int32'
          },
          'blue': {
            'type': 'integer',
            'format': 'int32'
          }
        }
      },
      'themeIconDto': {
        'type': 'object',
        'properties': {
          'themeLogoUrl': {
            'type': 'string',
            'description': 'Theme logo. Can be null.',
            'format': 'uri'
          },
          'signatureCharacterUrl': {
            'type': 'string',
            'description': 'Signature character. Can be null.',
            'format': 'uri'
          },
          'altText': {
            'type': 'string',
            'description': 'Alternate text for the icon. Can be null.'
          }
        }
      },
      'imageDto': {
        'type': 'object',
        'properties': {
          'url': {
            'type': 'string',
            'description': 'Absolute url to image',
            'format': 'uri'
          },
          'altText': {
            'type': 'string',
            'description': 'Image alternate text'
          }
        }
      },
      'parentImagesDto': {
        'type': 'object',
        'properties': {
          'logoPortrait': {
            'description': 'Logo portrait image',
            'oneOf': [
              { 'type': 'null' },
              { '$ref': '#/components/schemas/imageDto' }
            ]
          },
          'background': {
            'description': 'Background image',
            'oneOf': [
              { 'type': 'null' },
              { '$ref': '#/components/schemas/imageDto' }
            ]
          },
          'border': {
            'description': 'Border image',
            'oneOf': [
              { 'type': 'null' },
              { '$ref': '#/components/schemas/imageDto' }
            ]
          },
          'banner': {
            'description': 'Banner image',
            'oneOf': [
              { 'type': 'null' },
              { '$ref': '#/components/schemas/imageDto' }
            ]
          },
          'button': {
            'description': 'Button image',
            'oneOf': [
              { 'type': 'null' },
              { '$ref': '#/components/schemas/imageDto' }
            ]
          },
          'playlist': {
            'description': 'Playlist image',
            'oneOf': [
              { 'type': 'null' },
              { '$ref': '#/components/schemas/imageDto' }
            ]
          }
        }
      },
      'characterDto': {
        'type': 'object',
        'description': 'A person in a product or movie. Typically related to a mini-fig.',
        'properties': {
          'name': { 'type': 'string' },
          'descriptionKids': { 'type': 'string' },
          'descriptionGrownups': { 'type': 'string' },
          'mugshot': {
            'oneOf': [
              { 'type': 'null' },
              { '$ref': '#/components/schemas/imageDto' }
            ]
          },
          'landscapePose': {
            'oneOf': [
              { 'type': 'null' },
              { '$ref': '#/components/schemas/imageDto' }
            ]
          },
          'squareFullBody': {
            'oneOf': [
              { 'type': 'null' },
              { '$ref': '#/components/schemas/imageDto' }
            ]
          },
          'threeDSpin': {
            'oneOf': [
              { 'type': 'null' },
              { '$ref': '#/components/schemas/imageDto' }
            ]
          },
          'themes': {
            'type': 'array',
            'description': 'List of themes IDs',
            'items': { 'type': 'string' }
          },
          'channels': {
            'type': 'array',
            'description': 'List of target channels names',
            'items': { '$ref': '#/components/schemas/channelDto' }
          },
          'audiences': {
            'type': 'array',
            'description': 'List of target audiences names',
            'items': { '$ref': '#/components/schemas/audienceDto' }
          },
          'animationVideoId': {
            'type': 'string',
            'description': 'Id of the video animations of this character'
          },
          'id': {
            'type': 'string',
            'description': 'Unique id for the specific translations'
          },
          'invariantId': {
            'type': 'string',
            'description': 'Unique id across translations',
            'format': 'guid'
          },
          'contentLocale': {
            'type': 'string',
            'description': 'Localized language'
          },
          'marketLocale': {
            'type': 'string',
            'description': 'Published market'
          },
          'version': {
            'type': 'integer',
            'description': 'Published version',
            'format': 'int32'
          },
          'publishedAt': {
            'type': 'string',
            'description': 'Published at',
            'format': 'date-time'
          }
        }
      },
      'channelDto': {
        'type': 'object',
        'properties': {
          'id': { 'type': 'string' },
          'title': { 'type': 'string' }
        }
      },
      'audienceDto': {
        'type': 'object',
        'properties': {
          'id': { 'type': 'string' },
          'title': { 'type': 'string' }
        }
      }
    },
    'messages': {
      'ThemeV1': {
        'payload': { '$ref': '#/components/schemas/themeDto' },
        'name': 'ThemeV1',
        'title': 'Theme V1',
        'summary': 'Marked specific theme',
        'description': 'A theme data object containing title, description, colors and images. A theme is translated to a variaty of languages.'
      },
      'CharacterV1': {
        'payload': { '$ref': '#/components/schemas/characterDto' },
        'name': 'CharacterV1',
        'title': 'Character V1',
        'summary': 'Marked specific character',
        'description': 'A character data object containing title, description and images. The character is translated to a variaty of languages.'
      }
    }
  },
  'externalDocs': {
    'description': 'Product handbook',
    'url': 'https://platform.pages.git42.corp.lego.com/handbook/docs/products/catalog/'
  }
}";
            var reader = new AsyncApiStringReader();
            var doc = reader.Read(json, out var diagnostic);
        }
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

        [Test]
        public void Read_WithServerReference_ResolvesReference()
        {
            var yaml = @"asyncapi: 2.3.0
info:
  title: AMMA
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
";
            var reader = new AsyncApiStringReader();
            var doc = reader.Read(yaml, out var diagnostic);
            Assert.AreEqual("pulsar+ssl://prod.events.managed.io:1234", doc.Servers.First().Value.Url);

        }

        [Test]
        public void Read_WithChannelReference_ResolvesReference()
        {
            var yaml = @"asyncapi: 2.3.0
info:
  title: AMMA
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
";
            var reader = new AsyncApiStringReader();
            var doc = reader.Read(yaml, out var diagnostic);
            Assert.AreEqual("application/schema+yaml;version=draft-07", doc.Channels.First().Value.Publish.Message.SchemaFormat);

        }

        [Test]
        public void Read_WithBasicPlusMessageTraitsDeserializes()
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
";
            var reader = new AsyncApiStringReader();
            var doc = reader.Read(yaml, out var diagnostic);

            Assert.AreEqual(1, doc.Channels.First().Value.Publish.Message.Traits.Count);
            Assert.AreEqual("a common headers for common things", doc.Channels.First().Value.Publish.Message.Traits.First().Description);
        }
        /// <summary>
        /// Regression test.
        /// Bug: Serializing properties multiple times - specifically Schema.OneOf was serialized into OneOf and Then.
        /// </summary>
        [Test]
        public void Serialize_withOneOfSchema_DoesNotWriteThen()
        {
            var yaml = @"asyncapi: 2.3.0
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
      description: a first item for firsting the items";

            var reader = new AsyncApiStringReader();
            var doc = reader.Read(yaml, out var diagnostic);

            var yamlAgain = doc.Serialize(AsyncApiFormat.Yaml);
            Assert.True(!yamlAgain.Contains("then:"));
        }
        
      [Test]
      public void Read_WithBasicPlusSecurityRequirementsDeserializes()
      {
        var yaml = @"asyncapi: 2.3.0
info:
  title: AMMA
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
";
        var reader = new AsyncApiStringReader();
        var doc = reader.Read(yaml, out var diagnostic);
        var requirement = doc.Servers.First().Value.Security.First().First();
        Assert.AreEqual(SecuritySchemeType.OAuth2, requirement.Key.Type);
        Assert.IsTrue(requirement.Value.Contains("write:pets"));
        Assert.IsTrue(requirement.Value.Contains("read:pets"));
      }
    }
}
