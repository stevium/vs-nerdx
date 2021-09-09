using System;
using System.Windows.Forms;

namespace VsNerdX.Command
{

    internal sealed class CommandKey : IEquatable<CommandKey>
    {
        internal CommandKey(InputMode mode, Keys key)
        {
            this.Mode = mode;
            this.Key = key;
        }

        internal InputMode Mode { get; }
        internal Keys Key { get; }

        public override int GetHashCode()
        {
            return this.Mode.GetHashCode() ^ this.Key.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return this.Equals(obj as CommandKey);
        }

        public bool Equals(CommandKey other)
        {
            if (other == null)
            {
                return false;
            }

            return this.Mode == other.Mode && this.Key == other.Key;
        }
    }
}