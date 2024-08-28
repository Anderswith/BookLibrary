using BookLibrary.BE;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BookLibrary.DAL;

public class BookRepository
{
    private LibraryContext context;

    public BookRepository()
    {
        var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
        context = new LibraryContext(optionsBuilder.Options); 
    }
    
    public List<Book> GetBooks()
    {
        return context.Books.ToList();
    }

    public void AddBook(Book book)
    {
        context.Books.Add(book);
        context.SaveChanges();
    }

    public void UpdateBook(Book book)
    {
        context.Books.Update(book);
        context.SaveChanges();
        
    }

    public void DeleteBook(Book book)
    {
        context.Books.Remove(book);
        context.SaveChanges();
    }
    public Book GetBookByTitle(string title)
    {
        return context.Books.SingleOrDefault(b => b.title.Equals(title, StringComparison.OrdinalIgnoreCase));
    }
    public List<Book> GetBooksLoanedByUser(Guid userId)
    {
        return context.Books.Where(b => b.UserID == userId).ToList();
    }
    
    public void LoanBook(Guid userId, double bookIsbn)
    {
        var book = context.Books.SingleOrDefault(b => b.ISBN == bookIsbn);
        book.inStock = book.inStock;
        book.UserID = userId;
        context.SaveChanges(); 
    }

    public void ReturnBook(double bookIsbn)
    {
        var book = context.Books.SingleOrDefault(b => b.ISBN == bookIsbn);
        book.inStock = "Book is available";
        book.UserID = null;
        context.SaveChanges();
    }
    
    
}