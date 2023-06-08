using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using LEGO.AsyncAPI.Bindings;
using LEGO.AsyncAPI.Models.Any;
using NUnit.Framework;

namespace LEGO.AsyncAPI.Tests.Bindings;

public class StringOrStringList_Should
{
    
    [Test]
    public void StringOrStringList_IsInitialised_WhenPassedStringOrStringList()
    {
        // Arrange
        var stringValue = new StringOrStringList(new AsyncApiString("AsyncApi"));
        var listValue = new StringOrStringList(
            new AsyncApiArray()
            {
                new AsyncApiString("Async"),
                new AsyncApiString("Api"),
            });

        // Assert
        (stringValue.Value as AsyncApiString).Value.Should().Be("AsyncApi");
        (listValue.Value as AsyncApiArray)
            .Select(s => (s as AsyncApiString).Value)
            .Should().BeEquivalentTo(new List<string>() { "Async", "Api" });
    }

    [Test]
    public void StringOrStringList_ThrowsArgumentException_WhenIntialisedWithoutStringOrStringList()
    {
        // Assert
        var ex = Assert.Throws<ArgumentException>(() => new StringOrStringList(new AsyncApiBoolean(true)));

        // Assert
        ex.Message.Should().Be("StringOrStringList should be a string value or a string list.");
    }

    [Test]
    public void StringOrStringList_ThrowsArgumentException_WhenIntialisedWithListOfNonStrings()
    {
        // Assert
        var ex = Assert.Throws<ArgumentException>(() => new StringOrStringList(
            new AsyncApiArray()
            {
                new AsyncApiString("x"),
                new AsyncApiInteger(1),
                new AsyncApiString("y"),
            }));

        // Assert
        ex.Message.Should().Be("StringOrStringList value should only contain string items.");
    }
}