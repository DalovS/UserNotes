﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserNotes.Models;

namespace UserNotes.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Note> Notes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<NoteTag> NoteTags { get; set; }
        public DbSet<Category> Categories { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<NoteTag>()
                .HasKey(nt=> new {nt.NoteId,nt.TagId});

            modelBuilder.Entity<NoteTag>()
                .HasOne(nt => nt.Note)
                .WithMany(n => n.NoteTags)
                .HasForeignKey(nt => nt.NoteId);

            modelBuilder.Entity<NoteTag>()
                .HasOne(nt => nt.Tag)
                .WithMany(t => t.NoteTags)
                .HasForeignKey(nt => nt.TagId);
        }
    }
}
