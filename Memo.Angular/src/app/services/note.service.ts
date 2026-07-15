import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateNoteRequest, NoteResponse, CreateNoteResponse } from '../models/note.models';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class NoteService {
  private apiUrl = '/api/Notes';

  constructor(private http: HttpClient, private authService: AuthService) { }

  private getHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Authorization': token ? `Bearer ${token}` : ''
    });
  }

  createNote(data: CreateNoteRequest): Observable<CreateNoteResponse> {
    return this.http.post<CreateNoteResponse>(this.apiUrl, data, {
      headers: this.getHeaders()
    });
  }

  getNote(shortCode: string): Observable<NoteResponse> {
    return this.http.get<NoteResponse>(`${this.apiUrl}/${shortCode}`);
  }

  getMyNotes(): Observable<NoteResponse[]> {
    return this.http.get<NoteResponse[]>(`${this.apiUrl}/my`, {
      headers: this.getHeaders()
    });
  }

  deleteNote(shortCode: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${shortCode}`, {
      headers: this.getHeaders()
    });
  }

  updateNote(shortCode: string, content: string): Observable<any> {
    return this.http.put(`${this.apiUrl}/${shortCode}`, { content }, {
      headers: this.getHeaders()
    });
  }
}
