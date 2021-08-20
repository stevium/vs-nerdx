using System.Windows.Forms;
using VsNerdX.Core;
using static VsNerdX.Util.TreeHelper;

namespace VsNerdX.Command.Directory
{
    public class OpenNodeRecursively : ICommand
    {
        private readonly IHierarchyControl _hierarchyControl;

        public OpenNodeRecursively(IHierarchyControl hierarchyControl)
        {
            this._hierarchyControl = hierarchyControl;
        }

        public ExecutionResult Execute(IExecutionContext executionContext, Keys key)
        {
            var selectedNode = this._hierarchyControl.GetSelectedItem();

            if (IsFile(selectedNode)) {
                TraverseNode(selectedNode, (n) => true, ExpandNode, null);
            }
            else {
                TraverseNode(
                    selectedNode,
                    node => IsDirectory(node) || !IsFile(node) && !IsDirectory(node),
                    ExpandNode,
                    null
                );
            }

            return new ExecutionResult(executionContext.Clear(), CommandState.Handled);
        }
    }
}