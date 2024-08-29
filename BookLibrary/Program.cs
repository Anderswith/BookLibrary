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
        UserRepository userRepo = new UserRepository();
        
        Console.Write("Enter username: ");
        user.UserName = Console.ReadLine();
        Console.Write("Enter phonenumber: ");
        user.PhoneNumber = Convert.ToInt32(Console.ReadLine());
        userRepo.AddUser(user);
        Console.Write($"{user.UserName} signed up successfully");
        
        UserAccount();
    }

    private static void PickOptions()
    {
        Console.WriteLine("Please choose one of the following options:" +
                          "\n 1. Add book \n 2. Delete book \n 3. Update book " +
                          "\n 4. Loan book \n 5. Show all books \n 6. Show books that have been loaned out" +
                          "\n 7. Show books that you have loaned \n 8. Return books" );
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
                break;
            case 8:
                ReturnBook();
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
        
       // CreateBook(book);
       bookRepo.AddBook(book);
    }

    /*private static void CreateBook(Book book)
    {
        List<Book> books = JsonRead.ReadBooksFromFile();
        books.Add(book);
        JsonWrite.WriteBooksToFile(books);
        Console.WriteLine("Book has been added to the library");
    }
    */
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
        /*try
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
        */
    }

    public static void ShowBooks()
    {
        //List<Book> books = JsonRead.ReadBooksFromFile();
        List<Book> books = bookRepo.GetBooks();
        foreach (var book in books)
        {
            Console.WriteLine($"Title: {book.title}, Author: {book.author}, Year: {book.yearOfPublication}, ISBN: {book.ISBN}, In Stock: {book.inStock}");
        }
    }

    public static void UpdateBook()
    {
        Console.WriteLine("Here is a list of all the books in the library:");
        ShowBooks();
        Console.WriteLine("Enter the title of the book you want to update:");
        string book = Console.ReadLine();
        //List<Book> books = JsonRead.ReadBooksFromFile();
        //Book toEdit = books.FirstOrDefault(b => b.title.Equals(book, StringComparison.OrdinalIgnoreCase));
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
            
            //JsonWrite.WriteBooksToFile(books);
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
        //List<Book> books = JsonRead.ReadBooksFromFile();
        string toLoan = Console.ReadLine().ToLower();
        //Book loanBook = books.FirstOrDefault(b => b.title.Equals(toLoan, StringComparison.OrdinalIgnoreCase));
        //Book loanBook = bookRepo.GetBooks().FirstOrDefault(b => b.title.Equals(toLoan, StringComparison.OrdinalIgnoreCase));
        var book = bookRepo.GetBookByTitle(toLoan);
        if (book != null)
        {
            //bookRepo.LoanBook();
            book.UserID = LoggedInUserId;
            book.inStock = "Book unavailable";
            bookRepo.UpdateBook(book);
            Console.WriteLine($"{book.ISBN} was loaned successfully");
            //JsonWrite.WriteBooksToFile(books); 
        }
        else
        {
            Console.WriteLine($"No book found with the title '{toLoan}'.");
        }

    }

    public static void AllLoanedBooks()
    {
        //string loanedOut = "Book has been loaned out";
        //List<Book> books = JsonRead.ReadBooksFromFile();
        //Book loanedOutBooks = books.FirstOrDefault(b=>b.inStock.Equals(loanedOut, StringComparison.OrdinalIgnoreCase));
        List<Book> loanedOut = bookRepo.GetBooks().Where(b => b.inStock == "Book is available").ToList();
        /*List<Book> booksToLoan = new List<Book>();
        if (loanedOutBooks != null)
        {
            booksToLoan.Add(loanedOutBooks);
        }
        Console.WriteLine(booksToLoan);
        */
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
            Console.WriteLine("Book not found");
        }
        bookRepo.ReturnBook(bookToReturn.ISBN);
        Console.WriteLine($"{bookToReturn} has been returned");
    }

}