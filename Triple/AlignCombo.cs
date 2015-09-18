using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Triple.Hook;

namespace Triple
{
    public class AlignCombo : KeyCombo
    {
        public AlignCombo(WindowAlignment alignment, System.Windows.Forms.Keys keys, KeyModifier modifier = KeyModifier.None) : base(keys, modifier)
        {
            this.Alignment = alignment;
        }

        public WindowAlignment Alignment { get; private set; }
    }
}
