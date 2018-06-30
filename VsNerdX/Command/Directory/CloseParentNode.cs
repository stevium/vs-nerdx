using System.Windows.Forms;
using VsNerdX.Core;

namespace VsNerdX.Command.Directory
{
    public class CloseParentNode : ICommand
    {
        private readonly IHierarchyControl _hierarchyControl;

        public CloseParentNode(IHierarchyControl hierarchyControl)
        {
            this._hierarchyControl = hierarchyControl;
        }

        public ExecutionResult Execute(IExecutionContext executionContext, Keys key)
        {
            this._hierarchyControl.CloseParentNode();
            return new ExecutionResult(executionContext.Clear(), CommandState.Handled);
        }
    }
}