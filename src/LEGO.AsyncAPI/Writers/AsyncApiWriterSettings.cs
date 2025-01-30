// Copyright (c) The LEGO Group. All rights reserved.

namespace LEGO.AsyncAPI.Writers
{
    using LEGO.AsyncAPI.Models;

    /// <summary>
    /// Contains settings for writing async api.
    /// </summary>
    public class AsyncApiWriterSettings : AsyncApiSettings
    {
        private ReferenceInlineSetting referenceInline = ReferenceInlineSetting.DoNotInlineReferences;

        static AsyncApiWriterSettings()
        {
            Default = new AsyncApiWriterSettings();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncApiWriterSettings"/> class.
        /// </summary>
        public AsyncApiWriterSettings()
        {
            this.InlineLocalReferences = false;
            this.LoopDetector = new LoopDetector();
        }

        /// <summary>
        /// Gets the default settings to use for writing async api.
        /// </summary>
        public static AsyncApiWriterSettings Default { get; }

        /// <summary>
        /// Gets or sets indicates how references in the source document should be handled.
        /// </summary>
        public ReferenceInlineSetting ReferenceInline
        {
            get
            {
                return this.referenceInline;
            }

            set
            {
                this.referenceInline = value;
                switch (this.referenceInline)
                {
                    case ReferenceInlineSetting.DoNotInlineReferences:
                        this.InlineLocalReferences = false;
                        break;
                    case ReferenceInlineSetting.InlineReferences:
                        this.InlineLocalReferences = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether indicates if local references should be rendered as an inline object.
        /// </summary>
        public bool InlineLocalReferences { get; set; }

        /// <summary>
        /// Figures out if a loop exists.
        /// </summary>
        internal LoopDetector LoopDetector { get; }

        /// <summary>
        /// Returns back if the refernece should be inlined or not.
        /// </summary>
        /// <param name="reference">The refernece.</param>
        /// <returns>True if it should be inlined otherwise false.</returns>
        public bool ShouldInlineReference(AsyncApiReference reference)
        {
            if (reference.IsExternal)
            {
                return false;
            }

            return this.InlineLocalReferences;
        }
    }
}
