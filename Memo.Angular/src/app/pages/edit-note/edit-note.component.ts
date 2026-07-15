import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NoteService } from '../../services/note.service';

@Component({
  selector: 'app-edit-note',
  templateUrl: './edit-note.component.html',
  standalone: false,
  styleUrls: ['./edit-note.component.css']
})
export class EditNoteComponent implements OnInit {
  shortCode = '';
  content = '';
  loading = false;
  errorMessage = '';
  successMessage = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private noteService: NoteService
  ) { }

  ngOnInit(): void {
    this.shortCode = this.route.snapshot.paramMap.get('shortCode') || '';
    if (this.shortCode) {
      this.loadNote();
    }
  }

  loadNote(): void {
    this.loading = true;
    this.noteService.getNote(this.shortCode).subscribe({
      next: (note) => {
        this.content = note.content;
        this.loading = false;
      },
      error: () => {
        this.errorMessage = 'Не удалось загрузить заметку';
        this.loading = false;
      }
    });
  }

  onSubmit(): void {
    if (!this.content.trim()) {
      this.errorMessage = 'Введите текст заметки';
      return;
    }

    this.loading = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.noteService.updateNote(this.shortCode, this.content).subscribe({
      next: () => {
        this.successMessage = 'Заметка обновлена!';
        this.loading = false;
        setTimeout(() => this.router.navigate(['/my-notes']), 1500);
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Ошибка обновления';
        this.loading = false;
      }
    });
  }
}
