using HappyBusProject.Interfaces;
using System.Windows;

namespace HappyBusProject.Admin.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IBusAppObject<object> _busAppObject;
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
