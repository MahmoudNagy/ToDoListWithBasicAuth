using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using ToDoList.Core.Models;

namespace ToDoList.Services.Services.Interface
{
    public interface INotesService
    {
        Notes AddNote(Notes note);
        int EditNote(Notes note);
        bool DeleteNote(int noteId);
        List<Notes> GetNotes(string userId);
        Notes GetNote(int noteId, string userId);
        string UploadFile(IFormFile file);
        bool DeleteFile(string fileId);
    }
}
