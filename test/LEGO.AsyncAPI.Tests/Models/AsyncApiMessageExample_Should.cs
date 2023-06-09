// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests.Models
{
    using System;
    using LEGO.AsyncAPI.Models;
    using NUnit.Framework;

    public class AsyncApiMessageExample_Should
    {
        [Test]
        public void SerializeV2_WithNullWriter_Throws()
        {
            // Arrange
            var asyncApiMessageExample = new AsyncApiMessageExample();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => { asyncApiMessageExample.SerializeV2(null); });
        }
    }
}
