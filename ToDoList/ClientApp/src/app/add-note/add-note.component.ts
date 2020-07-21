import { Component, OnInit } from '@angular/core';
import { Note } from '../model/note.model';
import { AppService } from '../app.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-add-note',
  templateUrl: './add-note.component.html',
  styleUrls: ['./add-note.component.css']
})
export class AddNoteComponent implements OnInit {

  form: Note = new Note();
  date = new Date();
  today: any = new Date(this.date.getTime() - (this.date.getTimezoneOffset() * 60000)).toISOString();
  dateError: string;
  dateErrorExist: boolean;

  displayModes = {
    view: 'view', loading: 'loading', empty: 'empty', error: 'error', succeed: 'succeed'
  };
  displayMode = this.displayModes.view;
  noteId: number;
  existFileId: string;
  imagesExtension: string[] = ['jpg', 'png', 'jpeg'];

  constructor(private router: Router, private appService: AppService, private readonly route: ActivatedRoute) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.noteId = params['id'];
      if (this.noteId !== undefined) {
        this.displayMode = this.displayModes.loading;
        this.appService.getNote(this.noteId).subscribe(response => {
          this.displayMode = this.displayModes.view;
          if (response) {
            this.form = response as Note;
            this.existFileId = (response as Note).fileId;
          }
          else
            this.displayMode = this.displayModes.error;

        }, err => {
          this.displayMode = this.displayModes.error;
          console.log('Error in get note.');
          console.log(err);
        });
      }
    });
  }

  save() {
    this.displayMode = this.displayModes.loading;
    if (this.noteId !== undefined) {
      this.appService.editNote(this.form).subscribe(response => {
        this.displayMode = this.displayModes.view;
        if (response) {
          this.router.navigate(['/']);
        }
        else
          this.displayMode = this.displayModes.error;

      }, err => {
        this.displayMode = this.displayModes.error;
        console.log('Error in edit note.');
        console.log(err);
      });
    }
    else {
      this.appService.addNote(this.form).subscribe(response => {
        this.displayMode = this.displayModes.view;
        if (response) {
          this.router.navigate(['/']);
        }
        else
          this.displayMode = this.displayModes.error;

      }, err => {
        this.displayMode = this.displayModes.error;
        console.log('Error in save new note.');
        console.log(err);
      });
    }
  }

  upload(files) {
    if (files.length === 0) {
      return;
    }
    const formData = new FormData();
    for (const file of files) {
      formData.append(file.name, file);
      const extension = file.name.split('.').pop();
      if (!this.imagesExtension.includes(extension.toLowerCase()))
        return;
    }
    this.appService.uploadFile(formData).subscribe(result => {
      let file = JSON.parse(result as string);
      this.form.fileName = file.fileName;
      this.form.fileId = file.fileId;

    }, err => {
      console.log('Error in upload image.');
      console.log(err);
    });
  }

  delete(fileId) {
    if (this.noteId === undefined || fileId != this.existFileId) {
      this.appService.deleteFile(fileId).subscribe(response => {
        this.form.fileId = null;
        this.form.fileName = null;
      }, err => {
        this.displayMode = this.displayModes.view;
        console.log('Error in delete image.');
        console.log(err);
      });
    }
    else {
      this.form.fileId = null;
      this.form.fileName = null;
    }
  }

  checkDate() {
    this.dateError = '';
    this.dateErrorExist = false;
    if (this.today > this.form.noteTime) {
      this.dateError = "You must choose date before now";
      this.dateErrorExist = true;
    }
  }


}



