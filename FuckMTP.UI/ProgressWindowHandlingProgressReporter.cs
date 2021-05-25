using FuckMTP.Core;
using System;

namespace FuckMTP.UI
{
    internal class ProgressWindowHandlingProgressReporter : ProgressReporter
    {
        private readonly ProgressWindow progressWindow;

        public ProgressWindowHandlingProgressReporter(int numberOfElements, ProgressWindow progressWindow)
            : base(numberOfElements)
        {
            this.progressWindow = progressWindow ?? throw new ArgumentNullException(nameof(progressWindow));
            this.progressWindow.Dispatcher.Invoke(() => this.progressWindow.ProgressBar.Maximum = maximum);
        }

        protected override void HandleProgressChanged(object sender, int value)
            => progressWindow.ProgressBar.Dispatcher.Invoke(() => progressWindow.ProgressBar.Value = value);

        protected override void Finish()
        {
            base.Finish();
            progressWindow.Dispatcher.Invoke(progressWindow.Close);
        }
    }
}
