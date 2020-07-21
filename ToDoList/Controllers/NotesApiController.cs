using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoList.Services.Services.Interface;
using ToDoList.Core.Models;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotesApiController : ControllerBase
    {

        private INotesService _notesService;

        public NotesApiController(INotesService notesService)
        {
            _notesService = notesService;
        }

        [HttpPost]
        [Route("addNote")]
        public ActionResult<Notes> AddNote([FromBody] Notes note)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            note.UserId = userId;

            Notes newNote = _notesService.AddNote(note);

            if (newNote == null)
                return BadRequest();

            return Ok(newNote);
        }

        [HttpPost]
        [Route("editNote")]
        public ActionResult<int> EditNote([FromBody]Notes note)
        {
            int result = _notesService.EditNote(note);

            if (result == 0)
                return BadRequest();

            return Ok(result);
        }

        [HttpDelete]
        [Route("deleteNote/{noteId}")]
        public ActionResult<bool> DeleteNote(int noteId)
        {
            bool result = _notesService.DeleteNote(noteId);

            if (result == false)
                return BadRequest();

            return Ok(result);
        }

        [HttpGet]
        [Route("getNotes")]
        public ActionResult<List<Notes>> GetNotes()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<Notes> notes = _notesService.GetNotes(userId);

            if (notes == null)
                return BadRequest();

            return Ok(notes);
        }

        [HttpGet]
        [Route("getNote/{noteId}")]
        public ActionResult<Notes> GetNotes(int noteId)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            Notes note = _notesService.GetNote(noteId, userId);

            if (note == null)
                return BadRequest();

            return Ok(note);
        }



        [HttpPost]
        [Route("upload")]
        [Produces("application/json")]
        public ActionResult<string> Upload()
        {
            string result = null;
            var file = Request.Form.Files[0];
            if (file != null && file.Length > 0)
                result = _notesService.UploadFile(file);

            if (string.IsNullOrEmpty(result))
                return BadRequest();

            return Ok(result);
        }

        [HttpDelete]
        [Route("deleteFile/{fileId}")]
        public ActionResult<bool> DeleteFile(string fileId)
        {
            bool result = _notesService.DeleteFile(fileId);

            if (result == false)
                return BadRequest();

            return Ok(result);
        }
    }
}