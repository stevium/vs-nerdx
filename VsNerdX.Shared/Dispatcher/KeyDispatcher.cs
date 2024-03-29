﻿using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace VsNerdX.Dispatcher
{
    public class KeyDispatcher : IKeyDispatcher
    {
        private readonly IKeyDispatcherTarget target;

        public KeyDispatcher(IKeyDispatcherTarget target)
        {
            this.target = target;
        }

        public bool Dispatch(int nCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                var handled = false;
                if (nCode == NativeMethods.HC_ACTION && KeyDown(lParam))
                {
                    double vkCode = wParam.ToInt64();

                    var key = (Keys)vkCode;
                    if (IsModifier(key))
                    {
                        // Let modifiers pass through
                        handled = false;
                    }
                    else
                    {
                        key = AddModifierState(key);
                        Debug.Print($"Dispatching Key = {key}");
                        handled = this.target.OnKey(key);
                    }
                }

                return handled;
            }
            catch
            {
                throw;
            }
        }

        private static bool KeyDown(IntPtr lParam)
        {
            var bits = lParam.ToInt64();
            return ((bits & NativeMethods.TRANSITION_STATE_BIT) == 0);
        }

        private static bool IsModifier(Keys key)
        {
            return
                key == Keys.ShiftKey ||
                key == Keys.ControlKey ||
                key == Keys.Menu;
        }

        private static Keys AddModifierState(Keys key)
        {
            key |= (NativeMethods.GetKeyState(NativeMethods.VK_SHIFT) & NativeMethods.HIGHORDER_KEYSTATE_BIT) > 0 ? Keys.Shift : 0;
            key |= (NativeMethods.GetKeyState(NativeMethods.VK_CONTROL) & NativeMethods.HIGHORDER_KEYSTATE_BIT) > 0 ? Keys.Control : 0;
            key |= (NativeMethods.GetKeyState(NativeMethods.VK_ALT) & NativeMethods.HIGHORDER_KEYSTATE_BIT) > 0 ? Keys.Alt : 0;

            return key;
        }
    }
}