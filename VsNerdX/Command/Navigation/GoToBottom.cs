using System.Windows.Forms;
using VsNerdX.Core;

namespace VsNerdX.Command.Navigation
{
    public class GoToBottom : ICommand
    {
        private IHierarchyControl _hierarchyControl;

        public GoToBottom(IHierarchyControl hierarchyControl)
        {
            this._hierarchyControl = hierarchyControl;
        }

        public ExecutionResult Execute(IExecutionContext executionContext, Keys key)
        {
            this._hierarchyControl.GoToBottom();
            return new ExecutionResult(executionContext.Clear(), CommandState.Handled);
        }
    }
}
