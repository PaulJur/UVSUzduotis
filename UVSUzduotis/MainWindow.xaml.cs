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
using UVSUzduotis.Model;

namespace UVSUzduotis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly UVSDBContext _context;
        ThreadModelTest _test;

        public static ListView _listView;

        public MainWindow(UVSDBContext context)
        {
            _context = context;
            ThreadController threadController = new ThreadController(_context);

            InitializeComponent();

            threadController.ThreadSymbolGeneration(0) ;

            LoadListView();

        }

        private void LoadListView()
        {
            ThreadListView.ItemsSource = null;
            ThreadListView.ItemsSource = _context.ThreadTable.ToList();
            _listView = ThreadListView;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ThreadSelectionBox_Selected(object sender, RoutedEventArgs e)
        {
            if (ThreadSelectionBox.SelectedItem != null)
            {
                int selectedThreads = (int)ThreadSelectionBox.SelectedItem;
                ThreadController threadController = new ThreadController(_context);
                threadController.ThreadSymbolGeneration(selectedThreads);

            }

        }
    }
}