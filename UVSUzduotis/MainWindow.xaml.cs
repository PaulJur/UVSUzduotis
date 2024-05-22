using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UVSUzduotis.Controller;
using UVSUzduotis.Data;

namespace UVSUzduotis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly UVSDBContext _context;
        

        public MainWindow(UVSDBContext context)
        {
            _context = context;
            ThreadController threadController = new ThreadController(_context);

            InitializeComponent();

            threadController.ThreadTest();
            
            //Do a thread test
            //Make a string generator, make a single thread generate random symbols and send to database while and show it in the app.

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}