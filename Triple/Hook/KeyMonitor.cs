using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Triple.Hook
{
    public class KeyMonitor : IDisposable
    {
        public KeyMonitor()
        {
            this.KeyCombos = new List<KeyCombo>();
            this.Hook();
        }

        public List<KeyCombo> KeyCombos { get; private set; }

        public void Dispose()
        {
            this.Unhook();
        }

        // ------------------------------------------------------

        private IntPtr _hhook = IntPtr.Zero;

        private void Hook()
        {
            this.Unhook();

            IntPtr hInstance = LoadLibrary("User32");
            this._hhook = SetWindowsHookEx(WH_KEYBOARD_LL, this.HookProc, hInstance, 0);
        }

        private void Unhook()
        {
            if(this._hhook != IntPtr.Zero)
            {
                UnhookWindowsHookEx(this._hhook);
                this._hhook = IntPtr.Zero;
            }
        }

        // ------------------------------------------------------

        private KeyComboPressEventArgs keyComboPressEventArgs = new KeyComboPressEventArgs();

        /// <summary>
        /// The callback for the keyboard hook
        /// </summary>
        /// <param name="code">The hook code, if it isn't >= 0, the function shouldn't do anyting</param>
        /// <param name="wParam">The event type</param>
        /// <param name="lParam">The keyhook event information</param>
        /// <returns></returns>
        private int HookProc(int code, int wParam, ref KeyboardHookStruct lParam)
        {   
            bool doContinue = true;
            var ret = 1;

            if (this.OnKeyComboPressed != null && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
            {
                var key = (Keys)lParam.vkCode;

                var keyCombos = this.KeyCombos.Where(x => x.Key == key);

                if(keyCombos.Count() > 0)
                {
                    KeyModifier modifier = KeyModifier.None;

                    if (KeyState.IsControlDown)
                    {
                        modifier |= KeyModifier.Control;
                    }

                    if(KeyState.IsShiftDown)
                    {
                        modifier |= KeyModifier.Shift;
                    }

                    if (KeyState.IsWinDown)
                    {
                        modifier |= KeyModifier.Win;
                    }

                    if (KeyState.IsAltDown)
                    {
                        modifier |= KeyModifier.Alt;
                    }

                    doContinue = keyCombos.Where(x => x.Modifier == modifier)
                    .All(x =>
                    {
                        this.keyComboPressEventArgs.Continue = true;
                        this.keyComboPressEventArgs.KeyCombo = x;
                        this.OnKeyComboPressed(this, this.keyComboPressEventArgs);
                        return this.keyComboPressEventArgs.Continue;
                    });
                }
            }

            if(doContinue)
                ret =  CallNextHookEx(this._hhook, code, wParam, ref lParam);

            return ret;
        }

        // ------------------------------------------------------

        public EventHandler<KeyComboPressEventArgs> OnKeyComboPressed;

        // ------------------------------------------------------

        #region Constant, Structure and Delegate Definitions

        private delegate int KeyboardHookProc(int code, int wParam, ref KeyboardHookStruct lParam);

        private struct KeyboardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;
        private const int WM_SYSKEYDOWN = 0x104;
        private const int WM_SYSKEYUP = 0x105;

        #endregion

        #region DLL imports

        /// <summary>
        /// Sets the windows hook, do the desired event, one of hInstance or threadId must be non-null
        /// </summary>
        /// <param name="idHook">The id of the event you want to hook</param>
        /// <param name="callback">The callback.</param>
        /// <param name="hInstance">The handle you want to attach the event to, can be null</param>
        /// <param name="threadId">The thread you want to attach the event to, can be null</param>
        /// <returns>a handle to the desired hook</returns>
        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, KeyboardHookProc callback, IntPtr hInstance, uint threadId);

        /// <summary>
        /// Unhooks the windows hook.
        /// </summary>
        /// <param name="hInstance">The hook handle that was returned from SetWindowsHookEx</param>
        /// <returns>True if successful, false otherwise</returns>
        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        /// <summary>
        /// Calls the next hook.
        /// </summary>
        /// <param name="idHook">The hook id</param>
        /// <param name="nCode">The hook code</param>
        /// <param name="wParam">The wparam.</param>
        /// <param name="lParam">The lparam.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref KeyboardHookStruct lParam);

        /// <summary>
        /// Loads the library.
        /// </summary>
        /// <param name="lpFileName">Name of the library</param>
        /// <returns>A handle to the library</returns>
        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string lpFileName);

        #endregion
    }
}
