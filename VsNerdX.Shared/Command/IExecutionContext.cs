using System.Collections.Generic;
using System.Windows.Forms;

namespace VsNerdX.Command
{
    public interface IExecutionContext
    {
        /// <summary>
        /// Gets the current deferred command if any.
        /// </summary>
        ICommand DeferredExecutable { get; }

        /// <summary>
        /// Gets the current input mode.
        /// </summary>
        InputMode Mode { get; }

        /// <summary>
        /// Gets the current input stack.
        /// </summary>
        IReadOnlyCollection<Keys> Stack { get; }

        /// <summary>
        /// Adds a key the the input stack.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IExecutionContext Add(Keys key);

        /// <summary>
        /// Creates a new blank executionContext. 
        /// </summary>
        /// <returns></returns>
        IExecutionContext Clear();

        /// <summary>
        /// Creates a new executionContext with the non-null parameters updated. 
        /// </summary>
        /// <param name="mode">If specified, sets the value of <see cref="IExecutionContext.Mode"/></param>
        /// <param name="delayedExecutable">If specified, sets the value of <see cref="DeferredExecutable"/></param>
        /// <param name="stack">If specified, sets the value of <see cref="IExecutionContext.Stack"/></param>
        /// <returns>New executionContext with the specified parameters updated.</returns>
        IExecutionContext With(
            InputMode? mode = null, 
            ICommand delayedExecutable = null, 
            IEnumerable<Keys> stack = null);
    }
}