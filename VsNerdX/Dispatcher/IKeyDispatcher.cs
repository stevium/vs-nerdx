namespace VsNerdX.Dispatcher
{
    using System;

    public interface IKeyDispatcher
    {
        bool Dispatch(int nCode, IntPtr wParam, IntPtr lParam);
    }
}