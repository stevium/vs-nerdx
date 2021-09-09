namespace VsNerdX.Command
{

    public class ExecutionResult
    {
        public ExecutionResult(IExecutionContext executionContext, CommandState state)
        {
            this.ExecutionContext = executionContext;
            this.State = state;
        }

        public IExecutionContext ExecutionContext { get; }
        public CommandState State { get; }
    }
}