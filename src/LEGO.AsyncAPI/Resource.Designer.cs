﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LEGO.AsyncAPI {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("LEGO.AsyncAPI.Resource", typeof(Resource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The string &apos;{0}&apos; MUST be an email address..
        /// </summary>
        internal static string Validation_EmailMustBeEmailFormat {
            get {
                return ResourceManager.GetString("Validation_EmailMustBeEmailFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The extension name &apos;{0}&apos; in &apos;{1}&apos; object MUST being with &apos;x-&apos;..
        /// </summary>
        internal static string Validation_ExtensionNameMustBeginWithXDash {
            get {
                return ResourceManager.GetString("Validation_ExtensionNameMustBeginWithXDash", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The field &apos;{0}&apos; in &apos;{1}&apos; object is REQUIRED..
        /// </summary>
        internal static string Validation_FieldRequired {
            get {
                return ResourceManager.GetString("Validation_FieldRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The key &apos;{0}&apos; in &apos;{1}&apos; MUST match the regular expression &apos;{2}&apos;..
        /// </summary>
        internal static string Validation_KeyMustMatchRegularExpr {
            get {
                return ResourceManager.GetString("Validation_KeyMustMatchRegularExpr", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The field &apos;{0}&apos; in &apos;{1}&apos; object MUST be an absolute uri..
        /// </summary>
        internal static string Validation_MustBeAbsoluteUrl {
            get {
                return ResourceManager.GetString("Validation_MustBeAbsoluteUrl", resourceCulture);
            }
        }
    }
}
