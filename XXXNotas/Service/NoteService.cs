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
    class NoteService : INoteService
    {
        IList<Note> _notes;
        /// <summary>
        /// 文件所在路径
        /// </summary>
        private string _dataFile;

        public NoteService(string fileName)
        {
            _dataFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName + ".an");

            Deserialize();
        }

        #region INoteService
        public void Delete(Note note)
        {
            _notes.Remove(note);
            Serialize();
        }

        public IList<Note> FindAll()
        {
            return _notes;
        }

        public void Reset()
        {
            File.Delete(_dataFile);
            _notes = new List<Note>();
        }

        public void Save(Note note)
        {
            _notes.Add(note);
            Serialize();
        }
        #endregion

        #region Serialize
        void Serialize()
        {
            if (File.Exists(_dataFile))
            {
                using(Stream stream = File.Open(_dataFile, FileMode.OpenOrCreate))
                {
                    (new BinaryFormatter()).Serialize(stream, _notes);
                }
            }
        }

        void Deserialize()
        {
            using(Stream stream = File.Open(_dataFile, FileMode.Open))
            {
                _notes = (IList<Note>)(new BinaryFormatter()).Deserialize(stream);
            }
        }
        #endregion
    }
}
