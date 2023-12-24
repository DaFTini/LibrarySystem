using AutoLibraryUI.Entities;
using Microsoft.EntityFrameworkCore;
namespace AutoLibraryUI.Data;

public class LibraryDbContext : DbContext
{
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Reader> Readers => Set<Reader>();
    public DbSet<Librarian> Librarians => Set<Librarian>();
    
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Librarian>()
            .HasIndex(x => x.Username)
            .IsUnique();

        modelBuilder
            .Entity<Reader>()
            .HasMany(x => x.Bookings)
            .WithOne(x => x.Reader)
            .HasForeignKey(x => x.ReaderId);

        modelBuilder
            .Entity<Book>()
            .HasMany(x => x.Bookings)
            .WithOne(x => x.Book)
            .HasForeignKey(x => x.BookId);

        base.OnModelCreating(modelBuilder);
    }
}