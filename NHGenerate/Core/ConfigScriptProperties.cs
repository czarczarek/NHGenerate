using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace NHGenerate.Core
{
    /// <summary>
    /// Responsible for retrieve parameters from registers.
    /// </summary>
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDual)]
    public class ConfigScriptProperties
    {
        /// <summary>
        /// Location of the registry key.
        /// </summary>
        private const string RegistryDir = @"SOFTWARE\Microsoft\VisualStudio\11.0\NHGenerate";

        /// <summary>
        /// Default class namespace.
        /// </summary>
        public string DefaultScriptDatabase
        {
            get { return GetValue("DefaultScriptDatabase", string.Empty); }
            set { SetValue("DefaultScriptDatabase", value); }
        }

        /// <summary>
        /// Default class namespace.
        /// </summary>
        public string UpdateScriptTemplate
        {
            get { return GetValue("UpdateScriptTemplate", "envPortal.Update.{0}.sql"); }
            set { SetValue("UpdateScriptTemplate", value); }
        }

        /// <summary>
        /// Default schema name.
        /// </summary>
        public string UpdateScriptSchemaName
        {
            get { return GetValue("UpdateScriptSchemaName", "dbo"); }
            set { SetValue("UpdateScriptSchemaName", value); }
        }

        /// <summary>
        /// Default table name.
        /// </summary>
        public string UpdateScriptTableName
        {
            get { return GetValue("UpdateScriptTableName", "UpdateScriptFileNames"); }
            set { SetValue("UpdateScriptTableName", value); }
        }

        /// <summary>
        /// User that create the script.
        /// </summary>
        public string UpdateScriptUserName
        {
            get { return GetValue("UpdateScriptUserName", ""); }
            set { SetValue("UpdateScriptUserName", value); }
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
