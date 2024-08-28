using System.Collections;
using System.Runtime.CompilerServices;
using System.Text.Json;
using BookLibrary.BE;
using BookLibrary.DAL;
using BookLibrary.Util;

namespace BookLibrary;

class Program
{
    
    static void Main(string[] args)
    {
        
        /*
        {
            
            Console.WriteLine("enter username");
            string username = Console.ReadLine();
            Console.WriteLine("enter password");
            string password = Console.ReadLine();
        }*/
        Start();
    }
    
    private static void Start()
    {
        UserAccount();
        PickOptions();
    }

    private static void UserAccount()
    {
        Console.WriteLine("Welcome to the Book Library! \n Please choose one of the following options:" +
                          " \n 1. Log in \n Sign up");
        int choice = Convert.ToInt32(Console.ReadLine());
        switch (choice)
        {
            case 1:
                PickOptions();
                break;
            case 2:
                SignUp();
                break;
                
        }
    }

    private static void LogIn()
    {
       // Users user = new Users();
        UserRepository userRepo = new UserRepository();
        Console.WriteLine("Please enter your username");
        string userName = Console.ReadLine();
        List<Users> userList = userRepo.GetUsers();
        String toCheck = userList.FirstOrDefault(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));

        if (userName == toCheck)
        {
            PickOptions();
        }
        else
        {
            Console.WriteLine("Please enter a correct username");
        }
        
    }
    private static void PickOptions()
    {
        
        
        Console.WriteLine("Please choose one of the following options:" +
                          "\n 1. Add book \n 2. Delete book \n 3. Update book " +
                          "\n 4. Loan book \n 5. Show all books \n 6. show books that have been loaned out");
        PickChoice();
    }

    private static void SignUp()
    {
        Users user = new Users();
        UserRepository userRepo = new UserRepository();
        
        Console.Write("Enter username: ");
        user.UserName = Console.ReadLine();
        Console.Write("Enter phonenumber: ");
        user.PhoneNumber = Convert.ToInt32(Console.ReadLine());
        userRepo.AddUser(user);
    }

    private static void PickChoice()
    {
        int choice = Convert.ToInt32(Console.ReadLine());
        switch (choice)
        {
            case 1:
                AddBook();
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
            case 5: 
                ShowBooks();
                Start();
                break;
            case 6:
                AllLoanedBooks();
                Start();
                break;
            default: 
                Console.WriteLine("Pick a number between 1 and 4");
                break;
        }
    }

    private static void AddBook()
    {
        Book book = new Book();
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

    public static void AllLoanedBooks()
    {
        string loanedOut = "Book has been loaned out";
        List<Book> books = JsonRead.ReadBooksFromFile();
        Book loanedOutBooks = books.FirstOrDefault(b=>b.inStock.Equals(loanedOut, StringComparison.OrdinalIgnoreCase));
        List<Book> booksToLoan = new List<Book>();
        if (loanedOutBooks != null)
        {
            booksToLoan.Add(loanedOutBooks);
        }
        Console.WriteLine(booksToLoan);
    }
   

}