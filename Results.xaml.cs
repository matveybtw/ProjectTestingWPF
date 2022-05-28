using System.Windows;
using ProjectTesting.Viewmodels;
using Testing;
namespace ProjectTesting
{
    public partial class Results : Window
    {
        public Results(Test test)
        {
            InitializeComponent();
            DataContext = new ResultsViewmodel(test);
        }
    }
}
