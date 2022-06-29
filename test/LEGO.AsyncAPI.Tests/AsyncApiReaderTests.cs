﻿// <copyright file="AsyncApiReaderTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace LEGO.AsyncAPI.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using Models;
    using Models.Any;
    using Models.Interfaces;
    using NUnit.Framework;
    using Readers;
    using Writers;

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
            Assert.AreEqual((doc.Channels["workspace"].Extensions["x-eventarchetype"] as AsyncApiString).Value, "objectchanged");
            Assert.AreEqual((doc.Channels["workspace"].Extensions["x-classification"] as AsyncApiString).Value, "green");
            Assert.AreEqual((doc.Channels["workspace"].Extensions["x-datalakesubscription"] as AsyncApiBoolean).Value, true);
            var message = doc.Channels["workspace"].Publish.Message;
            Assert.AreEqual(message.SchemaFormat, "application/schema+yaml;version=draft-07");
            Assert.AreEqual(message.Summary, "Metadata about a workspace that has been created, updated or deleted.");
            var payload = doc.Channels["workspace"].Publish.Message.Payload;
            Assert.NotNull(payload);
            Assert.AreEqual(typeof(AsyncApiObject), payload.GetType());
        }
    }
}