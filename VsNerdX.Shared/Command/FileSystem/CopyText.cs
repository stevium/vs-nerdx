using System;
using System.Windows.Forms;
using VsNerdX.Core;
using VsNerdX.Util;

namespace VsNerdX.Command.Navigation
{
    public class CopyText : ICommand
    {
        private readonly IHierarchyControl _hierarchyControl;

        public CopyText(IHierarchyControl hierarchyControl)
        {
            _hierarchyControl = hierarchyControl;
        }

        public ExecutionResult Execute(IExecutionContext executionContext, Keys key)
        {
            string name = "";
            var selectedItem = _hierarchyControl.GetSelectedItem();
            try
            {
                name = (string) selectedItem.GetValue("Item").GetValue("SourceItem").GetValue("Text");
            }
            catch (NullReferenceException e)
            {
                name = (string) selectedItem.GetValue("Item").GetValue("Text");
            }

            if (name != null)
            {
                Clipboard.SetText(name);
            }

            executionContext = executionContext.Clear().With(mode: InputMode.Normal);
            return new ExecutionResult(executionContext, CommandState.Handled);
        }
    }
}
