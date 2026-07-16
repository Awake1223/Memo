import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TagDto } from '../models/tag.models';
import { AuthService } from './auth.service';
import { environment } from '../environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class TagService {
  private apiUrl = `${environment.apiUrl}/Tags`;

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) { }

  getPopularTags(limit: number = 10): Observable<TagDto[]> {
    return this.http.get<TagDto[]>(`${this.apiUrl}/popular?limit=${limit}`, {
      headers: { Authorization: `Bearer ${this.authService.getToken()}` }
    });
  }
}
