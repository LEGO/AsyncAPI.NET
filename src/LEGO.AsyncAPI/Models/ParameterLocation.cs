// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Models
{
    using LEGO.AsyncAPI.Attributes;

    public enum ParameterLocation
    {
        /// <summary>
        /// The user
        /// </summary>
        [Display("user")] User,

        /// <summary>
        /// The password
        /// </summary>
        [Display("password")] Password,

        /// <summary>
        /// Parameters that are appended to the URL.
        /// </summary>
        [Display("query")] Query,

        /// <summary>
        /// Custom headers that are expected as part of the request.
        /// </summary>
        [Display("header")] Header,

        /// <summary>
        /// Used to pass a specific cookie value to the API.
        /// </summary>
        [Display("cookie")] Cookie
    }
}