using BookLibrary.BE;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BookLibrary.DAL;

public class BookRepository
{
    public BookRepository()
    {
       var OptionsBuilder = new DbContextOptions<LibraryContext>();
    }
    
    public List<Book> GetBooks()
    {
        List<Book> books = new List<Book>();
        using (LibraryContext context = new LibraryContext())
        {
            books = context.Books.ToList();
        }
        return books;
    }

    public void AddBook(Book book)
    {
        using (LibraryContext context = new LibraryContext())
        {
            context.Books.Add(book);
            context.SaveChanges();
        }
    }

    public void UpdateBook(Book book)
    {
        using (LibraryContext context = new LibraryContext())
        {
            context.Books.Update(book);
            context.SaveChanges();
        }
    }

    public void DeleteBook(Book book)
    {
        using (LibraryContext context = new LibraryContext())
        {
            context.Books.Remove(book);
            context.SaveChanges();
        }
    }
    
    
}