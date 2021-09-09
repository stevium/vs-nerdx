using System.Windows.Forms;

namespace VsNerdX.Command.General
{
    public class EnterYankMode : ICommand
    {
        private readonly CommandState handledState;

        public EnterYankMode(CommandState handledState)
        {
            this.handledState = handledState;
        }

        public ExecutionResult Execute(IExecutionContext executionContext, Keys key)
        {
            return new ExecutionResult(executionContext.Clear().With(mode: InputMode.Yank), this.handledState);
        }
    }
}