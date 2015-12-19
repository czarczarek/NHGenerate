using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using NHGenerate.Core;
using NHGenerate.UI;

namespace NHGenerate
{
    /// <summary>The object for implementing an Add-in.</summary>
    /// <seealso class='IDTExtensibility2' />
    public class Connect : IDTExtensibility2, IDTCommandTarget
    {
        readonly PageProperties _properties = new PageProperties();
        readonly ConfigScriptProperties _configScriptProperties = new ConfigScriptProperties();

        /// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
        public Connect()
        {
        }

        [ComImport, Guid("6D5140C1-7436-11CE-8034-00AA006009FA"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IOleServiceProvider
        {
            [PreserveSig]
            int QueryService([In]ref Guid guidService, [In]ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out System.Object obj);
        }

        private CommandBar FindCommandBar(Guid guidCmdGroup, uint menuID)
        {
            var sp = (IOleServiceProvider)_applicationObject;
            Guid guidSvc = typeof(IVsProfferCommands).GUID;
            Object objService;
            sp.QueryService(ref guidSvc, ref guidSvc, out objService);
            var vsProfferCmds = (IVsProfferCommands)objService;
            return vsProfferCmds.FindCommandBar(IntPtr.Zero, ref guidCmdGroup, menuID) as CommandBar;
        }

        /// Add or change the registry key HKEY_CURRENT_USER\Software\Microsoft\VisualStudio\11.0\General\EnableVSIPLogging to 1.
        /// Click on the toolbar or menu you want identify while keeping CTRL+SHIFT pressed
        /// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
        /// <param term='application'>Root object of the host application.</param>
        /// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
        /// <param term='addInInst'>Object representing this Add-in.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            _applicationObject = (DTE2)application;
            _addInInstance = (AddIn)addInInst;

            if (connectMode == ext_ConnectMode.ext_cm_UISetup)
            {
                var contextGuids = new object[] { };
                var commands = (Commands2)_applicationObject.Commands;

                //CommandBar menuBarCommandBar = ((CommandBars)_applicationObject.CommandBars)["Object Node"];

                CommandBar menuBarCommandBar = FindCommandBar(new Guid("{D4F02A6A-C5AE-4BF2-938D-F1625BDCA0E2}"), 33280); 

                CommandBar mBarCommandBar = ((CommandBars)_applicationObject.CommandBars)["Static Node"];

                

                try
                {
                    CommandBar menuCommandBar = ((CommandBars)_applicationObject.CommandBars)["Folder"];
                    if (menuCommandBar != null)
                    {
                        foreach (CommandBarControl cbp in menuCommandBar.Controls)
                        {
                            if (cbp is CommandBarPopup && cbp.Caption == "A&dd")
                            {
                                var cmdBar = ((CommandBarPopup)cbp).CommandBar;

                                Command newUpdateScriptCommand = commands.AddNamedCommand2(_addInInstance, "NewUpdateScript", "New update script...", "New update script...", false, 8, ref contextGuids, (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled, (int)vsCommandStyle.vsCommandStylePictAndText, vsCommandControlType.vsCommandControlTypeButton);
                                newUpdateScriptCommand.AddControl(cmdBar, 1);

                                Command configUpdateScriptCommand = commands.AddNamedCommand2(_addInInstance, "ConfigUpdateScript", "Config update script", "Config update script", false, 11, ref contextGuids, (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled, (int)vsCommandStyle.vsCommandStylePictAndText, vsCommandControlType.vsCommandControlTypeButton);
                                configUpdateScriptCommand.AddControl(cmdBar, 2);

                                break;
                            }
                        }
                    }

                    Command cmdGenerate = commands.AddNamedCommand2(_addInInstance, "NHGenerate", "Generate model", "Generate model", false, 3, ref contextGuids, (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled, (int)vsCommandStyle.vsCommandStylePictAndText, vsCommandControlType.vsCommandControlTypeButton);
                    Command cmdMethods = commands.AddNamedCommand2(_addInInstance, "NHMethods", "Generate methods", "Generate methods", false, 1, ref contextGuids, (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled, (int)vsCommandStyle.vsCommandStylePictAndText, vsCommandControlType.vsCommandControlTypeButton);
                    Command cmdTest = commands.AddNamedCommand2(_addInInstance, "NHTest", "Generate test", "Generate test", false, 4, ref contextGuids, (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled, (int)vsCommandStyle.vsCommandStylePictAndText, vsCommandControlType.vsCommandControlTypeButton);
                    Command cmdSettings = commands.AddNamedCommand2(_addInInstance, "NHSettings", "Settings", "Settings", false, 2, ref contextGuids, (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled, (int)vsCommandStyle.vsCommandStylePictAndText, vsCommandControlType.vsCommandControlTypeButton);
                    Command cmdFindUnusedProc = commands.AddNamedCommand2(_addInInstance, "NHFindUnusedProc", "Find unused procedures", "Find unused procedures", false, 5, ref contextGuids, (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled, (int)vsCommandStyle.vsCommandStylePictAndText, vsCommandControlType.vsCommandControlTypeButton);

                    var nhCommandBar = (CommandBar)commands.AddCommandBar("NHibernate", vsCommandBarType.vsCommandBarTypeMenu, menuBarCommandBar, 1);

                    if ((nhCommandBar != null) && (cmdGenerate != null) && (cmdMethods != null) && (cmdTest != null) && (cmdSettings != null))
                    {
                        cmdGenerate.AddControl(nhCommandBar, 1);
                        cmdMethods.AddControl(nhCommandBar, 2);
                        cmdTest.AddControl(nhCommandBar, 3);
                        cmdSettings.AddControl(nhCommandBar, 4);
                    }

                    if (mBarCommandBar != null && cmdFindUnusedProc != null)
                    {
                        cmdFindUnusedProc.AddControl(mBarCommandBar, 1);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //If we are here, then the exception is probably because a command with that name
                    //  already exists. If so there is no need to recreate the command and we can 
                    //  safely ignore the exception.
                }
            }

        }

        /// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
        /// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
        {
            if (disconnectMode == ext_DisconnectMode.ext_dm_HostShutdown)
            {
                //try
                //{
                //    var commands = (Commands2)_applicationObject.Commands;

                //    CommandBar nhCommandBar = ((CommandBars)_applicationObject.CommandBars)["NHibernate"];
                //    while (nhCommandBar != null)
                //    {
                //        commands.RemoveCommandBar(nhCommandBar);
                //    }
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
            }
        }

        /// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />		
        public void OnAddInsUpdate(ref Array custom)
        {
        }

        /// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnStartupComplete(ref Array custom)
        {
        }

        /// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnBeginShutdown(ref Array custom)
        {
            
        }

        private DTE2 _applicationObject;
        private AddIn _addInInstance;

        public void Exec(string CmdName, vsCommandExecOption ExecuteOption, ref object VariantIn, ref object VariantOut, ref bool Handled)
        {
            Handled = false;
            if (ExecuteOption == vsCommandExecOption.vsCommandExecOptionDoDefault)
            {
                switch (CmdName)
                {
                    case "NHGenerate.Connect.NHGenerate":
                        {
                            ConnectionResult result = GetDbConnection();

                            if (result.Connection != null)
                            {
                                var setting = new Setting
                                {
                                    Table = result.TableName,
                                    Schema = result.SchemaName,
                                    Namespace = _properties.DefaultNamespace,
                                    IncludeColumnName = _properties.IncludeColumnName,
                                    IncludeColumnSize = _properties.IncludeColumnSize,
                                    IncludeNotNull = _properties.IncludeNotNull,
                                    UseShortTypeName = _properties.UseShortTypeName
                                };
                                string fileContent = new ClassGenerator(result.Connection).NHibernateTableSchema(setting);
                                _applicationObject.ItemOperations.NewFile("General\\Text File", result.FileName, Constants.vsViewKindCode);
                                var objTextDoc = (TextDocument)_applicationObject.ActiveDocument.Object("TextDocument");
                                EditPoint objEditPoint = objTextDoc.StartPoint.CreateEditPoint();
                                objEditPoint.Insert(fileContent);
                            }

                            Handled = true;
                            break;
                        }

                    case "NHGenerate.Connect.NHMethods":
                        {
                            ConnectionResult result = GetDbConnection();

                            if (result.Connection != null)
                            {
                                var setting = new Setting
                                {
                                    Table = result.TableName,
                                    Schema = result.SchemaName,
                                    Namespace = _properties.DefaultNamespace,
                                    IncludeColumnName = _properties.IncludeColumnName,
                                    IncludeColumnSize = _properties.IncludeColumnSize,
                                    IncludeNotNull = _properties.IncludeNotNull,
                                    UseShortTypeName = _properties.UseShortTypeName
                                };
                                string fileContent = new ClassGenerator(result.Connection).NHibernateTableSchema(setting);
                                _applicationObject.ItemOperations.NewFile("General\\Text File", result.FileName, Constants.vsViewKindCode);

                                

                                var objTextDoc = (TextDocument)_applicationObject.ActiveDocument.Object("TextDocument");
                                EditPoint objEditPoint = objTextDoc.StartPoint.CreateEditPoint();
                                objEditPoint.Insert(fileContent);
                            }


                            Handled = true;
                            break;
                        }

                    case "NHGenerate.Connect.NHTest":
                        {
                            ConnectionResult result = GetDbConnection();

                            if (result.Connection != null)
                            {

                            }

                            Handled = true;
                            break;
                        }
                    case "NHGenerate.Connect.NHSettings":
                        {
                            var win = new SettingsWindow();
                            win.ShowDialog();

                            Handled = true;
                            break;
                        }
                    case "NHGenerate.Connect.NHFindUnusedProc":
                        {
                            try
                            {
                                string pattern = @"(?<procedure>^\w+)(\s*\((?<schema>\w+)\))*";

                                var serverExplorer = _applicationObject.ToolWindows.GetToolWindow("Server Explorer") as UIHierarchy;

                                if (serverExplorer != null)
                                {
                                    dynamic item = ((object[])serverExplorer.SelectedItems)[0];
                                    var procedures = new List<Procedure>();
                                    foreach (dynamic i in item.UIHierarchyItems)
                                    {
                                        var regex = new Regex(pattern);
                                        if (regex.IsMatch(i.Name))
                                        {
                                            Match m = regex.Match(i.Name);
                                            var p = new Procedure
                                            {
                                                Name = m.Groups["procedure"].Value,
                                                Schema = string.IsNullOrEmpty(m.Groups["schema"].Value) ? "dbo" : m.Groups["schema"].Value
                                            };

                                            procedures.Add(p);
                                        }
                                    }

                                    new ProcedureHelper(_applicationObject, _addInInstance).FindUnusesProcedures(procedures);
                                }
                            }
                            catch
                            {
                                
                            }

                            Handled = true;
                            break;
                        }
                    case "NHGenerate.Connect.NewUpdateScript":
                        {
                            try
                            {
                                if (string.IsNullOrEmpty(_configScriptProperties.UpdateScriptUserName))
                                {
                                    var result = MessageBox.Show("You must configure user name! Do you want to configure that property now?", "Empty user name", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                    if (result == DialogResult.No)
                                    {
                                        return;
                                    }

                                    var win = new ConfigScriptWindow();
                                    win.ShowDialog();
                                }

                                if (string.IsNullOrEmpty(_configScriptProperties.DefaultScriptDatabase))
                                {
                                    var result = MessageBox.Show("You must configure the connection to the database! Do you want to configure the database connection now?", "Database connection", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                    if (result == DialogResult.No)
                                    {
                                        return;
                                    }

                                    var win = new ConfigScriptWindow();
                                    win.ShowDialog();
                                }

                                DbConnection connection;
                                if (!DbConnection.TestConnection(_configScriptProperties.DefaultScriptDatabase, out connection))
                                {
                                    MessageBox.Show(string.Format("Unable to connect to the database: '{0}'. Please configure update script settings.", _configScriptProperties.DefaultScriptDatabase));
                                    return;
                                }

                                SelectedItems items = _applicationObject.SelectedItems;

                                if (items.Count == 1)
                                {
                                    SelectedItem item = items.Item(1);
                                    if (item != null)
                                    {
                                        var solution = (Solution2)_applicationObject.Solution;
                                        string template = solution.GetProjectItemTemplate("TextFile.zip", "csproj");

                                        var sc = new ScriptGenerator(connection, _configScriptProperties);

                                        var fileName = sc.GetFileName();

                                        var dir = item.ProjectItem.FileNames[0];

                                        var fullName = Path.Combine(dir, fileName);

                                        if (File.Exists(fullName))
                                        {
                                            var result = MessageBox.Show(string.Format("The file '{0}' already exists! Do you want to overwrite this file?", fileName), "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                            if (result == DialogResult.No)
                                            {
                                                return;
                                            }

                                            File.Delete(fullName);
                                        }

                                        item.ProjectItem.ProjectItems.AddFromTemplate(template, fileName);

                                        var objTextDoc = (TextDocument)_applicationObject.ActiveDocument.Object("TextDocument");
                                        EditPoint objEditPoint = objTextDoc.StartPoint.CreateEditPoint();

                                        var content = string.Format(Properties.Settings.Default.ScriptTemplate, sc.FileNumber, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), _configScriptProperties.UpdateScriptUserName);

                                        objEditPoint.Insert(content);
                                        
                                        if (_applicationObject.ActiveDocument != null)
                                        {
                                            _applicationObject.ActiveDocument.Save();
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                            Handled = true;
                            break;
                        }

                    case "NHGenerate.Connect.ConfigUpdateScript":
                        {
                            var win = new ConfigScriptWindow();
                            win.ShowDialog();

                            Handled = true;
                            break;
                        }
                }
            }
        }

        public void QueryStatus(string CmdName, vsCommandStatusTextWanted NeededText, ref vsCommandStatus StatusOption, ref object CommandText)
        {
            if (NeededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
            {
                if (CmdName == "NHGenerate.Connect.NHGenerate")
                {
                    StatusOption = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
                    return;
                }
                if (CmdName == "NHGenerate.Connect.NHMethods")
                {
                    StatusOption = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
                    return;
                }
                if (CmdName == "NHGenerate.Connect.NHTest")
                {
                    StatusOption = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
                    return;
                }
                if (CmdName == "NHGenerate.Connect.NHSettings")
                {
                    StatusOption = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
                    return;
                }
                if (CmdName == "NHGenerate.Connect.NHFindUnusedProc")
                {
                    StatusOption = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
                    return;
                }
                if (CmdName == "NHGenerate.Connect.NewUpdateScript")
                {
                    StatusOption = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
                    return;
                }
                if (CmdName == "NHGenerate.Connect.ConfigUpdateScript")
                {
                    StatusOption = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
                    return;
                }
            }
        }

        private ConnectionResult GetDbConnection()
        {
            var result = new ConnectionResult();

            try
            {
                string pattern = @"(?<table>^\w+)(\s*\((?<schema>\w+)\))*";

                result.SchemaName = "dbo";
                result.TableName = null;
                result.FileName = "Test.cs";
                result.DefaultNamespace = _properties.DefaultNamespace;

                string dataBaseName = null;

                var serverExplorer = _applicationObject.ToolWindows.GetToolWindow("Server Explorer") as UIHierarchy;
                if (serverExplorer != null)
                {
                    dynamic item = ((object[])serverExplorer.SelectedItems)[0];

                    var regex = new Regex(pattern);
                    if (regex.IsMatch(item.Name))
                    {
                        var matches = regex.Matches(item.Name);
                        foreach (Match m in matches)
                        {
                            result.TableName = m.Groups["table"].Value;
                            result.SchemaName = string.IsNullOrEmpty(m.Groups["schema"].Value) ? "dbo" : m.Groups["schema"].Value;
                        }
                    }

                    result.FileName = string.Format("{0}.cs", result.TableName);

                    try
                    {
                        dataBaseName = item.Collection.Parent.Collection.Parent.Name;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                if (!string.IsNullOrEmpty(dataBaseName))
                {
                    DbConnection connection;
                    if (!DbConnection.TestConnection(dataBaseName, out connection))
                    {
                        var win = new ConnectionWindow(dataBaseName);
                        win.ShowDialog();
                        result.Connection = win.Connection;
                    }
                    else
                    {
                        result.Connection = connection;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return result;
        }
    }
}