// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Tests.Models
{
    using System;
    using LEGO.AsyncAPI.Models;
    using NUnit.Framework;

    public class AsyncApiContact_Should
    {
        [Test]
        public void SerializeV2_WithNullWriter_Throws()
        {
            // Arrange
            var asyncApiContact = new AsyncApiContact();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => { asyncApiContact.SerializeV2(null); });
        }
    }
}
