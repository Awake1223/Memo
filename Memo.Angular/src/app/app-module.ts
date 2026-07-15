import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MarkdownModule } from 'ngx-markdown';
import { TagCloudComponent } from './components/tag-cloud/tag-cloud.component';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';

// Components
import { HeaderComponent } from './components/header/header.component';
import { HomeComponent } from './pages/home/home.component';
import { ViewNoteComponent } from './pages/view-note/view-note.component';
import { MyNotesComponent } from './pages/my-notes/my-notes.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { EditNoteComponent } from './pages/edit-note/edit-note.component';

// Interceptors
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { TagService } from './services/tag.service';

@NgModule({
  declarations: [
    App,
    HeaderComponent,
    HomeComponent,
    ViewNoteComponent,
    MyNotesComponent,
    LoginComponent,
    RegisterComponent,
    EditNoteComponent,
    TagCloudComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule,
    MarkdownModule.forRoot()
  ],
  providers: [
    TagService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [App]
})
export class AppModule { }
