// Copyright (c) The LEGO Group. All rights reserved.

using LEGO.AsyncAPI.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace LEGO.AsyncAPI.Tests
{
    public class AsyncApiAnyTests
    {
        [Test]
        public void GetValue_ReturnsCorrectConversions()
        {
            // Arrange
            // Act
            var a = new AsyncApiAny("string");
            var b = new AsyncApiAny(1);
            var c = new AsyncApiAny(1.1);
            var d = new AsyncApiAny(true);
            var e = new AsyncApiAny(new MyType("test"));
            var f = new AsyncApiAny(new List<string>() { "test", "test2" });
            var g = new AsyncApiAny(new List<string>() { "test", "test2" }.AsEnumerable());
            var h = new AsyncApiAny(new List<MyType>() { new MyType("test") });
            var i = new AsyncApiAny(new Dictionary<string, int>() { { "t", 2 } });
            var j = new AsyncApiAny(new Dictionary<string, MyType>() { { "t", new MyType("test") } });

            // Assert
            Assert.AreEqual("string", a.GetValue<string>());
            Assert.AreEqual(1, b.GetValue<int>());
            Assert.AreEqual(1.1, c.GetValue<double>());
            Assert.AreEqual(true, d.GetValue<bool>());
            Assert.NotNull(e.GetValue<MyType>());
            Assert.IsNotEmpty(f.GetValue<List<string>>());
            Assert.IsNotEmpty(f.GetValue<IEnumerable<string>>());
            Assert.IsNotEmpty(g.GetValue<List<string>>());
            Assert.IsNotEmpty(g.GetValue<IEnumerable<string>>());
            Assert.IsNotEmpty(h.GetValue<List<MyType>>());
            Assert.IsNotEmpty(h.GetValue<IEnumerable<MyType>>());
            Assert.IsNotEmpty(i.GetValue<Dictionary<string, int>>());
            Assert.IsNotEmpty(j.GetValue<Dictionary<string, MyType>>());
        }

        class MyType
        {
            public MyType(string value)
            {
                this.Value = value;
            }

            public string Value { get; set; }
        }
    }
}
