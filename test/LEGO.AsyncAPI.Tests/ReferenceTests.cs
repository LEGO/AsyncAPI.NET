// Copyright (c) The LEGO Group. All rights reserved.
namespace LEGO.AsyncAPI.Tests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using FluentAssertions;
    using LEGO.AsyncAPI.Models;
    using LEGO.AsyncAPI.Readers;
    using LEGO.AsyncAPI.Readers.V2;
    using NUnit.Framework;

    public class ReferenceTests
    {
        [Test]
        public void ReferencePointers()
        {
            var diag = new AsyncApiDiagnostic();
            var versionService = new AsyncApiV2VersionService(diag);
            var externalFragment = versionService.ConvertToAsyncApiReference("https://github.com/test/test#whatever", ReferenceType.None);
            var internalFragment = versionService.ConvertToAsyncApiReference("#/components/servers/server1", ReferenceType.None);
            var localFile = versionService.ConvertToAsyncApiReference("./local/some/folder/whatever.yaml", ReferenceType.None);
            var externalFile = versionService.ConvertToAsyncApiReference("https://github.com/test/whatever.yaml", ReferenceType.None);

            externalFragment.ExternalResource.Should().Be("https://github.com/test/test");
            externalFragment.Id.Should().Be("whatever");
            externalFragment.IsFragment.Should().BeTrue();
            externalFragment.IsExternal.Should().BeTrue();

            internalFragment.ExternalResource.Should().Be(null);
            internalFragment.Id.Should().Be("#/components/servers/server1");
            internalFragment.IsFragment.Should().BeTrue();
            internalFragment.IsExternal.Should().BeFalse();

            localFile.ExternalResource.Should().Be("./local/some/folder/whatever.yaml");
            localFile.Id.Should().Be(null);
            localFile.IsFragment.Should().BeFalse();

            externalFile.ExternalResource.Should().Be("https://github.com/test/whatever.yaml");
            externalFile.Id.Should().Be(null);
            externalFile.IsFragment.Should().BeFalse();
        }

        [Test]
        public void Reference()
        {
            var json =
                """
                {
                  "asyncapi": "2.6.0",
                  "info": { },
                  "servers": {
                    "production": {
                      "$ref": "https://github.com/test/test#whatever"
                    }
                  }
                }
                """;

            var doc = new AsyncApiStringReader().Read(json, out var diag);
            var reference = doc.Servers.First().Value as AsyncApiServerReference;
            reference.Reference.ExternalResource.Should().Be("https://github.com/test/test");
            reference.Reference.Id.Should().Be("whatever");
            reference.Reference.HostDocument.Should().Be(doc);
            reference.Reference.IsFragment.Should().BeTrue();

        }
    } 
}