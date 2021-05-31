using FileSearchApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Ookii.Dialogs.Wpf;
using Microsoft.Extensions.Configuration;

namespace FileSearchApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AppSettings appSettings = GetSettings();

            DataContext = new FileSearchViewModel(appSettings);
        }

        private AppSettings GetSettings()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            
            builder.AddJsonFile("appsettings.json");
            IConfigurationRoot config = builder.Build();
            return config.Get<AppSettings>();
        }

        private FileSearchViewModel ViewModel
        {
            get { return (FileSearchViewModel)DataContext; }
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new VistaFolderBrowserDialog();

            if (dialog.ShowDialog() == true)
            {
                ViewModel.DirectorySearchPath = dialog.SelectedPath;
            }
        }
    }
}
