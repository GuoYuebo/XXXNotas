using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XXXNotas.Model;

namespace XXXNotas.Service
{
    public interface ICategoryService
    {
        void SaveAll(IList<Category> categories);

        IList<Category> FindAll();

        Category GetById(Guid id);

        void Reset();
    }
}
