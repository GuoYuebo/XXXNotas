using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using XXXNotas.Messages;
using XXXNotas.Model;
using XXXNotas.Service;

namespace XXXNotas.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region 字段
        private readonly ICategoryService _categoryService;
        private readonly INoteService _noteService;

        private ObservableCollection<Category> _categories;
        private ObservableCollection<Note> _notes;

        private Category _selectedCategory;
        private Note _actualNote;
        #endregion

        #region 属性
        public RelayCommand AddNoteCommand { get; private set; }

        public RelayCommand<Note> EditNoteCommand { get; private set; }

        public RelayCommand<Note> DeleteNoteCommand { get; private set; }

        public RelayCommand DeleteAllNotesCommand { get; private set; }

        public RelayCommand CategoryOptionsCommand { get; private set; }

        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set { Set(ref _categories, value); }
        }

        public ObservableCollection<Note> Notes
        {
            get { return _notes; }
            set { Set(ref _notes, value); }
        }

        public Category SelectedCategory
        {
            get { return _selectedCategory; }
            set { Set(ref _selectedCategory, value); }
        }

        public Note ActualNote
        {
            get { return _actualNote; }
            set { Set(ref _actualNote, value); }
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="categoryService">目录服务</param>
        /// <param name="noteService">笔记服务</param>
        public MainViewModel(ICategoryService categoryService, INoteService noteService)
        {
            _categoryService = categoryService;
            _noteService = noteService;

            Categories = new ObservableCollection<Category>(_categoryService.FindAll());
            Notes = new ObservableCollection<Note>(_noteService.FindAll());

            // 若目录列表为空，则添加一个带welcome笔记的默认目录
            if (Categories.Count == 0)
            {
                Category category = new Category(Resources.Strings.GeneralCat, "#33cc00", "#ffffff");
                Categories.Add(category);
                _categoryService.SaveAll(Categories);

                Note note = new Note(Resources.Strings.WelcomeMessage, category);
                Notes.Add(note);
                _noteService.Save(note);
            }

            ActualNote = new Note();
            SelectedCategory = Categories[0];

            AddNoteCommand = new RelayCommand(AddNote, CanAddNote);
            EditNoteCommand = new RelayCommand<Note>(EditNote);
            DeleteNoteCommand = new RelayCommand<Note>(DeleteNote);
            DeleteAllNotesCommand = new RelayCommand(DeleteAllNotes);
            CategoryOptionsCommand = new RelayCommand(OpenCategoryOptions);

            Messenger.Default.Register<CategoryEditorChangesMessage>(this, MakingNewCatChanges);
        }

        /// <summary>
        /// 响应目录变化
        /// </summary>
        /// <param name="message">目录变化信息</param>
        private void MakingNewCatChanges(CategoryEditorChangesMessage message)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Command
        /// <summary>
        /// 返回是否可以进行笔记添加
        /// </summary>
        /// <returns></returns>
        private bool CanAddNote()
        {
            return SelectedCategory != null;
        }

        ///// <summary>
        ///// 添加笔记命令函数
        ///// </summary>
        private void AddNote()
        {
            if (string.IsNullOrEmpty(ActualNote.Content)) return;

            ActualNote.Category = SelectedCategory;

            if (!Notes.Any(c => c.Id == ActualNote.Id))
            {
                Notes.Add(ActualNote);
                _noteService.Save(ActualNote);
            }

            ActualNote = new Note();
        }

        /// <summary>
        /// 编辑笔记命令
        /// </summary>
        /// <param name="note">需要编辑的笔记</param>
        private void EditNote(Note note)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 删除笔记命令
        /// </summary>
        /// <param name="note">需要删除的笔记</param>
        private void DeleteNote(Note note)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 删除所有笔记命令
        /// </summary>
        private void DeleteAllNotes()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 打开目录选项命令
        /// </summary>
        private void OpenCategoryOptions()
        {
            (new View.CategoryEditorView()).ShowDialog();
            UpdateCategory();
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 目录配置后，更新日记对应的目录
        /// </summary>
        private void UpdateCategory()
        {
            foreach(var item in Notes)
            {
                item.Category = Categories.FirstOrDefault(c => c.Id == item.Category.Id);
            }
            _noteService.SaveAll(Notes.ToList());
        }
        #endregion
    }
}