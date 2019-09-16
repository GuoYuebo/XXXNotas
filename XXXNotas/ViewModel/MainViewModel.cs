using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using XXXNotas.Messages;
using XXXNotas.Model;
using XXXNotas.Service;
using XXXNotas.View;

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
        private Category _trash;
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
            set
            {
                if(value.Category != null && value.Category.Name == "Trash")
                {
                    value.Category = Categories[0];
                }
                Set(ref _actualNote, value);
            }
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
            _trash = new Category("Trash", "#f8f8f8", "#777777");

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
            DeleteAllNotesCommand = new RelayCommand(DeleteAllNotes, () => Notes.Count > 0);
            CategoryOptionsCommand = new RelayCommand(OpenCategoryOptions);

            Messenger.Default.Register<CategoryEditorChangesMessage>(this, MakingNewCatChanges);
        }
        #endregion

        #region Command
        /// <summary>
        /// 返回是否可以进行笔记添加
        /// </summary>
        /// <returns></returns>
        private bool CanAddNote()
        {
            return SelectedCategory != null && !string.IsNullOrEmpty(ActualNote.Content);
        }

        /// <summary>
        /// 添加笔记命令函数
        /// </summary>
        private void AddNote()
        {
            ActualNote.Category = SelectedCategory;

            if (!Notes.Any(c => c.Id == ActualNote.Id))
            {
                Notes.Add(ActualNote);
                _noteService.Save(ActualNote);
            }
            else
            {
                Notes[Notes.IndexOf(Notes.FirstOrDefault(c => c.Id == ActualNote.Id))] = ActualNote;
                _noteService.SaveAll(Notes.ToList());
            }

            ActualNote = new Note();
        }

        /// <summary>
        /// 编辑笔记命令
        /// </summary>
        /// <param name="note">需要编辑的笔记</param>
        private void EditNote(Note note)
        {
            ActualNote = note;
            SelectedCategory = note.Category;
        }

        /// <summary>
        /// 删除笔记命令
        /// </summary>
        /// <param name="note">需要删除的笔记</param>
        private void DeleteNote(Note note)
        {
            Notes.Remove(note);
            _noteService.Delete(note);
        }

        /// <summary>
        /// 删除所有笔记命令
        /// </summary>
        private void DeleteAllNotes()
        {
            if ((new DeleteAllMsgBox()).ShowDialog() == true)
            {
                _noteService.Reset();
                Notes = new ObservableCollection<Note>();
            }
        }

        /// <summary>
        /// 打开目录选项命令
        /// </summary>
        private void OpenCategoryOptions()
        {
            (new View.CategoryEditorView()).ShowDialog();
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 响应目录变化
        /// </summary>
        /// <param name="message">目录变化信息</param>
        private void MakingNewCatChanges(CategoryEditorChangesMessage message)
        {
            UpdateCategoriesAndNotes(message.CategoriesId);
            DeleteNotesWithoutCategory(message.NotesToDelete);
            NotesToTrash(message.NotesToTrash);
        }

        private void UpdateCategoriesAndNotes(List<Guid> CategoriesId)
        {
            Categories = new ObservableCollection<Category>(_categoryService.FindAll());
            SelectedCategory = Categories[0];
            if(CategoriesId.Count > 0)
            {
                foreach(var id in CategoriesId)
                {
                    foreach(var note in Notes.Where(c => c.Category.Id == id))
                    {
                        note.Category = _categoryService.GetById(id);
                        _noteService.Save(note);
                    }
                }
            }
        }

        private void DeleteNotesWithoutCategory(List<Guid> catetoriesId)
        {
            if(catetoriesId.Count > 0)
            {
                var notes = (from Note note in Notes
                            where catetoriesId.Contains(note.Category.Id)
                            select note).ToList<Note>();

                foreach (var note in notes)
                {
                    Notes.Remove(note);
                    _noteService.Delete(note);
                }
            }
        }

        private void NotesToTrash(List<Guid> categoriesId)
        {
            if(categoriesId.Count > 0)
            {
                var notes = from Note note in Notes
                            where categoriesId.Contains(note.Category.Id)
                            select note;
                foreach(var note in notes)
                {
                    note.Category = _trash;
                    _noteService.Save(note);
                }
            }
        }
        #endregion
    }
}