using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using NiceConsole;

namespace NiceConsole
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ProcessRunner runner;
        private List<string> historyCommands;
        private int cmdIndex;


        public MainWindow() {
            InitializeComponent();
            historyCommands = new List<string>();
            cmdIndex = 0;
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            runner.Close();
            Environment.Exit(0);
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            historyCommands.Add(command.Text);
            cmdIndex = 0;
            runner.GetStdIn().WriteLine(command.Text);
            command.Text = "";
        }
        public TextBox GetOutput() {
            return output;
        }

        public void Run(string command)
        {
            title.Content = command;
            Title = command;
            Thread t = new Thread(() => {
                runner = new ProcessRunner(command);
                runner.start();
                StreamReader reader = runner.GetStdOut();
                StringBuilder builder = new StringBuilder();
                while (!reader.EndOfStream)
                {
                    builder.AppendLine(reader.ReadLine());
                    updateOutput(builder);
                }
                builder.AppendLine("Process finished with exit code " + runner.GetProcess().ExitCode);
                updateOutput(builder);
                runner.Close();
            });
            t.Start();
        }

        private void updateOutput(StringBuilder builder)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                output.Text = builder.ToString();
                scrollViewer.ScrollToVerticalOffset(output.ActualHeight);
            });
        }

        private void title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle_MouseLeftButtonDown(sender, e);
        }

        private void command_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key) {
                case Key.Enter:
                    Button_Click(sender, e);
                    break;
            }
        }

        private void previousCommand(object sender, RoutedEventArgs e)
        {
            int size = historyCommands.Count();
            if (size == 0) return;
            if (cmdIndex < size)
            {
                ++cmdIndex;
                command.Text = historyCommands[size - cmdIndex];
            }
        }

        private void nextCommand(object sender, RoutedEventArgs e)
        {
            int size = historyCommands.Count();
            if (size == 0) return;
            if (cmdIndex > 1)
            {
                --cmdIndex;
                command.Text = historyCommands[size - cmdIndex];
            }
            else
                command.Text = "";
        }
    }
    
}
