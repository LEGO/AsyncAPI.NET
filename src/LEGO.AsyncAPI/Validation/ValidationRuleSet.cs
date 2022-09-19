// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace LEGO.AsyncAPI.Validations
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using LEGO.AsyncAPI.Exceptions;

    /// <summary>
    /// The rule set of the validation.
    /// </summary>
    public sealed class ValidationRuleSet : IEnumerable<ValidationRule>
    {
        private IDictionary<Type, IList<ValidationRule>> rules = new Dictionary<Type, IList<ValidationRule>>();

        private static ValidationRuleSet defaultRuleSet;

        private IList<ValidationRule> emptyRules = new List<ValidationRule>();

        /// <summary>
        /// Retrieve the rules that are related to a specific type
        /// </summary>
        /// <param name="type">The type that is to be validated</param>
        /// <returns>Either the rules related to the type, or an empty list.</returns>
        public IList<ValidationRule> FindRules(Type type)
        {
            IList<ValidationRule> results = null;
            this.rules.TryGetValue(type, out results);
            return results ?? this.emptyRules;
        }

        /// <summary>
        /// Gets the default validation rule sets.
        /// </summary>
        /// <remarks>
        /// This is a method instead of a property to signal that a new default ruleset object is created
        /// per call. Making this a property may be misleading callers to think the returned rulesets from multiple calls
        /// are the same objects.
        /// </remarks>
        public static ValidationRuleSet GetDefaultRuleSet()
        {
            // Reflection can be an expensive operation, so we cache the default rule set that has already been built.
            if (defaultRuleSet == null)
            {
                defaultRuleSet = BuildDefaultRuleSet();
            }

            // We create a new instance of ValidationRuleSet per call as a safeguard
            // against unintentional modification of the private _defaultRuleSet.
            return new ValidationRuleSet(defaultRuleSet);
        }

        /// <summary>
        /// Return Ruleset with no rules
        /// </summary>
        public static ValidationRuleSet GetEmptyRuleSet()
        {
            // We create a new instance of ValidationRuleSet per call as a safeguard
            // against unintentional modification of the private _defaultRuleSet.
            return new ValidationRuleSet();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationRuleSet"/> class.
        /// </summary>
        public ValidationRuleSet()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationRuleSet"/> class.
        /// </summary>
        /// <param name="ruleSet">Rule set to be copied from.</param>
        public ValidationRuleSet(ValidationRuleSet ruleSet)
        {
            if (ruleSet == null)
            {
                return;
            }

            foreach (ValidationRule rule in ruleSet)
            {
                this.Add(rule);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationRuleSet"/> class.
        /// </summary>
        /// <param name="rules">Rules to be contained in this ruleset.</param>
        public ValidationRuleSet(IEnumerable<ValidationRule> rules)
        {
            if (rules == null)
            {
                return;
            }

            foreach (ValidationRule rule in rules)
            {
                this.Add(rule);
            }
        }

        /// <summary>
        /// Gets the rules in this rule set.
        /// </summary>
        public IList<ValidationRule> Rules
        {
            get
            {
                return this.rules.Values.SelectMany(v => v).ToList();
            }
        }

        /// <summary>
        /// Add the new rule into the rule set.
        /// </summary>
        /// <param name="rule">The rule.</param>
        public void Add(ValidationRule rule)
        {
            if (!this.rules.ContainsKey(rule.ElementType))
            {
                this.rules[rule.ElementType] = new List<ValidationRule>();
            }

            if (this.rules[rule.ElementType].Contains(rule))
            {
                throw new AsyncApiException("Rules cannot be added twice");
            }

            this.rules[rule.ElementType].Add(rule);
        }

        /// <summary>
        /// Get the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<ValidationRule> GetEnumerator()
        {
            foreach (var ruleList in this.rules.Values)
            {
                foreach (var rule in ruleList)
                {
                    yield return rule;
                }
            }
        }

        /// <summary>
        /// Get the enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private static ValidationRuleSet BuildDefaultRuleSet()
        {
            ValidationRuleSet ruleSet = new ValidationRuleSet();
            Type validationRuleType = typeof(ValidationRule);

            IEnumerable<PropertyInfo> rules = typeof(ValidationRuleSet).Assembly.GetTypes()
                .Where(t => t.IsClass
                            && t != typeof(object)
                            && t.GetCustomAttributes(typeof(AsyncApiRuleAttribute), false).Any())
                .SelectMany(t2 => t2.GetProperties(BindingFlags.Static | BindingFlags.Public)
                                .Where(p => validationRuleType.IsAssignableFrom(p.PropertyType)));

            foreach (var property in rules)
            {
                var propertyValue = property.GetValue(null); // static property
                ValidationRule rule = propertyValue as ValidationRule;
                if (rule != null)
                {
                    ruleSet.Add(rule);
                }
            }

            return ruleSet;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AsyncApiRuleAttribute : Attribute
    {
    }
}
