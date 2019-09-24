using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XXXNotas.Model
{
    /// <summary>
    /// 笔记类 需要可序列化
    /// </summary>
    public class Note : ObservableObject
    {
        private Guid _id;
        private string _content;
        private string _date;
        private Category _category;

        /// <summary>
        /// 创建一个新实例 由于需要在VM中创建Note，故需要无参构造
        /// </summary>
        public Note() : this(string.Empty, null) { }

        /// <summary>
        /// 初始化一个新的Note实例
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="category">目录</param>
        public Note(string content, Category category)
        {
            _id = Guid.NewGuid();
            _content = content;
            _category = category;
            _date = DateTime.Now.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Note唯一标识符
        /// </summary>
        public Guid Id
        {
            get { return _id; }
            set { Set(ref _id, value); }
        }

        /// <summary>
        /// 笔记内容
        /// </summary>
        public string Content
        {
            get { return _content; }
            set { Set(ref _content, value); }
        }

        /// <summary>
        /// 日期
        /// </summary>
        public string Date
        {
            get { return _date; }
            set { Set(ref _date, value); }
        }

        /// <summary>
        /// 笔记所在目录
        /// </summary>
        public Category Category
        {
            get { return _category; }
            set { Set(ref _category, value); }
        }

        /// <summary>
        /// 重写Equals
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if(obj is Note note)
            {
                return Id == note.Id;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
