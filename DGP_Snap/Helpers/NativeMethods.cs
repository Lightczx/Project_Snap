using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DGP_Snap.Helpers
{
    class NativeMethods
    {
        private const int SW_HIDE = 0;  //隐藏
        private const int SW_RESTORE = 5;  //显示

        [DllImport("user32.dll")]
        private static extern int FindWindow(string ClassName, string WindowName);
        [DllImport("user32.dll")]
        private static extern int ShowWindow(int handle, int cmdShow);


        //隐藏
        public static void HideSystemTaskBar()
        {
            ShowWindow(FindWindow("Shell_TrayWnd", null), SW_HIDE);//隐藏系统任务栏
            ShowWindow(FindWindow("Button", null), SW_HIDE);//隐藏系统开始菜单栏按钮
        }
        //显示
        public static void ShowSystemTaskBar()
        {
            ShowWindow(FindWindow("Shell_TrayWnd", null), SW_RESTORE);//显示系统任务栏
            ShowWindow(FindWindow("Button", null), SW_RESTORE);//显示系统开始菜单栏按钮
        }
    }
}
