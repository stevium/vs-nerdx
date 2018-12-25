using System.Windows.Forms;

namespace VsNerdX.Command.General
{
    public class EnterGoMode : ICommand
    {
        private readonly CommandState handledState;

        public EnterGoMode(CommandState handledState)
        {
            this.handledState = handledState;
        }

        public ExecutionResult Execute(IExecutionContext executionContext, Keys key)
        {
            return new ExecutionResult(executionContext.Clear().With(mode: InputMode.Go), this.handledState);
        }
    }
}