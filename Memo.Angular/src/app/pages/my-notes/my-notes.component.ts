import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NoteService } from '../../services/note.service';
import { NoteResponse } from '../../models/note.models';
import { SearchService, SearchParams } from '../../services/search.service';

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

  // Параметры поиска
  searchQuery = '';
  selectedTag = '';
  sortBy: 'relevance' | 'newest' | 'popular' = 'newest';
  currentPage = 1;
  pageSize = 20;
  totalNotes = 0;
  totalPages = 0;

  // Доступные теги для фильтра (опционально)
  availableTags: string[] = [];

  constructor(
    private noteService: NoteService,
    private searchService: SearchService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    // Подписываемся на изменение параметров URL
    this.route.queryParams.subscribe(params => {
      this.selectedTag = params['tag'] || '';
      this.searchQuery = params['search'] || '';
      this.currentPage = parseInt(params['page']) || 1;
      this.sortBy = params['sort'] || 'newest';
      this.loadNotes();
    });
  }

  loadNotes(): void {
    this.loading = true;
    this.errorMessage = '';

    this.noteService.getMyNotes().subscribe({
      next: (response) => {
        // Фильтрация на клиенте
        let filtered = response;

        // Поиск по тексту
        if (this.searchQuery) {
          const query = this.searchQuery.toLowerCase();
          filtered = filtered.filter(n =>
            n.content.toLowerCase().includes(query) ||
            n.shortCode.toLowerCase().includes(query)
          );
        }

        // Фильтр по тегу
        if (this.selectedTag) {
          filtered = filtered.filter(n =>
            n.tags?.some(t => t.name.toLowerCase() === this.selectedTag.toLowerCase())
          );
        }

        // Сортировка
        switch (this.sortBy) {
          case 'newest':
            filtered.sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime());
            break;
          case 'popular':
            filtered.sort((a, b) => (b.viewCount || 0) - (a.viewCount || 0));
            break;
          default: // relevance
            break;
        }

        this.notes = filtered;
        this.totalNotes = filtered.length;
        this.totalPages = Math.ceil(this.totalNotes / this.pageSize);
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

  private loadNotesSimple(): void {
    this.noteService.getMyNotes().subscribe({
      next: (response) => {
        this.notes = response;
        this.loading = false;
        this.totalNotes = response.length;
        this.totalPages = 1;
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

  private loadNotesWithSearch(): void {
    const params: SearchParams = {
      query: this.searchQuery || undefined,
      tag: this.selectedTag || undefined,
      page: this.currentPage,
      pageSize: this.pageSize,
      sortBy: this.sortBy
    };

    this.searchService.search(params).subscribe({
      next: (response) => {
        this.notes = response;
        this.loading = false;
        // TODO: когда бэкенд будет возвращать totalCount — обновить
        this.totalNotes = response.length;
        this.totalPages = 1;
      },
      error: (err) => {
        this.errorMessage = 'Ошибка поиска';
        this.loading = false;
        if (err.status === 401) {
          this.router.navigate(['/login']);
        }
      }
    });
  }

  // Поиск
  onSearch(): void {
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {
        search: this.searchQuery || undefined,
        tag: this.selectedTag || undefined,
        page: 1,
        sort: this.sortBy
      },
      queryParamsHandling: 'merge'
    });
  }

  // Очистка фильтров
  clearFilters(): void {
    this.searchQuery = '';
    this.selectedTag = '';
    this.sortBy = 'newest';
    this.currentPage = 1;
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {
        search: undefined,
        tag: undefined,
        page: 1,
        sort: 'newest'
      },
      queryParamsHandling: 'merge'
    });
  }

  // Выбор тега (клик по тегу)
  selectTag(tagName: string): void {
    this.selectedTag = tagName;
    this.searchQuery = '';
    this.onSearch();
  }

  // Удаление заметки
  deleteNote(shortCode: string): void {
    if (!confirm('Удалить эту заметку?')) return;

    this.deletingCode = shortCode;
    this.noteService.deleteNote(shortCode).subscribe({
      next: () => {
        this.notes = this.notes.filter(n => n.shortCode !== shortCode);
        this.deletingCode = null;
        this.totalNotes--;
      },
      error: () => {
        this.deletingCode = null;
        this.errorMessage = 'Ошибка удаления заметки';
      }
    });
  }

  // Смена страницы
  goToPage(page: number): void {
    if (page < 1 || page > this.totalPages) return;
    this.currentPage = page;
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { page },
      queryParamsHandling: 'merge'
    });
  }

  // Форматирование даты
  formatDate(date: string): string {
    return new Date(date).toLocaleString('ru-RU', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  // Проверка истечения
  isExpired(expiresAt: string | null): boolean {
    if (!expiresAt) return false;
    return new Date(expiresAt) < new Date();
  }

  // Статус заметки
  getStatusLabel(expiresAt: string | null): string {
    if (!expiresAt) return '♾️ Навсегда';
    if (this.isExpired(expiresAt)) return '⏰ Истекла';
    return '⏳ Активна';
  }

  // CSS класс для статуса
  getStatusClass(expiresAt: string | null): string {
    if (!expiresAt) return 'text-success';
    if (this.isExpired(expiresAt)) return 'text-danger';
    return 'text-success';
  }

  // CSS класс для тегов
  getTagClass(tagName: string): string {
    const colors = ['primary', 'secondary', 'success', 'danger', 'warning', 'info'];
    const hash = tagName.split('').reduce((acc, char) => acc + char.charCodeAt(0), 0);
    return colors[hash % colors.length];
  }
}
