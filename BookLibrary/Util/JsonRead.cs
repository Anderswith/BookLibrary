using System.Text.Json;
using BookLibrary.BE;

namespace BookLibrary.Util;

public class JsonRead
{
    private static string filePath = @"C:\Users\Anders\Documents\GitHub\BookLibrary\Library.json";
    
    public static List<Book> ReadBooksFromFile()
    {
        try
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                // ?? returner er et argument med mindre den er null så returner den et andet.
                List<Book> books = JsonSerializer.Deserialize<List<Book>>(json) ?? new List<Book>();
                return books;
            }
            else
            {
                Console.WriteLine("File not found.");
                return new List<Book>(); 
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading from file: {ex.Message}");
            return new List<Book>(); 
        }
    }
    
}