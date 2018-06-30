using System.Linq;
using System.Windows.Forms;
using VsNerdX.Core;

namespace VsNerdX.Command.Navigation
{
    public class GoToTop : ICommand
    {
        private IHierarchyControl _hierarchyControl;

        public GoToTop(IHierarchyControl hierarchyControl)
        {
            this._hierarchyControl = hierarchyControl;
        }

        public ExecutionResult Execute(IExecutionContext executionContext, Keys key)
        {
            var state = CommandState.Handled;
            if (executionContext.Stack.Count > 0 && executionContext.Stack.Last() == Keys.G && key == Keys.G)
            {
                this._hierarchyControl.GoToTop();
                executionContext = executionContext.Clear();
            }
            else if (key == Keys.G)
            {
                executionContext = executionContext.Add(key).With(delayedExecutable: this);
            }
            else
            {
                executionContext = executionContext.Clear();
                state = CommandState.Cleared;
            }

            return new ExecutionResult(executionContext, state);
        }
    }
}