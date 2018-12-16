using System.Collections.Generic;
using System.Windows.Forms;
using VsNerdX.Command;
using VsNerdX.Command.Directory;
using VsNerdX.Command.General;
using VsNerdX.Command.Navigation;
using VsNerdX.Dispatcher;
using VsNerdX.Util;

namespace VsNerdX.Core
{
    public class CommandProcessor : IKeyDispatcherTarget
    {
        private Dictionary<CommandKey, ICommand> commands = new Dictionary<CommandKey, ICommand>();
        private IExecutionContext executionContext;

        private readonly ILogger logger;
        private readonly IHierarchyControl _hierarchyControl;

        public CommandProcessor(IHierarchyControl hierarchyControl, ILogger logger, IExecutionContext initialExecutionContext = null)
        {
            this._hierarchyControl = hierarchyControl;
            this.logger = logger;
            this.executionContext = initialExecutionContext ?? new ExecutionContext();
            this.InitializeCommands();
        }

        public IExecutionContext ExecutionContext => this.executionContext;

        public bool OnKey(Keys key)
        {
            if (key == Keys.Return)
            {
                // Note: This is temporary, should be handled as other commands if we want to implement support for '/' search for instance.
                this.executionContext = this.executionContext.Clear();
                return false;
            }

            var handledKey = false;
            if (this.executionContext.DeferredExecutable != null)
            {
                var result = this.executionContext.DeferredExecutable.Execute(this.executionContext, key);
                this.executionContext = result.ExecutionContext;
                handledKey = result.State == CommandState.Handled;
            }

            if (!handledKey)
            {
                var commandKey = new CommandKey(this.executionContext.Mode, key);
                if (this.commands.TryGetValue(commandKey, out ICommand command))
                {
                    var result = command.Execute(this.executionContext, key);
                    this.executionContext = result.ExecutionContext;
                    handledKey = result.State == CommandState.Handled;
                }
            }

            this.logger.Log($"{key} handled = {handledKey}");
            return handledKey;
        }

        private void InitializeCommands()
        {
            commands.Add(new CommandKey(InputMode.Normal, Keys.X), new CloseParentNode(this._hierarchyControl));
            commands.Add(new CommandKey(InputMode.Normal, Keys.O), new OpenOrCloseNode(this._hierarchyControl));
            commands.Add(new CommandKey(InputMode.Normal, Keys.O | Keys.Shift), new OpenNodeRecursively(this._hierarchyControl));
            commands.Add(new CommandKey(InputMode.Normal, Keys.X | Keys.Shift), new CloseNodeRecursively(this._hierarchyControl));

            commands.Add(new CommandKey(InputMode.Normal, Keys.J), new GoDown(this._hierarchyControl));
            commands.Add(new CommandKey(InputMode.Normal, Keys.J | Keys.Shift), new GoToLastChild(this._hierarchyControl));
            commands.Add(new CommandKey(InputMode.Normal, Keys.K), new GoUp(this._hierarchyControl));
            commands.Add(new CommandKey(InputMode.Normal, Keys.K | Keys.Shift), new GoToFirtsChild(this._hierarchyControl));
            commands.Add(new CommandKey(InputMode.Normal, Keys.P), new GoToParent(this._hierarchyControl));
            commands.Add(new CommandKey(InputMode.Normal, Keys.P | Keys.Shift), new GoToRoot(this._hierarchyControl));
            commands.Add(new CommandKey(InputMode.Normal, Keys.G), new GoToTop(this._hierarchyControl));
            commands.Add(new CommandKey(InputMode.Normal, Keys.G | Keys.Shift), new GoToBottom(this._hierarchyControl));

            commands.Add(new CommandKey(InputMode.Normal, Keys.Oem2 | Keys.Shift), new ToggleHelp(this._hierarchyControl));
            commands.Add(new CommandKey(InputMode.Normal, Keys.Divide), new EnterFindMode(CommandState.Handled));

            // For now no need to enter Find mode when renaming. Intead use VS-s InRenameMode with ShouldDispatch
            //commands.Add(new CommandKey(InputMode.Normal, Keys.F2), new EnterFindMode(CommandState.PassThrough));

            // Note: Consider adding an option regarding the behavior of Escape in Normal mode
            // e.g should it clear the stack and stay in Normal mode, or should it fall through to the default behavior? 
            // Let's stick with the least disruptive mode for now.
            //commands.Add(new ExecutableKey(InputMode.Normal, Keys.Escape), new ClearExecutionStack());

            commands.Add(new CommandKey(InputMode.Find, Keys.Escape), new LeaveFindMode());
        }

    }
}