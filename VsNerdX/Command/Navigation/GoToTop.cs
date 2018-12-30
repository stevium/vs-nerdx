using System;
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
            try
            {
                if (((HierarchyControl) this._hierarchyControl).GetHierarchyListBox().IsKeyboardFocusWithin)
                {
                    this._hierarchyControl.GoToTop();
                }
                else
                {
                    ((HierarchyControl) this._hierarchyControl).helpViewControl.GoToTop();
                }
            }
            catch (Exception e) { }

            executionContext = executionContext.Clear().With(mode: InputMode.Normal);
            return new ExecutionResult(executionContext, CommandState.Handled);
        }
    }
}