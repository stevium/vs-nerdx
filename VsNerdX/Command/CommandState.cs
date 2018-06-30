namespace VsNerdX.Command
{

    /// <summary>
    /// Provides information if the command should be considered as handled.
    /// </summary>
    public enum CommandState
    {
        /// <summary>
        /// Find has been cleared, should check for another command for the key.
        /// </summary>
        Cleared,

        /// <summary>
        /// The command has handled the key, no further processing should be done.
        /// </summary>
        Handled,

        /// <summary>
        /// The command has handled the key, key should still be passed through to VS.
        /// </summary>
        PassThrough
    }
}