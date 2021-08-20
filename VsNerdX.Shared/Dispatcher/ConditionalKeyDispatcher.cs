using System;
using VsNerdX.Dispatcher;
using VsNerdX.Util;

namespace VsNerdX.Dispatcher
{
    public class ConditionalKeyDispatcher : IDisposable
    {
        private static NativeMethods.KeyboardProc proc = KeyboardProc;
        private static ConditionalKeyDispatcher activeInstance = null;

        private readonly IDispatchCondition condition;
        private readonly IKeyDispatcher dispatcher;
        private IntPtr hookId = IntPtr.Zero;

        public ConditionalKeyDispatcher(IDispatchCondition condition, IKeyDispatcher dispatcher, DebugLogger logger)
        {
            if (ConditionalKeyDispatcher.activeInstance != null)
            {
                ConditionalKeyDispatcher.activeInstance.Dispose();
            }

            ConditionalKeyDispatcher.activeInstance = this;
            this.condition = condition;
            this.dispatcher = dispatcher;
            this.hookId = InstallHook(proc);
            this.logger = logger;
            logger.Log($"Hook installed {this.hookId}");
        }

        private static IntPtr InstallHook(NativeMethods.KeyboardProc keyboardProc)
        {
            return NativeMethods.SetWindowsHookEx(NativeMethods.WH_KEYBOARD, keyboardProc, IntPtr.Zero, NativeMethods.GetCurrentThreadId());
        }

        private static IntPtr KeyboardProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            return activeInstance.InternalKeyboardProc(nCode, wParam, lParam);
        }

        private IntPtr InternalKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                logger.Log($"Hook {nCode}, {wParam}, {lParam}");
                return this.condition.ShouldDispatch && this.dispatcher.Dispatch(nCode, wParam, lParam)
                    ? (IntPtr)1
                    : NativeMethods.CallNextHookEx(this.hookId, nCode, wParam, lParam);
            }
            catch (Exception e)
            {
                logger.Log($"Exception in KeyboardProc, removing hook. Exception = {e}");
                var result = NativeMethods.CallNextHookEx(this.hookId, nCode, wParam, lParam);
                this.RemoveHook();
                return result;
            }
        }

        private void RemoveHook()
        {
            if (this.hookId != IntPtr.Zero)
            {
                NativeMethods.UnhookWindowsHookEx(this.hookId);
                this.hookId = IntPtr.Zero;
            }
        }
        #region IDisposable Support

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                this.RemoveHook();
                disposedValue = true;
            }
        }

        private bool disposedValue = false;
        private DebugLogger  logger;

        ~ConditionalKeyDispatcher()
        {
            Dispose(false);
        }
        #endregion IDisposable Support
    }
}