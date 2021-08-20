using System.Windows.Forms;

namespace VsNerdX.Command
{
    public interface ICommand
    {
        ExecutionResult Execute(IExecutionContext executionContext, Keys key);
    }
}