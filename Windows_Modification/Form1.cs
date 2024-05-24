using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows_Modification.Windows;
using static Windows_Modification.Windows.D_Windows;

namespace Windows_Modification
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, string lpString, int nMaxCount);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern bool GetLayeredWindowAttributes(IntPtr hwnd, out uint crKey, out byte bAlpha, out uint dwFlags);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            D_Windows d_Windows = new D_Windows();
            d_Windows.ShowDialog();
            label1.Text = "目标窗口句柄:" + File.ReadAllText("Config.txt");
            this.Visible = true;
            tabControl1.Enabled = true;

            timer1.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        IntPtr hWnd= (IntPtr)0;
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        private void timer1_Tick(object sender, EventArgs e)
        {
            hWnd = (IntPtr)int.Parse(File.ReadAllText("Config.txt"));
            // 获取目标窗口的句柄
            IntPtr targetWindow = hWnd;

            // 获取窗口标题
            const int nChars = 256;
            string windowTitle = new string(' ', nChars);
            GetWindowText(targetWindow, windowTitle, nChars);

            // 获取窗口位置和大小
            RECT windowRect;
            GetWindowRect(targetWindow, out windowRect);
            int width = windowRect.Right - windowRect.Left;
            int height = windowRect.Bottom - windowRect.Top;

            // 获取窗口透明度
            uint crKey;
            byte bAlpha;
            uint dwFlags;
            GetLayeredWindowAttributes(targetWindow, out crKey, out bAlpha, out dwFlags);
            
            richTextBox1.Clear();
            richTextBox1.AppendText("窗口标题: " + windowTitle.Trim());
            richTextBox1.AppendText("\n窗口句柄: " + targetWindow);
            richTextBox1.AppendText("\n窗口宽度: " + width);
            richTextBox1.AppendText("\n窗口高度: " + height);
            richTextBox1.AppendText("\n窗口位置: (" + windowRect.Left + ", " + windowRect.Top + ")");
            richTextBox1.AppendText("\n窗口透明度: " + bAlpha);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            // 最大化窗口
            ShowWindow(hWnd, 3);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 最小化窗口
            ShowWindow(hWnd, 2);

        }
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private void button3_Click(object sender, EventArgs e)
        {
            // 将窗口置顶
            SetWindowPos(hWnd, new IntPtr(-1), 0, 0, 0, 0, 0x0001 | 0x0002);
            if (button3.Text == "置顶此窗口")
            {
                // 将窗口置顶
                SetWindowPos(hWnd, new IntPtr(-1), 0, 0, 0, 0, 0x0001 | 0x0002);
                button3.Text = "取消置顶此窗口";
            }
            else
            {
                // 将窗口置顶
                SetWindowPos(hWnd, new IntPtr(-2), 0, 0, 0, 0, 0x0001 | 0x0002);
                button3.Text = "置顶此窗口";
            }
        }

        private void 退出程序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            Visible = true;
        }

        private void 置托盘ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Visible = false;
        }
    }
}
