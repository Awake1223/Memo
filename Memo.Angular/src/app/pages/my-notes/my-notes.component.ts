import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NoteService } from '../../services/note.service';
import { NoteResponse } from '../../models/note.models';

@Component({
  selector: 'app-my-notes',
  standalone: false,
  templateUrl: './my-notes.component.html',
  styleUrls: ['./my-notes.component.css']
})
export class MyNotesComponent implements OnInit {
  notes: NoteResponse[] = [];
  loading = true;
  errorMessage = '';
  deletingCode: string | null = null;

  constructor(
    private noteService: NoteService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadNotes();
  }

  loadNotes(): void {
    this.loading = true;
    this.errorMessage = '';

    this.noteService.getMyNotes().subscribe({
      next: (response) => {
        this.notes = response;
        this.loading = false;
      },
      error: (err) => {
        this.errorMessage = 'Ошибка загрузки заметок';
        this.loading = false;
        if (err.status === 401) {
          this.router.navigate(['/login']);
        }
      }
    });
  }

  deleteNote(shortCode: string): void {
    if (!confirm('Удалить эту заметку?')) return;

    this.deletingCode = shortCode;
    this.noteService.deleteNote(shortCode).subscribe({
      next: () => {
        this.notes = this.notes.filter(n => n.shortCode !== shortCode);
        this.deletingCode = null;
      },
      error: () => {
        this.deletingCode = null;
        this.errorMessage = 'Ошибка удаления заметки';
      }
    });
  }

  formatDate(date: string): string {
    return new Date(date).toLocaleString('ru-RU');
  }

  isExpired(expiresAt: string | null): boolean {
    if (!expiresAt) return false;
    return new Date(expiresAt) < new Date();
  }

  getStatusLabel(expiresAt: string | null): string {
    if (!expiresAt) return '♾️ Навсегда';
    if (this.isExpired(expiresAt)) return '⏰ Истекла';
    return '⏳ Активна';
  }

  getStatusClass(expiresAt: string | null): string {
    if (!expiresAt) return 'text-success';
    if (this.isExpired(expiresAt)) return 'text-danger';
    return 'text-success';
  }
}
