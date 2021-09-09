using System;
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
            try
            {
                if (((HierarchyControl) this._hierarchyControl).GetHierarchyListBox().IsKeyboardFocusWithin)
                {
                    this._hierarchyControl.GoUp();
                }
                else
                {
                    ((HierarchyControl) this._hierarchyControl).helpViewControl.LineUp();
                }
            }
            catch (Exception e) { }

            return new ExecutionResult(executionContext.Clear(), CommandState.Handled);
        }
    }
}