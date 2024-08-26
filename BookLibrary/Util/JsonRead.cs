using System.Text.Json;
using BookLibrary.BE;

namespace BookLibrary.Util;

public class JsonRead
{
    private static string filePath = @"C:\Users\ko2an\Desktop\Library.json";
    
    public static List<Book> ReadBooksFromFile()
    {
        try
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                List<Book> books = JsonSerializer.Deserialize<List<Book>>(json);
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