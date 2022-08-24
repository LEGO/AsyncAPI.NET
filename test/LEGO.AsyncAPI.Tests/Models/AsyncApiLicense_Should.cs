using System;
using LEGO.AsyncAPI.Models;
using NUnit.Framework;

namespace LEGO.AsyncAPI.Tests.Models
{
    public class AsyncApiLicense_Should
    {
        [Test]
        public void SerializeV2_WithNullWriter_Throws()
        {
            // Arrange
            var asyncApiLicense = new AsyncApiLicense();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => { asyncApiLicense.SerializeV2(null); });
        }
    }
}
