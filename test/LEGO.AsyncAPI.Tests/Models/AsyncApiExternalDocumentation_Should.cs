// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests.Models
{
    using System;
    using LEGO.AsyncAPI.Models;
    using NUnit.Framework;

    public class AsyncApiExternalDocumentation_Should
    {
        [Test]
        public void SerializeV2_WithNullWriter_Throws()
        {
            // Arrange
            var asyncApiExternalDocumentation = new AsyncApiExternalDocumentation();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => { asyncApiExternalDocumentation.SerializeV2(null); });
        }
    }
}
