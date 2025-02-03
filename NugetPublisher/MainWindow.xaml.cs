using NugetPublisher.Model;
using Path = System.IO.Path;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;

namespace NugetPublisher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isSpVisible;

        public MainWindow()
        {
            InitializeComponent();

            DotEnvService.LoadEnvironmentFromFile(Path.Combine(Environment.CurrentDirectory, ".env"));

            tbExportPath.Text = Environment.GetEnvironmentVariable("DEFAULT_PATH") ??
                              throw new InvalidOperationException("string DEFAULT_PATH not found.");

        }

        private async Task<bool> Load()
        {
            for (int i = 0; i<100; i++)
            {
                await Task.Delay(1);
                pbMain.Value = i;
            }
            return true;
        }

        private void ImagePanel_Drop(object sender, DragEventArgs e)
        {
            Load();

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                var filePath = files[0];
                string command = $"nuget add {filePath} -source {tbExportPath.Text}";
                //string command = "";
                ExecuteCommand(command); 
            }
        }
        private void ExecuteCommand(string command)
        {

            var processInfo = new ProcessStartInfo("cmd.exe", "/c " + command)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(processInfo))
            {
                if (process is not null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    MessageBox.Show(output);
                    pbMain.Value = 0;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            isSpVisible = !isSpVisible;
            if (isSpVisible)
            {
                spSettings.Visibility = Visibility.Visible;
                return;
            }
            spSettings.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var envPath = Path.Combine(Environment.CurrentDirectory, ".env");
            using (var file = new StreamWriter(envPath))
            {
                file.WriteLine("DEFAULT_PATH = " + tbExportPath.Text);
            }
        }
    }
}