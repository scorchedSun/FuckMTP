using FileSystem;
using System.Windows.Controls;

namespace FuckMTP.UI
{
    /// <summary>
    /// Interaction logic for FileControl.xaml
    /// </summary>
    public partial class FileControl : UserControl
    {
        public File File { get; }

        public FileControl(File file)
        {
            InitializeComponent();
            File = file;
            tbName.Text = File.Name;
            ToolTip = new ToolTip { Content = File.Name };
        }
    }
}
