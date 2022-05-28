using System.Windows;
using ProjectTesting.Viewmodels;
namespace ProjectTesting
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewmodel();
        }
    }
}
