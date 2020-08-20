using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace NiceConsole
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow m = new MainWindow();
            string[] args = e.Args;
            string command = args.Length > 0 ? string.Join(" ", args) : "cmd";
            m.Run(command);
            m.Show();
        }
    }
}
