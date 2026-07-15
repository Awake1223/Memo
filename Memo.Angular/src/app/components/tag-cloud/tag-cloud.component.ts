import { Component, OnInit } from '@angular/core';
import { TagService } from '../../services/tag.service';
import { TagDto } from '../../models/tag.models';

@Component({
  selector: 'app-tag-cloud',
  templateUrl: './tag-cloud.component.html',
  standalone: false,
  styleUrls: ['./tag-cloud.component.css']
})
export class TagCloudComponent implements OnInit {
  tags: TagDto[] = [];
  loading = false;
  error = '';

  constructor(private tagService: TagService) { }

  ngOnInit(): void {
    this.loadTags();
  }

  loadTags(): void {
    this.loading = true;
    this.tagService.getPopularTags(15).subscribe({
      next: (tags) => {
        this.tags = tags;
        this.loading = false;
      },
      error: () => {
        this.error = 'Не удалось загрузить теги';
        this.loading = false;
      }
    });
  }

  // Функция для размера шрифта (популярные теги — крупнее)
  getFontSize(count: number): string {
    const maxCount = Math.max(...this.tags.map(t => t.count), 1);
    const minFont = 12;
    const maxFont = 28;
    const size = minFont + (count / maxCount) * (maxFont - minFont);
    return `${size}px`;
  }

  getColor(count: number): string {
    const maxCount = Math.max(...this.tags.map(t => t.count), 1);
    const ratio = count / maxCount;
    // от синего к фиолетовому
    const hue = 220 - ratio * 80;
    return `hsl(${hue}, 70%, 50%)`;
  }
}
