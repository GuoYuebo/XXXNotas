using System.Windows;
using System.Windows.Controls;
using XXXNotas.ViewModel;

namespace XXXNotas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        private MainViewModel ViewModel
        {
            get
            {
                return DataContext as MainViewModel;
            }
        }

        private void EditNote_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.EditNoteCommand.Execute((e.Source as MenuItem).DataContext as Model.Note);
        }

        private void DeleteNote_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.DeleteNoteCommand.Execute((e.Source as MenuItem).DataContext);
        }
    }
}