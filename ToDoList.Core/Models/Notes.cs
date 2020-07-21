using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Core.Models
{
    public class Notes
    {
        public int Id { get; set; }
        public string Note { get; set; }
        public DateTime NoteTime { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public string FileId { get; set; }
        public string FileName { get; set; }


    }
}
