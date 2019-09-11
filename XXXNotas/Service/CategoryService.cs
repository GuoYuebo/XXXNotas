using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using XXXNotas.Model;

namespace XXXNotas.Service
{
    class CategoryService : ICategoryService
    {
        /// <summary>
        /// 目录列表
        /// </summary>
        private IList<Category> _categories;
        private readonly string _file;

        public CategoryService()
        {
            _categories = new List<Category>();
            _file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Categories.an");
        }

        #region ICategoryService
        public IList<Category> FindAll()
        {
            Deserialize();
            return _categories;
        }

        public Category GetById(Guid id)
        {
            Deserialize();
            return _categories.SingleOrDefault(c => c.Id == id);
        }

        public void Reset()
        {
            File.Delete(_file);
            _categories = new List<Category>();
        }

        public void SaveAll(IList<Category> categories)
        {
            _categories = categories;
            Serialize();
        }
        #endregion

        #region Serialize
        private void Serialize()
        {
            List<SerializedCategory> categories = new List<SerializedCategory>();
            foreach(Category category in _categories)
            {
                categories.Add(SerializedCategory.SerializeCategory(category));
            }
            using(FileStream fs = File.Open(_file, FileMode.OpenOrCreate))
            {
                (new BinaryFormatter()).Serialize(fs, categories);
            }
        }

        private void Deserialize()
        {
            if (File.Exists(_file))
            {
                try
                {
                    using(FileStream fs = File.Open(_file, FileMode.Open))
                    {
                        List<SerializedCategory> list = (List<SerializedCategory>)(new BinaryFormatter()).Deserialize(fs);
                        _categories.Clear();
                        foreach(SerializedCategory item in list)
                        {
                            _categories.Add(SerializedCategory.DeserializeCategory(item));
                        }
                    }
                }catch(Exception e)
                {
                    Debug.Log(e.Message);
                    File.Delete(_file);
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// 可序列化的Category
    /// </summary>
    [Serializable]
    class SerializedCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string BackgroundColor { get; set; }
        public string FontColor { get; set; }
        public bool IsDefault { get; set; }

        public static SerializedCategory SerializeCategory(Category category)
        {
            return new SerializedCategory()
            {
                Id = category.Id,
                Name = category.Name,
                BackgroundColor = category.BackgroundColor,
                FontColor = category.FontColor,
                IsDefault = category.IsDefault
            };
        }

        public static Category DeserializeCategory(SerializedCategory category)
        {
            return new Category()
            {
                Id = category.Id,
                Name = category.Name,
                BackgroundColor = category.BackgroundColor,
                FontColor = category.FontColor,
                IsDefault = category.IsDefault
            };
        }

    }
}
