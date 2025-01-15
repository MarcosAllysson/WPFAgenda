using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WPFAgenda.Models;

public partial class WpfAgendaDbContext : DbContext
{
    public WpfAgendaDbContext()
    {
    }

    public WpfAgendaDbContext(DbContextOptions<WpfAgendaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contact> Contacts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=WpfAgendaDB;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Contacts__3214EC0763A726FA");

            entity.Property(e => e.Email)
                .HasMaxLength(90)
                .IsFixedLength();
            entity.Property(e => e.Name)
                .HasMaxLength(90)
                .IsFixedLength();
            entity.Property(e => e.Phone)
                .HasMaxLength(30)
                .IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
