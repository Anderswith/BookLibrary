using System.ComponentModel.DataAnnotations;

namespace BookLibrary.BE;

public class Book
{
    
    public string title { get; set; }
    public string author { get; set; }
    public int year { get; set; }
    [Key]
    public double ISBN { get; set; }
    public string inStock { get; set; }

}