using FuckMTP.Core;
using System;
using System.Windows;

namespace FuckMTP.UI
{
    /// <summary>
    /// Interaction logic for ModeSelection.xaml
    /// </summary>
    public partial class ModeSelection : Window
    {
        public IOperationConfiguration Configuration { get; private set; }

        public ModeSelection()
        {
            InitializeComponent();
        }

        private void btnAbort_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;

            Configuration = new OperationConfiguration(GetSelectedMode(), GetSelectedBehaviorRegardingDuplicates());
            Close();
        }

        private Mode GetSelectedMode() => rbtnCopy.IsChecked.Value ? Mode.Copy : Mode.Move;

        private BehaviorRegardingDuplicates GetSelectedBehaviorRegardingDuplicates()
        {
            if (rbtnIgnore.IsChecked.Value)
                return BehaviorRegardingDuplicates.Ignore;
            if (rbtnWithSuffix.IsChecked.Value)
                return BehaviorRegardingDuplicates.CopyWithSuffix;
            if (rbtnOverwrite.IsChecked.Value)
                return BehaviorRegardingDuplicates.Overwrite;
            throw new InvalidOperationException("Unallowed application state: No behavior regarding duplicates was selected.");
        }

        private sealed class OperationConfiguration : IOperationConfiguration
        {
            public Mode Mode { get; }
            public BehaviorRegardingDuplicates BehaviorRegardingDuplicates { get; }

            public OperationConfiguration(Mode mode, BehaviorRegardingDuplicates behaviorRegardingDuplicates)
            {
                Mode = mode;
                BehaviorRegardingDuplicates = behaviorRegardingDuplicates;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) => DialogResult = DialogResult ?? false;
    }
}
