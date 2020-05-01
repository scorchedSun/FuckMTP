using FileSystem;
using System.Windows.Controls;

namespace FuckMTP.UI
{
    /// <summary>
    /// Interaction logic for DirectoryControl.xaml
    /// </summary>
    public partial class DirectoryControl : UserControl
    {
        public Directory Directory { get; }

        public DirectoryControl(Directory directory)
        {
            InitializeComponent();
            Directory = directory;
            tbName.Text = Directory.Name;
            ToolTip = new ToolTip { Content = Directory.Name };
        }
    }
}
