using System.Windows.Forms;

namespace VsNerdX.Command.General
{
    public class EnterFindMode : ICommand
    {
        private readonly CommandState handledState;

        public EnterFindMode(CommandState handledState)
        {
            this.handledState = handledState;
        }

        public ExecutionResult Execute(IExecutionContext executionContext, Keys key)
        {
            return new ExecutionResult(executionContext.Clear().With(mode: InputMode.Find), this.handledState);
        }
    }
}