using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Windows_Modification.Windows
{
    public partial class D_Windows : Form
    {
        public D_Windows()
        {
            InitializeComponent();
        }
        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(POINT Point);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);
        private void timer1_Tick(object sender, EventArgs e)
        {
            POINT point;
            GetCursorPos(out point);
            IntPtr hWnd = WindowFromPoint(point);
            RECT rect;
            GetWindowRect(hWnd, out rect);
            TopMost = true;
           /* Console.WriteLine("窗口位置：({0}, {1})", rect.Left, rect.Top);
            Console.WriteLine("窗口大小：{0} x {1}", rect.Right - rect.Left, rect.Bottom - rect.Top);*/

            Location =new Point(rect.Left, rect.Top);
            Width = rect.Right - rect.Left;
            Height = rect.Bottom - rect.Top;
            bool ctrlPressed = (GetAsyncKeyState(0x11) & 0x8000) != 0; // 0x11 是Ctrl键的虚拟键码
            bool altPressed = (GetAsyncKeyState(0x12) & 0x8000) != 0; // 0x12 是Alt键的虚拟键码
            bool qPressed = (GetAsyncKeyState(0x51) & 0x8000) != 0; // 0x51 是Q键的虚拟键码

            IntPtr foregroundWindow = GetForegroundWindow();
            POINT cursorPos;
            GetCursorPos(out cursorPos);
            ScreenToClient(foregroundWindow, ref cursorPos);

            if ((GetAsyncKeyState(0x01) & 0x8000) != 0)
            {
                timer1.Enabled= false;
                Console.WriteLine("左键被单击,目标窗口句柄："+hWnd.ToString());
                File.WriteAllText("Config.txt",hWnd.ToString());
                Thread.Sleep(100);
                Close();
            }
        }
    }
}
