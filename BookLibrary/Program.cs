using System.Collections;
using System.Text.Json;
using BookLibrary.BE;
using BookLibrary.Util;

namespace BookLibrary;

class Program
{
    
    static void Main(string[] args)
    {
        Start();
        
    }

    private static void Start()
    {
        
        Book book = new Book();
        List<Book> books = new List<Book>();
        Console.WriteLine("Hello, welcome to the Book Library! Please choose one of the following options:" +
                          "\n 1. Add book \n 2. Delete book \n 3. Update book \n 4. Loan book");
        PickChoice(book, books);
    }


    private static void PickChoice(Book book, List<Book> books)
    {
        int choice = Convert.ToInt32(Console.ReadLine());
        switch (choice)
        {
            case 1:
                AddBook(book);
                Start();
                break;
            case 2:
                ShowBooks();
                Console.WriteLine("Enter the title of the book you want to delete:");
                string bookToDelete = Console.ReadLine();
                DeleteBook(bookToDelete);
                Start();
                break;
            case 3:
                Start();
                break;
            case 4:
                Start();
                break;
        }
    }

    private static void AddBook(Book book)
    {
        
        Console.WriteLine("Enter book title:");
        book.title = Console.ReadLine();
        Console.WriteLine("Enter book author:");
        book.author = Console.ReadLine();
        Console.WriteLine("Enter publishing year:");
        book.year = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Enter the ISBN of the book");
        book.ISBN = Convert.ToDouble(Console.ReadLine());
        book.inStock = "Book is in available";
        
        List<Book> books = JsonRead.ReadBooksFromFile();
        books.Add(book);
        JsonWrite.WriteBooksToFile(books);
        
        //JsonWrite.WriteBookToFile(new List<Book> { book });
        Console.WriteLine("Book has been added successfully");
    }

    public static void DeleteBook(string titleToDelete)
    {
        try
        {
            // Read existing books from the file
            List<Book> books = JsonRead.ReadBooksFromFile();

            // Find and remove the book with the specified title
            Book bookToRemove = books.FirstOrDefault(b => b.title.Equals(titleToDelete, StringComparison.OrdinalIgnoreCase));

            if (bookToRemove != null)
            {
                books.Remove(bookToRemove);

                // Write the updated list back to the file
                JsonWrite.WriteBooksToFile(books);

                Console.WriteLine($"Book with title '{titleToDelete}' has been deleted successfully.");
            }
            else
            {
                Console.WriteLine($"No book found with the title '{titleToDelete}'.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting the book: {ex.Message}");
        }

    }

    public static void ShowBooks()
    {
        List<Book> books = JsonRead.ReadBooksFromFile();
        foreach (var book in books)
        {
            Console.WriteLine($"Title: {book.title}, Author: {book.author}, Year: {book.year}, ISBN: {book.ISBN}, In Stock: {book.inStock}");
        }
    }
   

}