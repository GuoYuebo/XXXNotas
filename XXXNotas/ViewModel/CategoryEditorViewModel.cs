using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Samples.CustomControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using XXXNotas.Messages;
using XXXNotas.Model;
using XXXNotas.Service;
using XXXNotas.Utilities;

namespace XXXNotas.ViewModel
{
    public class CategoryEditorViewModel : ViewModelBase
    {
        #region 字段
        private readonly ICategoryService _categoryService;
        private ObservableCollection<Category> _categories;
        private Category _selectedCategory;
        private Category _defaultCategory;
        private bool _onCategoryUpdate = false;
        private bool _deleteNotesToo = true;
        private bool _isValid = true;
        private List<Guid> _notesToDelete;
        private List<Guid> _notesToTrash;
        private readonly List<Guid> _categoriesId;
        #endregion

        #region 属性
        public RelayCommand<Category> DefaultCategoryChangedCommand { get; private set; }
        public RelayCommand CategoryBeenSelected { get; private set; }
        public RelayCommand SelectBgColorCommand { get; private set; }
        public RelayCommand NewCategoryCommand { get; private set; }
        public RelayCommand<object> DeleteCategoryCommand { get; private set; }
        public RelayCommand AcceptCategoryCommand { get; private set; }
        public RelayCommand SelectFontColorCommand { get; private set; }

        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set { Set(ref _categories, value); }
        }
        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set { Set(ref _selectedCategory, value); }
        }
        public bool OnCategoryUpdate
        {
            get { return _onCategoryUpdate; }
            set { Set(ref _onCategoryUpdate, value); }
        }
        public bool DeleteNotesToo
        {
            get { return _deleteNotesToo; }
            set { Set(ref _deleteNotesToo, value); }
        }
        public bool IsValid
        {
            get { return _isValid; }
            set { Set(ref _isValid, value); }
        }
        public List<Guid> NotesToDelete
        {
            get { return _notesToDelete; }
            set { Set(ref _notesToDelete, value); }
        }
        public List<Guid> NotesToTrash
        {
            get { return _notesToTrash; }
            set { Set(ref _notesToTrash, value); }
        }
        #endregion

        #region 公共方法
        public CategoryEditorViewModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _categoriesId = new List<Guid>();
            _notesToTrash = new List<Guid>();

            Categories = new ObservableCollection<Category>(_categoryService.FindAll());
            NotesToDelete = new List<Guid>();

            DefaultCategoryChangedCommand = new RelayCommand<Category>(DefaultCategoryChanged);
            CategoryBeenSelected = new RelayCommand(() => OnCategoryUpdate = true);
            SelectBgColorCommand = new RelayCommand(SelectBgColor, () => OnCategoryUpdate);
            SelectFontColorCommand = new RelayCommand(SelectFontColor, () => OnCategoryUpdate);
            NewCategoryCommand = new RelayCommand(NewCategory, () => !OnCategoryUpdate);
            DeleteCategoryCommand = new RelayCommand<object>(DeleteCategory, category => OnCategoryUpdate && Categories.Count > 1 && SelectedCategory != null);
            AcceptCategoryCommand = new RelayCommand(AcceptCategory, () => OnCategoryUpdate && IsValid);

            Messenger.Default.Register<string>(this, SavingCatOptions);
        }
        #endregion

        #region 命令
        private void DefaultCategoryChanged(Category category)
        {
            _defaultCategory.IsDefault = false;
            category.IsDefault = true;
            _defaultCategory = category;
        }

        private void SelectBgColor()
        {
            string color = SelectColor(SelectedCategory.BackgroundColor);
            if (!string.IsNullOrEmpty(color))
            {
                SelectedCategory.BackgroundColor = color;
            }
        }

        private void NewCategory()
        {
            Category category = new Category()
            {
                Name = Resources.Strings.Name,
                BackgroundColor = "#FFFFFF",
                FontColor = "#000000"
            };
            //if (!_categories.Contains(category))
            if (!_categories.Any(c => c.Id == category.Id))
            {
                _categories.Add(category);
            }
        }

        private void DeleteCategory(object category)
        {
            Category cat = (Category)category;

            if(cat != null)
            {
                if (DeleteNotesToo)
                {
                    NotesToDelete.Add(cat.Id);
                }
                else
                {
                    NotesToTrash.Add(cat.Id);
                }
                SelectedCategory = null;
                Categories.Remove(cat);
                if (cat.IsDefault)
                {
                    Categories[0].IsDefault = true;
                    _defaultCategory = Categories[0];
                }
                _categoryService.SaveAll(_categories);
            }
        }

        private void AcceptCategory()
        {
            if(SelectedCategory != null)
            {
                _categoriesId.Add(SelectedCategory.Id);
            }
            SelectedCategory = null;
            _categoryService.SaveAll(Categories);
            OnCategoryUpdate = false;
        }

        private void SelectFontColor()
        {
            string color = SelectColor(SelectedCategory.FontColor);
            if (!string.IsNullOrEmpty(color))
            {
                SelectedCategory.FontColor = color;
            }
        }
        #endregion

        #region 私有方法
        private string SelectColor(string color)
        {
            ColorPickerDialog dialog = new ColorPickerDialog()
            {
                StartingColor = color.ToColor()
            };
            bool? result = dialog.ShowDialog();
            if (result != null && (bool)result)
            {
                return dialog.SelectedColor.ToString();
            }
            return "";
        }

        private void SavingCatOptions(string message)
        {
            if(message == "ClosingWindow")
            {
                AcceptCategory();
                Messenger.Default.Send<CategoryEditorChangesMessage, MainViewModel>(new CategoryEditorChangesMessage
                {
                    NotesToDelete = _notesToDelete,
                    NotesToTrash = _notesToTrash,
                    CategoriesId = _categoriesId
                });
            }
        }
        #endregion
    }
}
