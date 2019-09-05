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

        public CategoryService()
        {
            _categories = new List<Category>();
        }

        #region ICategoryService
        public IList<Category> FindAll()
        {
            return _categories;
        }

        public Category GetById(Guid id)
        {
            return _categories.SingleOrDefault(c => c.Id == id);
        }

        public void Reset()
        {
            _categories = new List<Category>();
        }

        public void SaveAll(IList<Category> categories)
        {
            _categories = categories;
        }
        #endregion
    }
}
