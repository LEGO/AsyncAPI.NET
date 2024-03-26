﻿// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using LEGO.AsyncAPI.Attributes;

    public static class EnumExtensions
    {
        /// <summary>
        /// Gets an attribute on an enum field value.
        /// </summary>
        /// <typeparam name="T">The type of the attribute to retrieve.</typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>
        /// The attribute of the specified type or null.
        /// </returns>
        public static T GetAttributeOfType<T>(this Enum enumValue)
            where T : Attribute
        {
            var type = enumValue.GetType();
            var memInfo = type.GetMember(enumValue.ToString()).First();
            var attributes = memInfo.GetCustomAttributes<T>(false);
            return attributes.FirstOrDefault();
        }

        /// <summary>
        /// Gets the enum display name.
        /// </summary>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>
        /// Use <see cref="DisplayAttribute"/> if exists.
        /// Otherwise, use the standard string representation.
        /// </returns>
        public static string GetDisplayName(this Enum enumValue)
        {
            var attribute = enumValue.GetAttributeOfType<DisplayAttribute>();
            return attribute == null ? enumValue.ToString() : attribute.Name;
        }

        public static IEnumerable<TEnum> GetFlags<TEnum>(this Enum input)
            where TEnum : Enum
        {
            foreach (TEnum value in System.Enum.GetValues(input.GetType()))
            {
                if (input.HasFlag(value))
                {
                    yield return value;
                }
            }
        }
    }
}
