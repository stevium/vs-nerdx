using System;
using System.Linq;
using System.Windows.Forms;
using VsNerdX.Core;

namespace VsNerdX.Command.Navigation
{
    public class Delete : ICommand
    {
        private readonly IHierarchyControl _hierarchyControl;

        public Delete(IHierarchyControl hierarchyControl)
        {
            this._hierarchyControl = hierarchyControl;
        }

        public ExecutionResult Execute(IExecutionContext executionContext, Keys key)
        {
            var dte = (this._hierarchyControl as HierarchyControl).dte;
            var state = CommandState.Handled;
            if (executionContext.Stack.Count > 0 && executionContext.Stack.Last() == Keys.D && key == Keys.D)
            {
                try
                {
                    dte.ExecuteCommand("Edit.Delete");
                }
                catch (Exception e) { }

                executionContext = executionContext.Clear();
            }
            else if (key == Keys.D)
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