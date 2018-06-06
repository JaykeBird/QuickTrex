using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Threading.Tasks;

namespace QuickTranslate
{
    public static class DialogHelper
    {
        private const int GWL_STYLE = -16,
          WS_MAXIMIZEBOX = 0x10000,
          WS_MINIMIZEBOX = 0x20000;

        [DllImport("user32.dll")]
        extern static int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        extern static int SetWindowLong(IntPtr hwnd, int index, int value);

        /// <summary>
        /// Hide/deactivate the Minimize and Maximize buttons in a window. The window can still be resized and moved.
        /// </summary>
        /// <param name="w"></param>
        public static void HideExtraCaptionButtons(Window w)
        {
            IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper(w).Handle;
            int currentStyle = GetWindowLong(hwnd, GWL_STYLE);

            SetWindowLong(hwnd, GWL_STYLE, (currentStyle & ~WS_MAXIMIZEBOX & ~WS_MINIMIZEBOX));
        }

    }
}
