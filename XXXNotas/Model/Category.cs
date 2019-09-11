﻿using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XXXNotas.Model
{
    /// <summary>
    /// 目录类
    /// </summary>
    public class Category : ObservableObject
    {
        private Guid _id;
        private string _name;
        private string _backgroundColor;
        private string _fontColor;
        private bool _isDefault;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public Category() : this("", "", "") { }

        /// <summary>
        /// 初始化一个 <see cref="Category"/> 类
        /// </summary>
        /// <param name="name">目录名称</param>
        /// <param name="backgroundColor">目录背景色</param>
        /// <param name="fontColor">目录字体颜色</param>
        public Category(string name, string backgroundColor, string fontColor)
        {
            _id = Guid.NewGuid();
            _name = name;
            _backgroundColor = backgroundColor;
            _fontColor = fontColor;
        }

        /// <summary>
        /// Id
        /// </summary>
        public Guid Id
        {
            get { return _id; }
            set { Set(ref _id, value); }
        }

        /// <summary>
        /// 目录名称
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        /// <summary>
        /// 背景色
        /// </summary>
        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set { Set(ref _backgroundColor, value); }
        }

        /// <summary>
        /// 字体颜色
        /// </summary>
        public string FontColor
        {
            get { return _fontColor; }
            set { Set(ref _fontColor, value); }
        }

        /// <summary>
        /// 是否是默认目录
        /// </summary>
        public bool IsDefault
        {
            get { return _isDefault; }
            set { Set(ref _isDefault, value); }
        }

        /// <summary>
        /// 表示该 <see cref="System.Object"/> 是否是本实例相同
        /// </summary>
        /// <param name="obj">比较的 <see cref="System.Object"/></param>
        /// <returns><c>true</c>表示相同</returns>
        public override bool Equals(object obj)
        {
            Category category = obj as Category;
            return category != null && (category.Id == this.Id && category.Name == this.Name);
        }

        /// <summary>
        /// 重写Equals方法后需要重写GetHashCode方法
        /// </summary>
        /// <returns>返回的对象哈希值</returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode() + Name.GetHashCode();
        }
    }
}
