using System.Text.Json;
using BookLibrary.BE;

namespace BookLibrary.Util;

public class JsonWrite
{
    private static string filePath = @"C:\Users\Anders\Documents\GitHub\BookLibrary\Library.json";
    public static void WriteBooksToFile(List<Book> books)
    {
        try
        {
            string json = JsonSerializer.Serialize(books, new JsonSerializerOptions()
            {
                WriteIndented = true
            });
            
            File.WriteAllText(filePath, json);
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing to file: {ex.Message}");
        }
    }  
}