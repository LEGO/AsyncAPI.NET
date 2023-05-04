// Copyright (c) The LEGO Group. All rights reserved.

using LEGO.AsyncAPI.Validations;
using NUnit.Framework;

namespace LEGO.AsyncAPI.Tests.Validation
{
    public class ValidationRuleSetTests
    {
        [Test]
        public void DefaultRuleSet_ReturnsTheCorrectRules()
        {
            // Arrange
            var ruleSet = new ValidationRuleSet();

            // Act
            var rules = ruleSet.Rules;

            // Assert
            Assert.NotNull(rules);
            Assert.IsEmpty(rules);
        }

        [Test]
        public void DefaultRuleSet_PropertyReturnsTheCorrectRules()
        {
            // Arrange & Act
            var ruleSet = ValidationRuleSet.GetDefaultRuleSet();
            Assert.NotNull(ruleSet); // guard

            var rules = ruleSet.Rules;

            // Assert
            Assert.NotNull(rules);
            Assert.IsNotEmpty(rules);

            // Update the number if you add new default rule(s).
            Assert.AreEqual(17, rules.Count);
        }
    }
}
