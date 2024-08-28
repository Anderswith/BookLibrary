using BookLibrary.BE;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.DAL;

public class LibraryContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Users> Users {get; set; }
    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=EASV-DB4;Database=AndersW_Library;User Id=CSe2023t_t_5;Password=CSe2023tT5#23");
    }
    
}