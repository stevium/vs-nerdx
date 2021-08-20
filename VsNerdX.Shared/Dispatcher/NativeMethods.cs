namespace VsNerdX.Dispatcher
{
    using System;
    using System.Runtime.InteropServices;

    internal static class NativeMethods
    {
        internal const int WH_KEYBOARD = 2;

        internal const int VK_SHIFT = 0x10;
        internal const int VK_CONTROL = 0x11;
        internal const int VK_ALT = 0x12;

        internal const int HC_ACTION = 0;
        internal const int TRANSITION_STATE_BIT = (0x1 << 31);
        internal const int HIGHORDER_KEYSTATE_BIT = 0x8000;

        internal delegate IntPtr KeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr SetWindowsHookEx(int idHook, KeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern uint GetCurrentThreadId();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern ushort GetKeyState(int vkKey);
    }
}