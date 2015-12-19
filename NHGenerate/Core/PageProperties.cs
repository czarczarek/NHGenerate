using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace NHGenerate.Core
{
    /// <summary>
    /// Responsible for retrieve parameters from registers.
    /// </summary>
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    public class PageProperties
    {
        /// <summary>
        /// Location of the registry key.
        /// </summary>
        private const string RegistryDir = @"SOFTWARE\Microsoft\VisualStudio\11.0\NHGenerate";

        /// <summary>
        /// Default class namespace.
        /// </summary>
        public string DefaultNamespace
        {
            get { return GetValue("DefaultNamespace", "DefaultNamespace"); }
            set { SetValue("DefaultNamespace", value); }
        }

        /// <summary>
        /// Whether to include column name.
        /// </summary>
        public bool IncludeColumnName
        {
            get { return GetValue("IncludeColumnName", "1") == "1"; }
            set { SetValue("IncludeColumnName", (value ? "1" : "0")); }
        }

        /// <summary>
        /// Whether to include column size.
        /// </summary>
        public bool IncludeColumnSize
        {
            get { return GetValue("IncludeColumnSize", "1") == "1"; }
            set { SetValue("IncludeColumnSize", (value ? "1" : "0")); }
        }

        /// <summary>
        /// Whether to include NotNull clause.
        /// </summary>
        public bool IncludeNotNull
        {
            get { return GetValue("IncludeNotNull", "1") == "1"; }
            set { SetValue("IncludeNotNull", (value ? "1" : "0")); }
        }

        /// <summary>
        /// Whether to use short type name.
        /// </summary>
        public bool UseShortTypeName
        {
            get { return GetValue("UseShortTypeName", "1") == "1"; }
            set { SetValue("UseShortTypeName", (value ? "1" : "0")); }
        }

        #region Helper

        /// <summary>
        /// Set registry key.
        /// </summary>
        /// <param name="key">Registry key.</param>
        /// <param name="value">Default value if empty</param>
        public static void SetValue(string key, string value)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(RegistryDir, true) ?? Registry.CurrentUser.CreateSubKey(RegistryDir);
            string registryValue = !string.IsNullOrEmpty(value) ? value : "-1";
            if (registryKey != null) registryKey.SetValue(key, registryValue, RegistryValueKind.String);
        }

        /// <summary>
        /// Get registry key. Create key, if not exists.
        /// </summary>
        /// <param name="key">Registry key.</param>
        /// <param name="defaultValue">Default value if empty.</param>
        /// <returns>Registry value.</returns>
        public static string GetValue(string key, string defaultValue)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(RegistryDir, false) ?? Registry.CurrentUser.CreateSubKey(RegistryDir);
            var registryValue = (string)registryKey.GetValue(key, "-1");
            return registryValue == "-1" ? defaultValue : registryValue;
        }

        #endregion
    }
}
