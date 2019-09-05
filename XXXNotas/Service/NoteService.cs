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

        public NoteService()
        {
            _notes = new List<Note>();
        }

        #region INoteService
        public void Delete(Note note)
        {
            _notes.Remove(note);
        }

        public IList<Note> FindAll()
        {
            return _notes;
        }

        public void Reset()
        {
            _notes = new List<Note>();
        }

        public void Save(Note note)
        {
            _notes.Add(note);
        }
        #endregion
    }
}
