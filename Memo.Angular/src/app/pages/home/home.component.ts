import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { NoteService } from '../../services/note.service';
import { NoteLifetime } from '../../models/note.models';

@Component({
  selector: 'app-home',
  standalone: false,
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  content = '';
  lifetime: NoteLifetime = NoteLifetime.OneHour;
  loading = false;
  errorMessage = '';
  createdNoteCode: string | null = null;

  lifetimes = [
    { value: NoteLifetime.OneHour, label: '1 час' },
    { value: NoteLifetime.OneDay, label: '1 день' },
    { value: NoteLifetime.Forever, label: 'Навсегда' }
  ];

  constructor(
    private noteService: NoteService,
    private router: Router
  ) { }

  onSubmit(): void {
    if (!this.content.trim()) {
      this.errorMessage = 'Введите текст заметки';
      return;
    }

    this.loading = true;
    this.errorMessage = '';
    this.createdNoteCode = null;

    this.noteService.createNote({
      content: this.content,
      lifetime: this.lifetime
    }).subscribe({
      next: (response) => {
        this.createdNoteCode = response.shortCode;
        this.loading = false;
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Ошибка создания заметки';
        this.loading = false;
      }
    });
  }

  getNoteUrl(): string {
    return `${window.location.origin}/note/${this.createdNoteCode}`;
  }

  copyToClipboard(): void {
    navigator.clipboard?.writeText(this.getNoteUrl());
    alert('Ссылка скопирована!');
  }
}
