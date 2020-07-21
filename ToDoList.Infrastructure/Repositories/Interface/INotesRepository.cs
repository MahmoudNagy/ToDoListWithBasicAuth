using System;
using System.Collections.Generic;
using System.Text;
using ToDoList.Core.Models;

namespace ToDoList.Infrastructure.Repositories.Interface
{
    public interface INotesRepository
    {
        Notes AddNote(Notes note);
        int EditNote(Notes note);
        bool DeleteNote(Notes note);
        Notes GetNote(int noteId);
        List<Notes> GetNotes(string userId);
        Notes GetNote(int noteId, string userId);
    }
}
