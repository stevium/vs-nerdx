using System.Windows.Forms;
using VsNerdX.Core;
using VsNerdX.Util;
using static VsNerdX.VsNerdXPackage;

namespace VsNerdX.Command.Navigation
{
    public class CopyPath : ICommand
    {
        private readonly IHierarchyControl _hierarchyControl;

        public CopyPath(IHierarchyControl hierarchyControl)
        {
            this._hierarchyControl = hierarchyControl;
        }

        public ExecutionResult Execute(IExecutionContext executionContext, Keys key)
        {
            string path;
            var selectedTreeNode = _hierarchyControl.GetSelectedItem();
            var wsVisualNode = selectedTreeNode.GetValue("Item").GetValue("WorkspaceVisualNode");

            if (wsVisualNode != null)
            {
                path = (string)wsVisualNode.GetValue("FullPath");
            }
            else
            {
                var selectedItem = Dte.SelectedItems.Item(1);
                var selectedProject = selectedItem.Project;
                if (selectedProject != null)
                {
                    path = selectedProject.FullName != "" ? selectedProject.FullName : selectedProject.Name;
                }
                else if (selectedItem.ProjectItem != null)
                {
                    path = selectedItem.ProjectItem.FileNames[1];
                }
                else if (selectedItem.Name == (string)Dte.Solution.Properties.Item("Name").Value)
                {
                    path = Dte.Solution.FullName;
                }
                else
                {
                    path = TreeHelper.GetText(selectedTreeNode);
                }
            }

            if (path != null)
            {
                Clipboard.SetText(path);
            }

            executionContext = executionContext.Clear().With(mode: InputMode.Normal);
            return new ExecutionResult(executionContext, CommandState.Handled);
        }
    }
}
