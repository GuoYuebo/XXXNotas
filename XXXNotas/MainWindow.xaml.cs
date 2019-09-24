using System.Windows;
using System.Windows.Controls;
using XXXNotas.Model;
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
            ViewModel.EditNoteCommand.Execute((e.Source as MenuItem).DataContext as Note);
        }

        private void DeleteNote_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.DeleteNoteCommand.Execute((e.Source as MenuItem).DataContext);
        }

        /// <summary>
        /// 右键列表中combobox载入时，初始化内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            var note = (Note)cb.DataContext;
            cb.ItemsSource = ViewModel.Categories;
            cb.SelectedItem = note.Category;
        }

        /// <summary>
        /// 右键列表中选项变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(sender is ComboBox cb && cb.DataContext is Note note)
            {
                ViewModel.ChangeNoteCategory(note, cb.SelectedItem as Category);
            }
        }
    }
}