﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17020
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace outcold.MoMaSy.Resources.Strings {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class StringResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal StringResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("outcold.MoMaSy.Resources.Strings.StringResources", typeof(StringResources).Assembly);
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
        ///   Looks up a localized string similar to {0} ({1}).
        /// </summary>
        internal static string Account_FullNameFormat {
            get {
                return ResourceManager.GetString("Account_FullNameFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to New Account.
        /// </summary>
        internal static string Account_NewName {
            get {
                return ResourceManager.GetString("Account_NewName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CCC.
        /// </summary>
        internal static string CurrencyType_NewName {
            get {
                return ResourceManager.GetString("CurrencyType_NewName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot create new account. Add at last one currency type and make it visible..
        /// </summary>
        internal static string ErrMsg_Account_CannotCreateNew {
            get {
                return ResourceManager.GetString("ErrMsg_Account_CannotCreateNew", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Current account is used by transactions. It cannot be deleted. You can set true for &quot;Is Hidden&quot; to hide it..
        /// </summary>
        internal static string ErrMsg_Account_UsedByTransaction {
            get {
                return ResourceManager.GetString("ErrMsg_Account_UsedByTransaction", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Current currency type is used by accounts. It cannot be deleted. You can set true for &quot;Is Hidden&quot; to hide it..
        /// </summary>
        internal static string ErrMsg_CurrencyType_UsedByAccount {
            get {
                return ResourceManager.GetString("ErrMsg_CurrencyType_UsedByAccount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Item is not saved: &apos;{0}&apos;..
        /// </summary>
        internal static string ErrMsg_ItemUnsaved {
            get {
                return ResourceManager.GetString("ErrMsg_ItemUnsaved", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to At least one account should exists. Please add it first..
        /// </summary>
        internal static string ErrMsg_Transaction_AtLeastOneAccount {
            get {
                return ResourceManager.GetString("ErrMsg_Transaction_AtLeastOneAccount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to At least one currency type should exists. Please add it first..
        /// </summary>
        internal static string ErrMsg_Transaction_AtLeastOneCurrency {
            get {
                return ResourceManager.GetString("ErrMsg_Transaction_AtLeastOneCurrency", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to At least one transaction type should exists. Please add it first..
        /// </summary>
        internal static string ErrMsg_Transaction_AtLeastOneTransactionType {
            get {
                return ResourceManager.GetString("ErrMsg_Transaction_AtLeastOneTransactionType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You should set account for transaction..
        /// </summary>
        internal static string ErrMsg_Transaction_SetAccount {
            get {
                return ResourceManager.GetString("ErrMsg_Transaction_SetAccount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You should set account from which you want transfer money..
        /// </summary>
        internal static string ErrMsg_Transaction_SetAccountFrom {
            get {
                return ResourceManager.GetString("ErrMsg_Transaction_SetAccountFrom", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You should set account to which you want transfer money..
        /// </summary>
        internal static string ErrMsg_Transaction_SetAccountTo {
            get {
                return ResourceManager.GetString("ErrMsg_Transaction_SetAccountTo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Current transaction type is used by transactions. It cannot be deleted. You can set true for &quot;Is Hidden&quot; to hide it..
        /// </summary>
        internal static string ErrMsg_TransactionType_UsedByTransaction {
            get {
                return ResourceManager.GetString("ErrMsg_TransactionType_UsedByTransaction", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Transaction Types.
        /// </summary>
        internal static string TransactionType_CollectionName {
            get {
                return ResourceManager.GetString("TransactionType_CollectionName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to New Transaction Type.
        /// </summary>
        internal static string TransactionType_NewName {
            get {
                return ResourceManager.GetString("TransactionType_NewName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Name should be input for transaction type..
        /// </summary>
        internal static string ValidationMsg_TransactionType_Name {
            get {
                return ResourceManager.GetString("ValidationMsg_TransactionType_Name", resourceCulture);
            }
        }
    }
}
