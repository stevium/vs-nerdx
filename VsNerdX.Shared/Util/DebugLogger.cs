using System.Diagnostics;

namespace VsNerdX.Util
{
    public class DebugLogger : ILogger
    {
        public void Log(string message)
        {
            Debug.Print(message);
        }
    }
}
