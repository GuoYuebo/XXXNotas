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
    [Serializable]
    class CategoryService : ICategoryService
    {
        /// <summary>
        /// 目录列表
        /// </summary>
        private IList<Category> _categories;
        /// <summary>
        /// 文件存储路径
        /// </summary>
        private readonly string _dataFile;

        /// <summary>
        /// 以目录名构造目录服务
        /// </summary>
        /// <param name="name">目录名称</param>
        public CategoryService(string name)
        {
            _dataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name + ".an");

            Deserialize();
        }

        #region Serialize
        /// <summary>
        /// 将_categories序列化为文件
        /// </summary>
        private void Serialize()
        {
            using(FileStream stream = File.Open(_dataFile, FileMode.OpenOrCreate))
            {
                (new BinaryFormatter()).Serialize(stream, _categories);
            }
        }

        /// <summary>
        /// 反序列化文件
        /// </summary>
        private void Deserialize()
        {
            if (File.Exists(_dataFile))
            {
                using(FileStream stream = File.Open(_dataFile, FileMode.Open))
                {
                    _categories = (IList<Category>)(new BinaryFormatter()).Deserialize(stream);
                }
            }
            else
            {
                _categories = new List<Category>();
            }
        }
        #endregion

        #region ICategoryService
        public IList<Category> FindAll()
        {
            Deserialize();
            return _categories;
        }

        public Category GetById(Guid id)
        {
            return _categories.SingleOrDefault(c => c.Id == id);
        }

        public void Reset()
        {
            File.Delete(_dataFile);
            _categories = new List<Category>();
        }

        public void SaveAll(IList<Category> categories)
        {
            _categories = categories;
            Serialize();
        }
        #endregion
    }
}
