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
        
        //JsonWrite.WriteBookToFile(new List<Book> { book });
        Console.WriteLine("Book has been added to the library");
    }

    public static void DeleteBook()
    {
        Console.WriteLine("Enter the title of the book you want to delete:");
        ShowBooks();
        string bookToDelete = Console.ReadLine();
        try
        {
            // Read existing books from the file
            List<Book> books = JsonRead.ReadBooksFromFile();

            // Find and remove the book with the specified title
            Book bookToRemove = books.FirstOrDefault(b => b.title.Equals(bookToDelete, StringComparison.OrdinalIgnoreCase));

            if (bookToRemove != null)
            {
                books.Remove(bookToRemove);

                // Write the updated list back to the file
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
        Console.WriteLine("enter the attribute you want to update:");
        
        

        string pickedBookToEdit = Console.ReadLine();
        /*foreach(Book book in books)
        {
            if(book.title == someValue)
            {
                book.title = newValue;
                break;
            }
        }*/
        if (pickedBookToEdit.Contains("title"))
        {
            Console.WriteLine("Enter new title for the book:");
            toEdit.title = Console.ReadLine();
            //CreateBook(toEdit);
        }

        if (pickedBookToEdit.Contains("author"))
        {
         Console.WriteLine("Enter new author for the book:");
         toEdit.author = Console.ReadLine();
         CreateBook(toEdit);
        }

        if (pickedBookToEdit.Contains("year"))
        {
            Console.WriteLine("Enter new publishing year for the book:");
            toEdit.year = Convert.ToInt32(Console.ReadLine());
            CreateBook(toEdit);
        }

        if (pickedBookToEdit.Contains("isbn"))
        {
            Console.WriteLine("Enter new ISBN for the book:");
            toEdit.ISBN = Convert.ToDouble(Console.ReadLine());
            CreateBook(toEdit);
        }
    }
    public static void LoanBook()
    {
        Console.WriteLine("Enter the name of the book you want to loan:");
        ShowBooks();
        string toLoan = Console.ReadLine();
        List<Book> books = JsonRead.ReadBooksFromFile();
        Book loanBook = books.FirstOrDefault(b => b.title.Equals(toLoan, StringComparison.OrdinalIgnoreCase));
        
        loanBook.inStock = "Book has been loaned out";
        CreateBook(loanBook);

    }
   

}