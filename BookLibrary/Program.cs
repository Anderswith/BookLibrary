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
                DeleteBook();
                Start();
                break;
            case 3:
                UpdateBook();
                Start();
                break;
            case 4:
                LoanBook();
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
        book.inStock = "Book is available";
        
        CreateBook(book);
    }

    private static void CreateBook(Book book)
    {
        List<Book> books = JsonRead.ReadBooksFromFile();
        books.Add(book);
        JsonWrite.WriteBooksToFile(books);
        Console.WriteLine("Book has been added to the library");
    }

    public static void DeleteBook()
    {
        Console.WriteLine("Enter the title of the book you want to delete:");
        ShowBooks();
        string bookToDelete = Console.ReadLine();
        try
        {
            List<Book> books = JsonRead.ReadBooksFromFile();
            Book bookToRemove = books.FirstOrDefault(b => b.title.Equals(bookToDelete, StringComparison.OrdinalIgnoreCase));

            if (bookToRemove != null)
            {
                books.Remove(bookToRemove);
                JsonWrite.WriteBooksToFile(books);
                Console.WriteLine($"Book with title '{bookToDelete}' has been removed from the library.");
            }
            else
            {
                Console.WriteLine($"No book found with the title '{bookToDelete}'.");
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

    public static void UpdateBook()
    {
        Console.WriteLine("Here is a list of all the books in the library:");
        ShowBooks();
        Console.WriteLine("Enter the title of the book you want to update:");
        string book = Console.ReadLine();
        List<Book> books = JsonRead.ReadBooksFromFile();
        Book toEdit = books.FirstOrDefault(b => b.title.Equals(book, StringComparison.OrdinalIgnoreCase));
        
        
        
        if(toEdit != null)
        {
            Console.WriteLine("Choose one of the following options to edit: title, author, year, or ISBN:");
            string pickedBookToEdit = Console.ReadLine();
            
            
            if (pickedBookToEdit.Contains("title", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Enter new title for the book:");
                toEdit.title = Console.ReadLine();
            }

            if (pickedBookToEdit.Contains("author", StringComparison.OrdinalIgnoreCase))
            {
             Console.WriteLine("Enter new author for the book:");
             toEdit.author = Console.ReadLine();
            }

            if (pickedBookToEdit.Contains("year", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Enter new publishing year for the book:");
                toEdit.year = Convert.ToInt32(Console.ReadLine());
            }

            if (pickedBookToEdit.Contains("isbn", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Enter new ISBN for the book:");
                toEdit.ISBN = Convert.ToDouble(Console.ReadLine());
            }
            JsonWrite.WriteBooksToFile(books);
            Console.WriteLine("Book has been updated");
        }
        else
        {
            Console.WriteLine("Book not found");
        }
    }
    public static void LoanBook()
    {
        
        Console.WriteLine("Enter the name of the book you want to loan:");
        ShowBooks();
        List<Book> books = JsonRead.ReadBooksFromFile();
        string toLoan = Console.ReadLine().ToLower();
        Book loanBook = books.FirstOrDefault(b => b.title.Equals(toLoan, StringComparison.OrdinalIgnoreCase));
        
        if (loanBook != null)
        {
            if (loanBook.inStock == "Book is available")  
            {
                loanBook.inStock = "Book has been loaned out";  
                JsonWrite.WriteBooksToFile(books);  
                Console.WriteLine($"'{loanBook.title}' has been loaned out.");
            }
            else if (loanBook.inStock == "Book has been loaned out")  
            {
                Console.WriteLine("Book is already loaned out.");
            }
        }
        else
        {
            Console.WriteLine($"No book found with the title '{toLoan}'.");
        }

    }
   

}