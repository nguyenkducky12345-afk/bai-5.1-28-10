using System;
using System.Windows.Forms;

namespace LoginFormApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Khởi chạy FormLogin
            Application.Run(new FormLogin());
        }
    }
}