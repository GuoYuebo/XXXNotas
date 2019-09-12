using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using XXXNotas.Model;
using XXXNotas.Service;

namespace XXXNotas.Service
{
    class NoteService : INoteService
    {
        IList<Note> _notes;
        private readonly string _file;

        public NoteService()
        {
            _file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Notes.an");
            _notes = new List<Note>();
        }

        #region INoteService
        public void Delete(Note note)
        {
            _notes.Remove(note);
            Serialize();
        }

        public IList<Note> FindAll()
        {
            Deserialize();
            return _notes;
        }

        public void Reset()
        {
            File.Delete(_file);
            _notes = new List<Note>();
        }

        public void Save(Note note)
        {
            _notes.Add(note);
            Serialize();
        }

        public void SaveAll(List<Note> notes)
        {
            _notes = notes;
            Serialize();
        }
        #endregion

        #region Serialize
        private void Serialize()
        {
            List<SerializedNote> notes = new List<SerializedNote>();
            foreach(Note note in _notes)
            {
                notes.Add(SerializeNote(note));
            }
            using (FileStream fs = File.Open(_file, FileMode.OpenOrCreate))
            {
                (new BinaryFormatter()).Serialize(fs, notes);
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
                        List<SerializedNote> list = (List<SerializedNote>)(new BinaryFormatter()).Deserialize(fs);
                        _notes.Clear();
                        foreach(SerializedNote note in list)
                        {
                            _notes.Add(DeserializeNote(note));
                        }
                    }
                }catch(Exception e)
                {
                    Debug.Log(e.Message);
                    File.Delete(_file);
                }
            }
        }

        private SerializedNote SerializeNote(Note note)
        {
            return new SerializedNote()
            {
                Id = note.Id,
                Content = note.Content,
                Date = note.Date,
                Category = SerializedCategory.SerializeCategory(note.Category)
            };
        }

        private Note DeserializeNote(SerializedNote note)
        {
            return new Note()
            {
                Id = note.Id,
                Content = note.Content,
                Date = note.Date,
                Category = SerializedCategory.DeserializeCategory(note.Category)
            };
        }
        #endregion
    }

    [Serializable]
    class SerializedNote
    {
        public Guid Id;
        public string Content;
        public string Date;
        public SerializedCategory Category;
    }
}
