using EnvDTE;
using Microsoft.VisualStudio.CommandBars;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using EnvDTE80;
using VsNerdX.Core;

namespace VsNerdX.Command.Navigation
{
    public class PreviewFile : ICommand
    {
        private readonly IHierarchyControl _hierarchyControl;
        private DTE2 _dte;
        private Timer _timer;

        public PreviewFile(IHierarchyControl hierarchyControl)
        {
            this._hierarchyControl = hierarchyControl;
        }

        public ExecutionResult Execute(IExecutionContext executionContext, Keys key)
        {
            _dte = ((HierarchyControl) this._hierarchyControl).dte;

            try
            {
                _dte.ExecuteCommand("SolutionExplorer.ToggleSingleClickPreview");
                _dte.ExecuteCommand("SolutionExplorer.ToggleSingleClickPreview");
            }
            catch (Exception e)
            {
                this._hierarchyControl.GoUp();
                _dte.ExecuteCommand("SolutionExplorer.Folder.ToggleSingleClickPreview");
                this._hierarchyControl.GoDown();
                _timer = new Timer();
                _timer.Interval = 500;
                _timer.Start();
                _timer.Tick += Timer_Tick;
            }

            executionContext = executionContext.Clear().With(mode: InputMode.Normal);
            return new ExecutionResult(executionContext, CommandState.Handled);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _dte.ExecuteCommand("SolutionExplorer.Folder.ToggleSingleClickPreview");
            _timer.Stop();
        }
    }
}
