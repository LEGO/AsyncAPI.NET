using LEGO.AsyncAPI.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LEGO.AsyncAPI.Tests
{

    public class AsyncApiAnyTests
    {
        [Test]
        public void Test()
        {
            // Arrange
            // Act
            // Assert
        }


        [Test]
        public void ctor_Primitives()
        {
            // Arrange
            // Act
            var a = new AsyncApiAny("string");
            var b = new AsyncApiAny(1);
            var c = new AsyncApiAny(1.1);
            var d = new AsyncApiAny(true);
            var e = new AsyncApiAny(new MyType("test"));
            var f = new AsyncApiAny(new List<string>() { "test", "test2"});
            var g = new AsyncApiAny(new List<MyType>() { new MyType("test") });
            var h = new AsyncApiAny(new Dictionary<string, int>() { { "t", 2 } });
            var i = new AsyncApiAny(new Dictionary<string, MyType>() { { "t", new MyType("test") } });

            var v = e.GetValue<MyType>();
            var k = i.GetValue<Dictionary<string, MyType>>();
            // Assert
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
