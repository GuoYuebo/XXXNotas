using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XXXNotas.Messages
{
    class CategoryEditorChangesMessage
    {
        public List<Guid> NotesToDelete { get; set; }

        public List<Guid> CategoriesId { get; set; }

        public List<Guid> NotesToTrash { get; set; }
    }
}
