﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tekapo.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.7.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(".jpg,.jpeg")]
        public string SupportedFileTypes {
            get {
                return ((string)(this["SupportedFileTypes"]));
            }
            set {
                this["SupportedFileTypes"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string LastSearchDirectory {
            get {
                return ((string)(this["LastSearchDirectory"]));
            }
            set {
                this["LastSearchDirectory"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool SearchSubDirectories {
            get {
                return ((bool)(this["SearchSubDirectories"]));
            }
            set {
                this["SearchSubDirectories"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public string SearchFilterType {
            get {
                return ((string)(this["SearchFilterType"]));
            }
            set {
                this["SearchFilterType"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string WildcardSearchFilter {
            get {
                return ((string)(this["WildcardSearchFilter"]));
            }
            set {
                this["WildcardSearchFilter"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string RegularExpressionSearchFilter {
            get {
                return ((string)(this["RegularExpressionSearchFilter"]));
            }
            set {
                this["RegularExpressionSearchFilter"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string NameFormat {
            get {
                return ((string)(this["NameFormat"]));
            }
            set {
                this["NameFormat"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool IncrementOnCollision {
            get {
                return ((bool)(this["IncrementOnCollision"]));
            }
            set {
                this["IncrementOnCollision"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1000")]
        public int MaxCollisionIncrement {
            get {
                return ((int)(this["MaxCollisionIncrement"]));
            }
            set {
                this["MaxCollisionIncrement"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string NameFormatMRU {
            get {
                return ((string)(this["NameFormatMRU"]));
            }
            set {
                this["NameFormatMRU"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public int MaxNameFormatMRUItems {
            get {
                return ((int)(this["MaxNameFormatMRUItems"]));
            }
            set {
                this["MaxNameFormatMRUItems"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string SearchDirectoryMRU {
            get {
                return ((string)(this["SearchDirectoryMRU"]));
            }
            set {
                this["SearchDirectoryMRU"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5")]
        public int MaxSearchDirectoryMRUItems {
            get {
                return ((int)(this["MaxSearchDirectoryMRUItems"]));
            }
            set {
                this["MaxSearchDirectoryMRUItems"] = value;
            }
        }
    }
}
