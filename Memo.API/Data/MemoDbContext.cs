using System.Collections.Generic;
using System.Reflection.Emit;
using Memo.API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Memo.API.Data
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

            base.OnModelCreating(modelBuilder);
        }
    }
}
