using System.Windows.Forms;
using VsNerdX.Core;

namespace VsNerdX.Command.Navigation
{
    public class GoToFirtsChild : ICommand
    {
        private IHierarchyControl _hierarchyControl;

        public GoToFirtsChild(IHierarchyControl hierarchyControl)
        {
            this._hierarchyControl = hierarchyControl;
        }

        public ExecutionResult Execute(IExecutionContext executionContext, Keys key)
        {
            this._hierarchyControl.GoToFirstChild();
            return new ExecutionResult(executionContext.Clear(), CommandState.Handled);
        }
    }
}