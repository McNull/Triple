using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Triple
{
    public enum WindowAlignment : int
    {
        Left = 0, Middle = 1, Right = 2
    }

    public class WindowAligner
    {
        public static void Align(WindowAlignment alignment, IntPtr hWnd)
        {
            if(hWnd == IntPtr.Zero)
            {
                hWnd = GetForegroundWindow();
            }

            var style = GetWindowLong(hWnd, GWL_STYLE);
            
            if((style & WS_SIZEBOX) == WS_SIZEBOX)
            {
                int areaWidth = Screen.PrimaryScreen.WorkingArea.Width;
                int areaHeight = Screen.PrimaryScreen.WorkingArea.Height;
                int windowWidth = areaWidth / 3;
                int marginLeft = alignment > 0 ? ((areaWidth % windowWidth) >> 1) * (int)alignment : 0;
                int xPos = windowWidth * (int)alignment + marginLeft;

                //Console.WriteLine("WorkingArea: {0}x{1}", areaWidth, areaHeight);
                //Console.WriteLine("Window: {0}x{1}", windowWidth, areaHeight);
                //Console.WriteLine("Margin left: {0}", marginLeft);
                //Console.WriteLine("Style: {0}", style);
                //Console.WriteLine("Position X: {0}", xPos);

                MoveWindow(hWnd, xPos, 0, windowWidth, areaHeight, true);
            }

            
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        private const int GWL_STYLE = -16;
        private const int WS_SIZEBOX = 0x00040000;
    }
}
