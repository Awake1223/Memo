using System.Collections.Generic;
using System.Reflection.Emit;
using Memo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Memo.Infrastructure
{
    public class MemoDbContext : DbContext
    {
        public MemoDbContext(DbContextOptions<MemoDbContext> options) : base(options) { }

        public MemoDbContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=MiniPastebin;Username=postgres;Password=postgres");
            }
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<NoteEntity> Notes { get; set; }
        public DbSet<TagEntity> Tags { get; set; }          
        public DbSet<NoteTagEntity> NoteTags { get; set; }  
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Уникальный индекс для ShortCode
            modelBuilder.Entity<NoteEntity>()
                .HasIndex(n => n.ShortCode)
                .IsUnique();

            // Индекс для быстрого поиска по UserId
            modelBuilder.Entity<NoteEntity>()
                .HasIndex(n => n.UserId);

            // Связь User -> Notes (один ко многим)
            modelBuilder.Entity<NoteEntity>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notes)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.SetNull); // если пользователь удалён, заметки остаются анонимными

            // Уникальный индекс для тегов (чтобы не было дублей)
            modelBuilder.Entity<TagEntity>()
                .HasIndex(t => t.Name)
                .IsUnique();

            // Составной ключ для связи Note-Tag
            modelBuilder.Entity<NoteTagEntity>()
                .HasKey(nt => new { nt.NoteId, nt.TagId });

            // Связи
            modelBuilder.Entity<NoteTagEntity>()
                .HasOne(nt => nt.Note)
                .WithMany(n => n.NoteTags)
                .HasForeignKey(nt => nt.NoteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NoteTagEntity>()
                .HasOne(nt => nt.Tag)
                .WithMany(t => t.NoteTags)
                .HasForeignKey(nt => nt.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);

        }
    }
}
