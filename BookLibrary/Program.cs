using System.Collections;
using System.Runtime.CompilerServices;
using System.Text.Json;
using BookLibrary.BE;
using BookLibrary.DAL;
using BookLibrary.Util;

namespace BookLibrary;

class Program
{
    private static UserRepository userRepo = new UserRepository();
    private static BookRepository bookRepo = new BookRepository();
    //private static JsonRead jsonRead = new JsonRead();
    //private static JsonWrite jsonWrite = new JsonWrite();
    private static Guid? LoggedInUserId;
    
    static void Main(string[] args)
    {
        UserAccount();
    }

    private static void UserAccount()
    {
        Console.WriteLine("Welcome to the Book Library! \n Please choose one of the following options:" +
                          " \n 1. Log in \n 2. Sign up");
        int choice = Convert.ToInt32(Console.ReadLine());
        switch (choice)
        {
            case 1:
                LogIn();
                break;
            case 2:
                SignUp();
                break;
        }
    }

    private static void LogIn()
    {
        Console.WriteLine("Please enter your username");
        string userName = Console.ReadLine();
        var user = userRepo.GetUsers().FirstOrDefault(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));

        if (user != null)
        {
            LoggedInUserId = user.UserID;
            Console.WriteLine("login was successfull");
            PickOptions();
        }
        else
        {
            Console.WriteLine("Please enter a correct username");
        }
    }

    private static void SignUp()
    {
        User user = new User();
        Console.Write("Enter username: ");
        string toTest = Console.ReadLine();
        bool alreadyExists = userRepo.GetUsers().Any(u => u.UserName.Equals(toTest, StringComparison.OrdinalIgnoreCase));
            
        if (!alreadyExists)
        {
                
                user.UserName = toTest;
                Console.Write("Enter phonenumber: ");
                user.PhoneNumber = Convert.ToInt32(Console.ReadLine());
                userRepo.AddUser(user);
                Console.Write($"{user.UserName} signed up successfully");
        }
        else
        {
                Console.WriteLine("That username is already taken");
        } 
        UserAccount();
    }

    private static void PickOptions()
    {
        Console.WriteLine("Please choose one of the following options:" +
                          "\n 1. Add book \n 2. Delete book \n 3. Update book " +
                          "\n 4. Loan book \n 5. Show all books \n 6. Show books that have been loaned out" +
                          "\n 7. Show books that you have loaned \n 8. Return books \n 9. Show all users" );
        int choice = Convert.ToInt32(Console.ReadLine());
        switch (choice)
        {
            case 1:
                AddBook();
                PickOptions();
                break;
            case 2:
                DeleteBook();
                PickOptions();
                break;
            case 3:
                UpdateBook();
                PickOptions();
                break;
            case 4:
                LoanBook();
                PickOptions();
                break;
            case 5: 
                ShowBooks();
                PickOptions();
                break;
            case 6:
                AllLoanedBooks();
                PickOptions();
                break;
            case 7:
                ShowLoanedBooksByUser();
                PickOptions();
                break;
            case 8:
                ReturnBook();
                PickOptions();
                break;
            case 9:
                ShowAllUsers();
                PickOptions();
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
        book.yearOfPublication = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Enter the ISBN of the book");
        book.ISBN = Console.ReadLine();
        book.inStock = "Book is available";
        book.UserID = null;
        
       bookRepo.AddBook(book);
    }
    
    public static void DeleteBook()
    {
        Console.WriteLine("Enter the title of the book you want to delete:");
        ShowBooks();
        string bookToDelete = Console.ReadLine();
        
        Book book = bookRepo.GetBooks().FirstOrDefault(b=> b.title.Equals(bookToDelete, StringComparison.OrdinalIgnoreCase));
        if (book != null)
        {
            bookRepo.DeleteBook(book);
            Console.WriteLine($"{bookToDelete}was deleted successfully");
        }
        else
        {
            Console.WriteLine("Book not found");
        }
    }

    public static void ShowBooks()
    {
        List<Book> books = bookRepo.GetBooks();
        foreach (var book in books)
        {
            Console.WriteLine($"Title: {book.title}, Author: {book.author}, Year: {book.yearOfPublication}, ISBN: {book.ISBN}, In Stock: {book.inStock}");
        }
    }

    public static void ShowAllUsers()
    {
        List<User> users = userRepo.GetUsers();
        foreach (var user in users)
        {
            Console.WriteLine($"Username:{user.UserName} Phone number:{user.PhoneNumber}");
        }
    }

    public static void UpdateBook()
    {
        Console.WriteLine("Here is a list of all the books in the library:");
        ShowBooks();
        Console.WriteLine("Enter the title of the book you want to update:");
        string book = Console.ReadLine();
        Book toUpdate = bookRepo.GetBooks().FirstOrDefault(b => b.title.Equals(book, StringComparison.OrdinalIgnoreCase));
        
        if(toUpdate != null)
        {
            Console.WriteLine("Choose one of the following options to edit: title, author, year, or ISBN:");
            string pickedBookToEdit = Console.ReadLine();
            
            
            if (pickedBookToEdit.Contains("title", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Enter new title for the book:");
                toUpdate.title = Console.ReadLine();
            }

            if (pickedBookToEdit.Contains("author", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Enter new author for the book:");
                toUpdate.author = Console.ReadLine();
            }

            if (pickedBookToEdit.Contains("year", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Enter new publishing year for the book:");
                toUpdate.yearOfPublication = Convert.ToInt32(Console.ReadLine());
            }
            
            bookRepo.UpdateBook(toUpdate);
            Console.WriteLine("Book has been updated");
        }
        else
        {
            Console.WriteLine("Book not found");
        }
    }
    public static void LoanBook()
    {
        ShowBooks();
        Console.WriteLine("Enter the name of the book you want to loan:");
        string toLoan = Console.ReadLine().ToLower();
        var book = bookRepo.GetBookByTitle(toLoan);
        if (book != null)
        {
            book.UserID = LoggedInUserId;
            book.inStock = "Book unavailable";
            bookRepo.UpdateBook(book);
            Console.WriteLine($"{book.ISBN} was loaned successfully");
        }
        else
        {
            Console.WriteLine($"No book found with the title '{toLoan}'.");
        }

    }

    public static void AllLoanedBooks()
    {
        List<Book> loanedOut = bookRepo.GetBooks().Where(b => b.inStock == "Book is available").ToList();
        foreach (var book in loanedOut)
        {
            Console.WriteLine($"Title: {book.title}, Author: {book.author}, Year: {book.yearOfPublication}, ISBN: {book.ISBN}");
        }
    }

    public static void ShowLoanedBooksByUser()
    {
        List<Book> loanedBooks = bookRepo.GetBooksLoanedByUser(LoggedInUserId.Value);
        Console.WriteLine("books loaned by you");
        if (loanedBooks != null)
        {
            foreach (var book in loanedBooks)
            {
                Console.WriteLine($"Title: {book.title}, Author: {book.author}, Year: {book.yearOfPublication}, ISBN: {book.ISBN}");
            }
        }
        else
        {
            Console.WriteLine("You have not loaned any books yet");
        }
    }

    public static void ReturnBook()
    {
        List<Book> loanedBooks = bookRepo.GetBooksLoanedByUser(LoggedInUserId.Value);
        ShowLoanedBooksByUser();
        Console.WriteLine("Enter the title of the book you want to return:");
        
        string toReturn = Console.ReadLine();
        var bookToReturn = loanedBooks.FirstOrDefault(b => b.title.Equals(toReturn, StringComparison.OrdinalIgnoreCase));
        if (bookToReturn != null)
        {
            Console.WriteLine($"{bookToReturn} has been returned");
            bookRepo.ReturnBook(bookToReturn.ISBN);
        }
        else
        {
            Console.WriteLine("Book not found");
        }
    }
}