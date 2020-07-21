import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AppService {

  constructor(private http: HttpClient) { }

  addNote(note) {
    return this.http.post('/api/NotesApi/addNote', note);
  }

  editNote(note) {
    return this.http.post('/api/NotesApi/editNote', note);
  }

  deleteNote(noteId) {
    return this.http.delete('/api/NotesApi/deleteNote/' + noteId);
  }

  getNotes() {
    return this.http.get('/api/NotesApi/getNotes');
  }

  getNote(noteId) {
    return this.http.get('/api/NotesApi/getNote/' + noteId);
  }

  deleteFile(fileId) {
    return this.http.delete('/api/NotesApi/deleteFile/' + fileId);
  }

  uploadFile(formData) {
    return this.http.post('/api/NotesApi/upload', formData);
  }

}
