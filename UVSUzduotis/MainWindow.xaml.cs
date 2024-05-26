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
using System.Windows.Threading;

namespace UVSUzduotis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private readonly UVSDBContext _context;
        private ThreadController _threadController;
        public static ListView _listView;

        private int selectedThreads;

        public MainWindow(UVSDBContext context)
        {
            _context = context;
            _threadController = new ThreadController(_context);

            InitializeComponent();

            LoadListView();

        }

        private void LoadListView()
        {
            //Takes 20 rows from DB in descending ordered by ID.
            var latestThreads = _context.UVSThreadTable.OrderByDescending(t => t.ID ).Take(20).ToList();
            ThreadListView.ItemsSource = null;
            ThreadListView.ItemsSource = latestThreads;
            _listView = ThreadListView;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ThreadSelectionBox_Selected(object sender, RoutedEventArgs e)
        {
                //Assigns the thread amount to the variable.
                if (ThreadSelectionBox.SelectedItem != null)
                {
                    selectedThreads = (int)ThreadSelectionBox.SelectedItem;

                }

        }

        private void ThreadButton_Start(object sender, RoutedEventArgs e)
        {
            //The amount of threads used is based on the user request.
            _threadController.ThreadSymbolGeneration(selectedThreads,true);

        }
        private void ThreadButton_Stop(object sender, RoutedEventArgs e)
        {
            _threadController.ThreadSymbolGeneration(selectedThreads,false);
            LoadListView();
        }
    }
}