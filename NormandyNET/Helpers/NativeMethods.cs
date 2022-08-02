using System;
using System.Runtime.InteropServices;

public static class NativeMethods
{
    public const int SB_BOTH = 3;

    [DllImport("user32.dll")]
    public static extern bool ShowScrollBar(System.IntPtr hWnd, int wBar, bool bShow);

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    #region screen

    [DllImport("user32.dll", SetLastError = true)]
    internal static extern IntPtr FindWindowA(string lpClassName, string lpWindowName);

    [DllImport("user32.dll")]
    internal static extern IntPtr GetDC(IntPtr hwnd);

    [DllImport("user32.dll")]
    internal static extern bool GetWindowRect(IntPtr hwnd, out Rectangle rect);

    [DllImport("user32.dll")]
    internal static extern int ClientToScreen(IntPtr hwnd, out Point point);

    #endregion screen

    #region structs

    [StructLayout(LayoutKind.Sequential)]
    internal struct Rectangle
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct Point
    {
        public int x;
        public int y;
    }

    #endregion structs
}