using System;
using System.Windows.Forms;
using VsNerdX.Core;

namespace VsNerdX.Command.Navigation
{
    public class GoDown : ICommand
    {
        private readonly IHierarchyControl _hierarchyControl;

        public GoDown(IHierarchyControl hierarchyControl)
        {
            this._hierarchyControl = hierarchyControl;
        }

        public ExecutionResult Execute(IExecutionContext executionContext, Keys key)
        {
            try
            {
                if (((HierarchyControl) this._hierarchyControl).GetHierarchyListBox().IsKeyboardFocusWithin)
                {
                    this._hierarchyControl.GoDown();
                }
                else
                {
                    ((HierarchyControl) this._hierarchyControl).helpViewControl.LineDown();
                }
            }
            catch (Exception e) { }

            return new ExecutionResult(executionContext.Clear(), CommandState.Handled);
        }
    }
}