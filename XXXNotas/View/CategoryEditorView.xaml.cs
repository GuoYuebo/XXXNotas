using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using XXXNotas.Messages;
using XXXNotas.ViewModel;

namespace XXXNotas.View
{
    /// <summary>
    /// CategoryEditorView.xaml 的交互逻辑
    /// </summary>
    public partial class CategoryEditorView : Window
    {
        public CategoryEditorView()
        {
            InitializeComponent();
        }

        private CategoryEditorViewModel ViewModel
        {
            get
            {
                return DataContext as CategoryEditorViewModel;
            }
        }

        private void OnColorChange(object sender, TextChangedEventArgs e)
        {
            ViewModel.IsValid = !Validation.GetHasError(backgroundTxt)
                && !Validation.GetHasError(fontColorTxt);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Messenger.Default.Send<string, CategoryEditorViewModel>("ClosingWindow");
        }
    }
}
