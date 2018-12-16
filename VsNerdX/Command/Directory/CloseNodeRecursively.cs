using System.Windows.Forms;
using VsNerdX.Core;
using static VsNerdX.Util.TreeHelper;

namespace VsNerdX.Command.Directory
{
    public class CloseNodeRecursively : ICommand
    {
        private readonly IHierarchyControl _hierarchyControl;

        public CloseNodeRecursively(IHierarchyControl hierarchyControl)
        {
            this._hierarchyControl = hierarchyControl;
        }

        public ExecutionResult Execute(IExecutionContext executionContext, Keys key)
        {
            var selectedNode = this._hierarchyControl.GetSelectedItem();

            if (IsFile(selectedNode)) {
                TraverseNode(selectedNode, (n) => true, CollapseNode, null);
            }
            else {
                TraverseNode(
                    selectedNode,
                    node => IsDirectory(node) || !IsFile(node) && !IsDirectory(node),
                    CollapseNode,
                    null
                );
            }

            return new ExecutionResult(executionContext.Clear(), CommandState.Handled);
        }
    }
}