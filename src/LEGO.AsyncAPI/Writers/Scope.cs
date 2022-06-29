// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Writers
{
    /// <summary>
    /// Class representing scope information.
    /// </summary>
    public sealed class Scope
    {
        /// <summary>
        /// The type of the scope.
        /// </summary>
        private readonly ScopeType type;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scope"/> class.
        /// Constructor.
        /// </summary>
        /// <param name="type">The type of the scope.</param>
        public Scope(ScopeType type)
        {
            this.type = type;
        }

        /// <summary>
        /// Gets or sets get/Set the object count for this scope.
        /// </summary>
        /// <value>
        /// The object count.
        /// </value>
        public int ObjectCount { get; set; }

        /// <summary>
        /// Gets the scope type for this scope.
        /// </summary>
        public ScopeType Type
        {
            get
            {
                return this.type;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether get/Set the whether it is in previous array scope.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is in array; otherwise, <c>false</c>.
        /// </value>
        public bool IsInArray { get; set; } = false;
    }
}
