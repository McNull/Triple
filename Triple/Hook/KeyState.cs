using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Triple.Hook
{
    public static class KeyState
    {
        [Flags]
        private enum KeyStates
        {
            None = 0,
            Down = 1,
            Toggled = 2
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern short GetKeyState(int keyCode);

        private static KeyStates GetKeyState(Keys key)
        {
            KeyStates state = KeyStates.None;

            //var t = GetKeyState(0x12);

            short retVal = GetKeyState((int)key);

            //If the high-order bit is 1, the key is down
            //otherwise, it is up.
            if ((retVal & 0x8000) == 0x8000)
                state |= KeyStates.Down;

            //If the low-order bit is 1, the key is toggled.
            if ((retVal & 1) == 1)
                state |= KeyStates.Toggled;

            return state;
        }

        public static bool IsAltDown
        {
            get { return (GetKeyState(0x12) & 0x8000) == 0x8000; }
        }

        public static bool IsShiftDown
        {
            get { return (GetKeyState(0x10) & 0x8000) == 0x8000; }
        }

        public static bool IsControlDown
        {
            get { return (GetKeyState(0x11) & 0x8000) == 0x8000; }
        }

        public static bool IsWinDown
        {
            get
            {
                return (GetKeyState(0x5B) & 0x8000) == 0x8000 ||
                         (GetKeyState(0x5C) & 0x8000) == 0x8000;
            }
        }


        public static bool IsKeyDown(Keys key)
        {
            return KeyStates.Down == (GetKeyState(key) & KeyStates.Down);
        }

        public static bool IsKeyToggled(Keys key)
        {
            return KeyStates.Toggled == (GetKeyState(key) & KeyStates.Toggled);
        }
    }
}
