using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Triple.Hook;

namespace Triple
{
    public class TripleSysTray : Form
    {
        private NotifyIcon trayIcon;
        private ContextMenu trayMenu;

        public KeyMonitor KeyMonitor { get; private set; }

        public TripleSysTray()
        {
            this.trayMenu = new ContextMenu();
            this.trayMenu.MenuItems.Add("Exit", this.OnExit);
            this.trayIcon = new NotifyIcon();
            this.trayIcon.Text = "Triple";
            this.trayIcon.Icon = new Icon(SystemIcons.WinLogo, 40, 40);

            // Add menu to tray icon and show it.
            this.trayIcon.ContextMenu = trayMenu;
            this.trayIcon.Visible = true;

            this.KeyMonitor = new KeyMonitor();
        }

        protected override void OnLoad(EventArgs e)
        {
            this.Visible = false; // Hide form window.
            this.ShowInTaskbar = false; // Remove from taskbar.

            this.RegisterHotkeys();

            base.OnLoad(e);
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // Release the icon resource.
                this.trayIcon.Dispose();
                this.KeyMonitor.Dispose();
            }

            base.Dispose(isDisposing);
        }

        // ---------------------------------------------------------------

        private void RegisterHotkeys()
        {
            this.KeyMonitor.KeyCombos.Add(new AlignCombo(WindowAlignment.Left, Keys.Left, KeyModifier.Win | KeyModifier.Alt));
            this.KeyMonitor.KeyCombos.Add(new AlignCombo(WindowAlignment.Middle, Keys.Up, KeyModifier.Win | KeyModifier.Alt));
            this.KeyMonitor.KeyCombos.Add(new AlignCombo(WindowAlignment.Right, Keys.Right, KeyModifier.Win | KeyModifier.Alt));

            this.KeyMonitor.OnKeyComboPressed += (object s, KeyComboPressEventArgs e) =>
            {
                var alignment = ((AlignCombo)e.KeyCombo).Alignment;
                WindowAligner.Align(alignment, IntPtr.Zero);

                e.Continue = false;
            };
        }

        
        // ---------------------------------------------------------------

        
    }
}
