import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';

// Components
import { HeaderComponent } from './components/header/header.component';
import { HomeComponent } from './pages/home/home.component';
import { ViewNoteComponent } from './pages/view-note/view-note.component';
import { MyNotesComponent } from './pages/my-notes/my-notes.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';

// Interceptors
import { AuthInterceptor } from './interceptors/auth.interceptor';

@NgModule({
  declarations: [
    App,
    HeaderComponent,
    HomeComponent,
    ViewNoteComponent,
    MyNotesComponent,
    LoginComponent,
    RegisterComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [App]
})
export class AppModule { }
