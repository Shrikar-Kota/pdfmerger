using System.Windows;
using System.Windows.Input;

namespace PdfMerge
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var _ = new MainWindowPresenter(this);
        }
    }
}
