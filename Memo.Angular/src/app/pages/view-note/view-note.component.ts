import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NoteService } from '../../services/note.service';
import { NoteResponse } from '../../models/note.models';

@Component({
  selector: 'app-view-note',
  standalone: false,
  templateUrl: './view-note.component.html',
  styleUrls: ['./view-note.component.css']
})
export class ViewNoteComponent implements OnInit {
  note: NoteResponse | null = null;
  loading = true;
  errorMessage = '';
  shortCode = '';

  constructor(
    private route: ActivatedRoute,
    private noteService: NoteService
  ) { }

  ngOnInit(): void {
    this.shortCode = this.route.snapshot.paramMap.get('shortCode') || '';
    if (this.shortCode) {
      this.loadNote();
    } else {
      this.errorMessage = 'Не указан код заметки';
      this.loading = false;
    }
  }

  loadNote(): void {
    this.loading = true;
    this.errorMessage = '';

    this.noteService.getNote(this.shortCode).subscribe({
      next: (response) => {
        this.note = response;
        this.loading = false;
      },
      error: (err) => {
        if (err.status === 404) {
          this.errorMessage = 'Заметка не найдена или истекла';
        } else {
          this.errorMessage = 'Ошибка загрузки заметки';
        }
        this.loading = false;
      }
    });
  }

  formatDate(date: string): string {
    return new Date(date).toLocaleString('ru-RU');
  }

  isExpired(): boolean {
    if (!this.note) return false;
    if (!this.note.expiresAt) return false;
    return new Date(this.note.expiresAt) < new Date();
  }
}
