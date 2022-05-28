using System.Windows;
using Testing;
using ProjectTesting.Viewmodels;
namespace ProjectTesting
{
    /// <summary>
    /// Логика взаимодействия для Test.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        public TestViewmodel tw { get; set; }
        public TestWindow(Test test)
        {
            tw= new TestViewmodel(test, this);
            DataContext = tw;
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
