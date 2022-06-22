//// Copyright (c) The LEGO Group. All rights reserved.

//namespace LEGO.AsyncAPI.Models.Bindings.OperationBindings
//{
//    using System.Collections.Generic;
//    using LEGO.AsyncAPI.Models.Interfaces;

//    /// <summary>
//    /// Binding class for http operations.
//    /// </summary>
//    public class HttpOperationBinding : IOperationBinding
//    {
//        /// <summary>
//        /// Gets or sets type of binding.
//        /// </summary>
//        public string Type { get; set; }

//        /// <summary>
//        /// Gets or sets http method.
//        /// </summary>
//        public string Method { get; set; }

//        /// <summary>
//        /// Gets or sets http query.
//        /// </summary>
//        public Schema Query { get; set; }

//        /// <summary>
//        /// Gets or sets property containing version of a binding.
//        /// </summary>
//        public string BindingVersion { get; set; }

//        /// <inheritdoc/>
//        public IDictionary<string, IAsyncApiAny> Extensions { get; set; }
//    }
//}