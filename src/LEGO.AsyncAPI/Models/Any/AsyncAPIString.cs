﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

namespace LEGO.AsyncAPI.Models.Any
{
    using LEGO.AsyncAPI.Models.Interfaces;

    /// <summary>
    /// AsyncApi string type.
    /// </summary>
    public class AsyncApiString : AsyncApiPrimitive<string>
    {
        private bool isExplicit;
        private bool isRawString;

        /// <summary>
        /// Initializes the <see cref="AsyncApiString"/> class.
        /// </summary>
        /// <param name="value"></param>
        public AsyncApiString(string value)
            : this(value, false)
        {
        }

        /// <summary>
        /// Initializes the <see cref="AsyncApiString"/> class.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isExplicit">Used to indicate if a string is quoted.</param>
        public AsyncApiString(string value, bool isExplicit)
            : base(value)
        {
            this.isExplicit = isExplicit;
        }

        /// <summary>
        /// Initializes the <see cref="AsyncApiString"/> class.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isExplicit">Used to indicate if a string is quoted.</param>
        /// <param name="isRawString">Used to indicate to the writer that the value should be written without encoding.</param>
        public AsyncApiString(string value, bool isExplicit, bool isRawString)
            : base(value)
        {
            this.isExplicit = isExplicit;
            this.isRawString = isRawString;
        }

        /// <summary>
        /// The primitive class this object represents.
        /// </summary>
        public override PrimitiveType PrimitiveType { get; } = PrimitiveType.String;

        /// <summary>
        /// True if string was specified explicitly by the means of double quotes, single quotes, or literal or folded style.
        /// </summary>
        public bool IsExplicit()
        {
            return this.isExplicit;
        }

        /// <summary>
        /// True if the writer should process the value as supplied without encoding.
        /// </summary>
        public bool IsRawString()
        {
            return this.isRawString;
        }
    }
}
