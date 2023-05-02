// Copyright (c) The LEGO Group. All rights reserved.

using System;
using LEGO.AsyncAPI.Models;
using NUnit.Framework;

namespace LEGO.AsyncAPI.Tests.Models
{
    public class AsyncApiInfo_Should
    {
        [Test]
        public void SerializeV2_WithNullWriter_Throws()
        {
            // Arrange
            var asyncApiInfo = new AsyncApiInfo();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => { asyncApiInfo.SerializeV2(null); });
        }
    }
}
