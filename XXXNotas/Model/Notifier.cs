using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XXXNotas.Model
{
    /// <summary>
    /// 提供通知支持的方法
    /// 用于Model
    /// 需要可序列化
    /// </summary>
    [Serializable]
    class Notifier : INotifyPropertyChanged
    {
        /// <summary>
        /// 属性值改变时调用
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 通知 <see cref="propertyName"/> 改变
        /// </summary>
        /// <param name="propertyName"></param>
        public void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
