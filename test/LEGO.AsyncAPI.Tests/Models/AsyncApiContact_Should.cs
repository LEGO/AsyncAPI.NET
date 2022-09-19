using System;
using LEGO.AsyncAPI.Models;
using NUnit.Framework;

namespace LEGO.AsyncAPI.Tests.Models
{
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
