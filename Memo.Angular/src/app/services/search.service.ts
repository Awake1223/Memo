import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { NoteResponse } from '../models/note.models';
import { AuthService } from './auth.service';

export interface SearchParams {
  query?: string;
  tag?: string;
  page?: number;
  pageSize?: number;
  sortBy?: 'relevance' | 'newest' | 'popular';
}

@Injectable({
  providedIn: 'root'
})
export class SearchService {
  private apiUrl = '/api/Search';

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) { }

  search(params: SearchParams): Observable<NoteResponse[]> {
    const headers = { Authorization: `Bearer ${this.authService.getToken()}` };
    return this.http.get<NoteResponse[]>(this.apiUrl, {
      headers,
      params: { ...params }
    });
  }
}
