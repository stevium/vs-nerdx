using System;
using System.Linq;
using System.Windows.Forms;
using VsNerdX.Core;
using static VsNerdX.VsNerdXPackage;

namespace VsNerdX.Command.Navigation
{
    public class AddItem : ICommand
    {
        private readonly IHierarchyControl _hierarchyControl;

        public AddItem(IHierarchyControl hierarchyControl)
        {
            this._hierarchyControl = hierarchyControl;
        }

        public ExecutionResult Execute(IExecutionContext executionContext, Keys key)
        {
            var state = CommandState.Handled;
            if (executionContext.Stack.Count > 0 && executionContext.Stack.Last() == Keys.N && key == Keys.C)
            {
                try
                {
                    Dte.ExecuteCommand("Project.AddClass");
                }
                catch (Exception e) { }

                executionContext = executionContext.Clear();
            }
            else if (executionContext.Stack.Count > 0 && executionContext.Stack.Last() == Keys.N && key == Keys.F)
            {
                try
                {
                    Dte.ExecuteCommand("Project.NewFolder");
                }
                catch (Exception e) { }

                executionContext = executionContext.Clear();
            }
            else if (executionContext.Stack.Count > 0 && executionContext.Stack.Last() == Keys.N && key == Keys.S)
            {
                try
                {
                    Dte.ExecuteCommand("ProjectAndSolutionContextMenus.Project.Add.NewScaffoldedItem");
                }
                catch (Exception e) { }

                executionContext = executionContext.Clear();
            }
            else if (key == Keys.N)
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
