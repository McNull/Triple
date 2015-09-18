using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Triple.Hook
{
    public class KeyCombo
    {
        public KeyCombo(Keys key, KeyModifier modifier = KeyModifier.None)
        {
            this.Key = key;
            this.Modifier = modifier;
        }

        public Keys Key { get; private set; }
        public KeyModifier Modifier { get; private set; }
    }

    /// <summary>
    /// The bits of the LWin and RWin values in the Keys enum overlap the normal key values.  
    /// </summary>
    public enum KeyModifier : uint
    {
        None = 0,
        Alt = 1,
        Control = 2,
        Shift = 4,
        Win = 8
    }
}


