using System.Collections;
using System.Text.Json;
using BookLibrary.BE;

namespace BookLibrary;

class Program
{
    
    static void Main(string[] args)
    {
        Book book = new Book();
        Console.WriteLine("Hello, welcome to the Book Library! Please choose one of the following options:" +
                          "\n 1. Add book \n 2. Delete book \n 3. Update book \n 4. Loan book");
        int choice = Convert.ToInt32(Console.ReadLine());
        switch (choice)
        {
            case 1:
                Console.WriteLine("Enter book title:");
                book.title = Console.ReadLine();
                Console.WriteLine("Enter book author:");
                book.author = Console.ReadLine();
                Console.WriteLine("Enter publishing year:");
                book.year = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Enter the ISBN of the book");
                book.ISBN = Convert.ToDouble(Console.ReadLine());
                book.isLoaned = false;
                break;
            case 2:
                Console.WriteLine("Book has been deleted");
                break;
            case 3:
                break;
            case 4:
                break;
        }
        
        
        
        
        string json = JsonSerializer.Serialize(book, new JsonSerializerOptions()
        {
            WriteIndented = true
        });
        File.WriteAllText(@"\Users\ko2an\Desktop\Library.json", json);
    }

    
}