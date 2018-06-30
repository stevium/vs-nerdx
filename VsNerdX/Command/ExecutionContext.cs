using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace VsNerdX.Command
{
    public class ExecutionContext : IExecutionContext
    {
        public ExecutionContext()
        {
            this.DeferredExecutable = null;
            this.Mode = InputMode.Normal;
            this.Stack = new List<Keys>();
        }

        public InputMode Mode { get; }
        
        public IReadOnlyCollection<Keys> Stack { get; }

        public ICommand DeferredExecutable { get; }

        public IExecutionContext Add(Keys key)
        {
            return this.With(stack: this.Stack.Concat(new[] { key }));
        }


        public IExecutionContext With(InputMode? mode = null, ICommand delayedExecutable = null, IEnumerable<Keys> stack = null)
        {
            var changed =
                (mode != null && mode.Value != this.Mode) ||
                (delayedExecutable != null && !object.ReferenceEquals(delayedExecutable, this.DeferredExecutable) ||
                (stack != null && !object.ReferenceEquals(stack, this.Stack)));

            return changed
                ? new ExecutionContext(delayedExecutable ?? this.DeferredExecutable, mode ?? this.Mode, stack ?? this.Stack)
                : this;
        }

        public IExecutionContext Clear()
        {
            return new ExecutionContext();
        }

        private ExecutionContext(ICommand delayedExecutable, InputMode mode, IEnumerable<Keys> stack)
        {
            this.Mode = mode;
            this.DeferredExecutable = delayedExecutable;
            this.Stack = stack.ToList();
        }
    }
}