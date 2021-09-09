using System.Windows.Forms;

namespace VsNerdX.Command.General
{
    public class ClearExecutionStack : ICommand
    {
        public ExecutionResult Execute(IExecutionContext executionContext, Keys key)
        {
            return new ExecutionResult(executionContext.Clear().With(mode: InputMode.Normal), CommandState.Handled);
        }
    }
}