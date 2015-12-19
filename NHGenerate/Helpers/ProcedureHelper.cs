using System;
using System.Collections.Generic;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using NHGenerate.Core;
using NHGenerate.UI;

namespace NHGenerate
{
    public class ProcedureHelper
    {
        private DTE2 _applicationObject;
        private AddIn _addInInstance;

        public ProcedureHelper(DTE2 applicationObject, AddIn addInInstance)
        {
            _applicationObject = applicationObject;
            _addInInstance = addInInstance;

        }

        public void FindUnusesProcedures(List<Procedure> procedures)
        {
            try
            {
                var win = new SearchProgress
                {
                    ApplicationObject = _applicationObject,
                    AddInstance = _addInInstance,
                    Procedures = procedures
                };
                win.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}