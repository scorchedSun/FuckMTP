using System;
using System.Collections.Generic;
using System.Windows;

namespace FuckMTP.UI
{
    /// <summary>
    /// Interaction logic for FileDrop.xaml
    /// </summary>
    public partial class FileDrop : Window
    {
        private readonly FileDropViewModel viewModel;

        public IEnumerable<string> Files => viewModel.Files;

        public FileDrop()
        {
            InitializeComponent();
            DataContext = viewModel = new FileDropViewModel();
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            try
            {
                viewModel.AddFiles((string[])e.Data.GetData(DataFormats.FileDrop));
            }
            catch (Exception)
            {
            }
        }

        private void btStart_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void btAbort_Click(object sender, RoutedEventArgs e) => Close();
    }
}
