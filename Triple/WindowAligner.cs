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
            // Check if wnd is resizeable

            if(hWnd == IntPtr.Zero)
            {
                hWnd = GetForegroundWindow();
            }

            int areaWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int areaHeight = Screen.PrimaryScreen.WorkingArea.Height;
            int windowWidth = areaWidth / 3;
            int marginLeft = alignment > 0 ? ((areaWidth % windowWidth) >> 1) * (int)alignment : 0;
            int xPos = windowWidth * (int)alignment + marginLeft;

            //Console.WriteLine("WorkingArea: {0}x{1}", areaWidth, areaHeight);
            //Console.WriteLine("Window: {0}x{1}", windowWidth, areaHeight);
            //Console.WriteLine("Margin left: {0}", marginLeft);
            //Console.WriteLine("Position X: {0}", xPos);

            MoveWindow(hWnd, xPos, 0, windowWidth, areaHeight, true);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
    }
}
