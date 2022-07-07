using System;
using LEGO.AsyncAPI.Models;
using Xunit;

namespace LEGO.AsyncAPI.Tests.Models
{
    public class AsyncApiMessageExample_Should
    {
        [Fact]
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
