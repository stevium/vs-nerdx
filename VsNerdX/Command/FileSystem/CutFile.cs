using System;
using System.Linq;
using System.Windows.Forms;
using VsNerdX.Core;

namespace VsNerdX.Command.Navigation
{
    public class CutFile : ICommand
    {
        private readonly IHierarchyControl _hierarchyControl;

        public CutFile(IHierarchyControl hierarchyControl)
        {
            this._hierarchyControl = hierarchyControl;
        }

        public ExecutionResult Execute(IExecutionContext executionContext, Keys key)
        {
            var dte = (this._hierarchyControl as HierarchyControl).dte;
            var state = CommandState.Handled;
            if (executionContext.Stack.Count > 0 && executionContext.Stack.Last() == Keys.C && key == Keys.C)
            {
                try
                {
                    dte.ExecuteCommand("Edit.Cut");
                }
                catch (Exception e) { }

                executionContext = executionContext.Clear();
            }
            else if (key == Keys.C)
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
