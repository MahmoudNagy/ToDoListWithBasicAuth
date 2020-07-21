using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToDoList.Core.Models;
using ToDoList.Infrastructure.Repositories.Interface;

namespace ToDoList.Infrastructure.Repositories
{
    public class NotesRepository : INotesRepository
    {
        private ToDoListDbContext _dbContext;
        public NotesRepository(ToDoListDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Notes AddNote(Notes note)
        {
            _dbContext.Notes.Add(note);
            _dbContext.SaveChanges();
            return note;
        }

        public int EditNote(Notes note)
        {
            _dbContext.Notes.Update(note);
            _dbContext.SaveChanges();
            return note.Id;
        }

        public bool DeleteNote(Notes note)
        {
            _dbContext.Notes.Remove(note);
            _dbContext.SaveChanges();
            return true;
        }

        public Notes GetNote(int noteId)
        {
            return _dbContext.Notes.FirstOrDefault(n => n.Id == noteId);
        }

        public List<Notes> GetNotes(string userId)
        {
            return _dbContext.Notes.Where(x => x.UserId == userId).ToList();
        }

        public Notes GetNote(int noteId, string userId)
        {
            return _dbContext.Notes.FirstOrDefault(x => x.UserId == userId && x.Id == noteId);
        }
    }
}
