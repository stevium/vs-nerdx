using System;
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
                    try
                    {
                        var result = command.Execute(this.executionContext, key);
                        this.executionContext = result.ExecutionContext;
                        handledKey = result.State == CommandState.Handled;
                    } catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                } else if (this.executionContext.Mode == InputMode.Yank || this.executionContext.Mode == InputMode.Go)
                {
                    handledKey = true;
                    this.executionContext = this.executionContext.Clear();
                }
            }

            this.logger.Log($"{key} handled = {handledKey}");
            return handledKey;
        }

        private void InitializeCommands()
        {
            commands.Add(new CommandKey(InputMode.Normal, Keys.N), new AddItem(this._hierarchyControl));
            commands.Add(new CommandKey(InputMode.Normal, Keys.Tab), new LeaveNerdx(this._hierarchyControl));

            commands.Add(new CommandKey(InputMode.Normal, Keys.X), new CloseParentNode(this._hierarchyControl));
            commands.Add(new CommandKey(InputMode.Normal, Keys.O), new OpenOrCloseNode(this._hierarchyControl));
            commands.Add(new CommandKey(InputMode.Normal, Keys.O | Keys.Shift), new OpenNodeRecursively(this._hierarchyControl));
            commands.Add(new CommandKey(InputMode.Normal, Keys.X | Keys.Shift), new CloseNodeRecursively(this._hierarchyControl));

            commands.Add(new CommandKey(InputMode.Normal, Keys.J), new GoDown(this._hierarchyControl));
            commands.Add(new CommandKey(InputMode.Normal, Keys.J | Keys.Shift), new GoToLastChild(this._hierarchyControl));
            commands.Add(new CommandKey(InputMode.Normal, Keys.K), new GoUp(this._hierarchyControl));
            commands.Add(new CommandKey(InputMode.Normal, Keys.K | Keys.Shift), new GoToFirtsChild(this._hierarchyControl));
            commands.Add(new CommandKey(InputMode.Normal, Keys.P | Keys.Shift), new GoToParent(this._hierarchyControl));
            commands.Add(new CommandKey(InputMode.Normal, Keys.G | Keys.Shift), new GoToBottom(this._hierarchyControl));

            commands.Add(new CommandKey(InputMode.Normal, Keys.G), new EnterGoMode(CommandState.Handled));
            commands.Add(new CommandKey(InputMode.Go, Keys.G), new GoToTop(this._hierarchyControl));
            commands.Add(new CommandKey(InputMode.Go, Keys.O), new PreviewFile(this._hierarchyControl));
            commands.Add(new CommandKey(InputMode.Normal, Keys.I), new OpenSplit(this._hierarchyControl));
            commands.Add(new CommandKey(InputMode.Normal, Keys.S), new OpenVSplit(this._hierarchyControl));
            commands.Add(new CommandKey(InputMode.Normal, Keys.I | Keys.Shift), new ShowAllFiles(this._hierarchyControl));

            commands.Add(new CommandKey(InputMode.Normal, Keys.D), new Delete(this._hierarchyControl));
            commands.Add(new CommandKey(InputMode.Normal, Keys.C), new CutFile(this._hierarchyControl));
            commands.Add(new CommandKey(InputMode.Normal, Keys.P), new Paste(this._hierarchyControl));
            commands.Add(new CommandKey(InputMode.Normal, Keys.R), new Rename(this._hierarchyControl));
            commands.Add(new CommandKey(InputMode.Normal, Keys.Y), new EnterYankMode(CommandState.Handled));
            commands.Add(new CommandKey(InputMode.Yank, Keys.Y), new CopyFile(this._hierarchyControl));
            commands.Add(new CommandKey(InputMode.Yank, Keys.P), new CopyPath(this._hierarchyControl));
            commands.Add(new CommandKey(InputMode.Yank, Keys.W), new CopyText(this._hierarchyControl));
            
            commands.Add(new CommandKey(InputMode.Normal, Keys.Divide), new EnterFindMode(CommandState.Handled));
            commands.Add(new CommandKey(InputMode.Normal, Keys.OemQuestion), new EnterFindMode(CommandState.Handled));
            commands.Add(new CommandKey(InputMode.Normal, Keys.Escape), new ClearExecutionStack());
            commands.Add(new CommandKey(InputMode.Normal, Keys.Oem2 | Keys.Shift), new ToggleHelp(this._hierarchyControl));
            commands.Add(new CommandKey(InputMode.Find, Keys.Escape), new LeaveFindMode());
        }

    }
}