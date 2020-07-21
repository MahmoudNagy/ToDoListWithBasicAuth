using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using ToDoList.Infrastructure.Repositories.Interface;
using ToDoList.Services.Services.Interface;
using Microsoft.AspNetCore.Hosting;
using ToDoList.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using Newtonsoft.Json;

namespace ToDoList.Services.Services
{
    public class NotesService : INotesService
    {
        private readonly INotesRepository _notesRepository;
        private IConfiguration _configuration { get; set; }
        private readonly IHostingEnvironment _hostingEnvironment;
        public ILogger<NotesService> _logger { get; }
        public NotesService(INotesRepository notesRepository, IConfiguration configuration, IHostingEnvironment hostingEnvironment, ILogger<NotesService> logger)
        {
            _notesRepository = notesRepository;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        public Notes AddNote(Notes note)
        {
            try
            {
                Notes newNotes = _notesRepository.AddNote(note);
                return note;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in add new note");
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public int EditNote(Notes note)
        {
            try
            {
                Notes editedNote = _notesRepository.GetNote(note.Id);

                if (editedNote == null)
                    return 0;

                //delete old file in case of changng
                if (editedNote.FileId != note.FileId)
                    DeleteFile(editedNote.FileId);
                editedNote.Note = note.Note;
                editedNote.NoteTime = note.NoteTime;
                editedNote.FileId = note.FileId;
                editedNote.FileName = note.FileName;
                int result = _notesRepository.EditNote(editedNote);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in edit note with id {0}", note.Id);
                _logger.LogError(ex.Message);
                return 0;
            }
        }

        public bool DeleteNote(int noteId)
        {
            try
            {
                Notes note = _notesRepository.GetNote(noteId);
                if (note == null)
                    return false;

                bool result = false;
                result = DeleteFile(note.FileId);

                if (result)
                    result = _notesRepository.DeleteNote(note);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in delete note with id {0}", noteId);
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public string UploadFile(IFormFile file)
        {
            try
            {
                string filesFolderName = _configuration.GetSection("DefaultInfo").GetSection("FolderName").Value;
                string fileId = Guid.NewGuid().ToString();

                string webRootPath = _hostingEnvironment.WebRootPath;
                string newPath = Path.Combine(webRootPath, filesFolderName, fileId);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }

                string fileName = file.FileName;
                string fullPath = Path.Combine(newPath, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                return JsonConvert.SerializeObject(new { fileId, fileName });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in upload file");
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public bool DeleteFile(string fileId)
        {
            try
            {
                string filesFolderName = _configuration.GetSection("DefaultInfo").GetSection("FolderName").Value;
                string webRootPath = _hostingEnvironment.WebRootPath;

                var path = Path.Combine(webRootPath, filesFolderName, fileId);
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in delete file : {0}", fileId);
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public List<Notes> GetNotes(string userId)
        {
            try
            {
                List<Notes> notes = _notesRepository.GetNotes(userId);
                return notes;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in get notes of user {0}", userId);
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public Notes GetNote(int noteId, string userId)
        {
            try
            {
                Notes note = _notesRepository.GetNote(noteId, userId);
                return note;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in get note : {0}", noteId);
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
