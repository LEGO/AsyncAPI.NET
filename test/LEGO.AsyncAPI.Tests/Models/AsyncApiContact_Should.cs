using System;
using LEGO.AsyncAPI.Models;
using Xunit;

namespace LEGO.AsyncAPI.Tests.Models
{
    public class AsyncApiContact_Should
    {
        [Fact]
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
