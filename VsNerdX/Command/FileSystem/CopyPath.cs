using System;
using System.Windows.Forms;
using EnvDTE80;
using VsNerdX.Core;
using VsNerdX.Util;

namespace VsNerdX.Command.Navigation
{
    public class CopyPath : ICommand
    {
        private readonly IHierarchyControl _hierarchyControl;
        private DTE2 dte;

        public CopyPath(IHierarchyControl hierarchyControl)
        {
            this._hierarchyControl = hierarchyControl;
            this.dte = ((HierarchyControl) _hierarchyControl).dte;
        }

        public ExecutionResult Execute(IExecutionContext executionContext, Keys key)
        {
            string path = "";
            try
            {
                var selectedItem = dte.SelectedItems.Item(1);
                if (selectedItem.Project != null)
                {
                    path = selectedItem.Project.FullName;
                }
                else if (selectedItem.ProjectItem != null)
                {
                    path = selectedItem.ProjectItem.FileNames[1];
                }
                else if (selectedItem.Name == (string) dte.Solution.Properties.Item("Name").Value)
                {
                    path = dte.Solution.FullName;
                }
                else
                {
                    path = TreeHelper.GetCanonicalName(_hierarchyControl.GetSelectedItem());
                }
            }
            catch (NotImplementedException e)
            {
                path = (string) _hierarchyControl.GetSelectedItem().GetValue("Item").GetValue("WorkspaceVisualNode").GetValue("FullPath");
            }
            catch (Exception e) { }

            if (path != null)
            {
                Clipboard.SetText(path);
            }

            executionContext = executionContext.Clear().With(mode: InputMode.Normal);
            return new ExecutionResult(executionContext, CommandState.Handled);
        }
    }
}
