using FuckMTP.Core;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FuckMTP.UI
{
    /// <summary>
    /// Interaction logic for ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow : Window
    {
        private readonly int numberOfElements;
        private readonly Action<ProgressReporter> action;

        internal ProgressBar ProgressBar => progressBar;

        public ProgressWindow(int numberOfElements, string title, Action<ProgressReporter> action)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("May not be empty", nameof(title));
            if (action is null) throw new ArgumentNullException(nameof(action));

            this.numberOfElements = numberOfElements;
            this.action = action;

            InitializeComponent();
            Title = title;
            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                using (ProgressWindowHandlingProgressReporter progressReporter = new ProgressWindowHandlingProgressReporter(numberOfElements, this))
                    action(progressReporter);
            }).ConfigureAwait(false);
        }
    }
}
