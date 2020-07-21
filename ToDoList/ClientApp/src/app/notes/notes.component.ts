import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AppService } from '../app.service';
import { Note } from '../model/note.model';
@Component({
  selector: 'app-notes',
  templateUrl: './notes.component.html',
  styleUrls: ['./notes.component.css']
})
export class NotesComponent implements OnInit {

  notes: Note[];
  displayModes = {
    view: 'view', loading: 'loading', empty: 'empty', error: 'error', succeed: 'succeed'
  };
  displayMode = this.displayModes.view;
  date = new Date();
  today: any = new Date(this.date.getTime() - (this.date.getTimezoneOffset() * 60000)).toISOString();

  constructor(private router: Router,
    private appService: AppService) { }

  ngOnInit() {
    this.displayMode = this.displayModes.loading;
    this.appService.getNotes().subscribe(result => {
      this.notes = result as Note[];
      this.notes.forEach(n => {
        if (this.today > n.noteTime)
          n.isTimeOver = true;
      })
      this.displayMode = this.displayModes.view;
    }, err => {
      console.log(err);
      this.displayMode = this.displayModes.error;
      console.log('Error in get notes.');
      console.log(err);
    });
  }



  add() {
    this.router.navigate(['/note']);
  }

  edit(note) {
    this.router.navigate(['/note', note.id]);
  }

  delete(note) {
    this.appService.deleteNote(note.id).subscribe(result => {
      this.notes.splice(this.notes.indexOf(note), 1);
      this.displayMode = this.displayModes.view;
    }, err => {
      console.log(err);
      this.displayMode = this.displayModes.error;
      console.log('Error in delete note.');
      console.log(err);
    });
  }




}

