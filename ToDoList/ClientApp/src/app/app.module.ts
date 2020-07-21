import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { AuthorizeGuard } from 'src/authorization/authorize.guard';
import { AuthorizeInterceptor } from 'src/authorization/authorize.interceptor';
import { AddNoteComponent } from './add-note/add-note.component';
import { NotesComponent } from './notes/notes.component';
import { LoginComponent } from '../authorization/login/login.component';
import { RegisterComponent } from '../authorization/register/register.component';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    AddNoteComponent,
    NotesComponent,
    LoginComponent,
    RegisterComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', redirectTo: 'notes', pathMatch: 'full' },
      { path: 'note', component: AddNoteComponent, canActivate: [AuthorizeGuard] },
      { path: 'note/:id', component: AddNoteComponent, canActivate: [AuthorizeGuard] },
      { path: 'notes', component: NotesComponent, canActivate: [AuthorizeGuard] },
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent }

    ])
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
