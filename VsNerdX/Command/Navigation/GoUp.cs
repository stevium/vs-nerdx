using System.Windows.Forms;
using VsNerdX.Core;

namespace VsNerdX.Command.Navigation
{
    public class GoUp : ICommand
    {
        private readonly IHierarchyControl _hierarchyControl;

        public GoUp(IHierarchyControl hierarchyControl)
        {
            this._hierarchyControl = hierarchyControl;
        }

        public ExecutionResult Execute(IExecutionContext executionContext, Keys key)
        {
            this._hierarchyControl.GoUp();
            return new ExecutionResult(executionContext.Clear(), CommandState.Handled);
        }
    }
}