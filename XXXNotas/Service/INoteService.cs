﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XXXNotas.Model;

namespace XXXNotas.Service
{
    public interface INoteService
    {
        void Save(Note note);

        void Delete(Note note);

        void Reset();

        IList<Note> FindAll();

        void SaveAll(List<Note> notes);
    }
}
