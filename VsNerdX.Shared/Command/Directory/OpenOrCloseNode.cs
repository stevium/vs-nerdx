using System.Windows.Forms;
using VsNerdX.Core;

namespace VsNerdX.Command.Directory
{
    public class OpenOrCloseNode : ICommand
    {
        private readonly IHierarchyControl _hierarchyControl;

        public OpenOrCloseNode(IHierarchyControl hierarchyControl)
        {
            this._hierarchyControl = hierarchyControl;
        }

        public ExecutionResult Execute(IExecutionContext executionContext, Keys key)
        {
            this._hierarchyControl.OpenOrCloseNode();
            return new ExecutionResult(executionContext.Clear(), CommandState.Handled);
        }
    }
}