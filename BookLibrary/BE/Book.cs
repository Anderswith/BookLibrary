using System.ComponentModel.DataAnnotations;

namespace BookLibrary.BE;

public class Book
{
    
    public string title { get; set; }
    public string author { get; set; }
    public int yearOfPublication { get; set; }
    [Key]
    public string ISBN { get; set; }
    public string inStock { get; set; }
    
    public Guid? UserID { get; set; } 
    public User LoanedBy { get; set; }

}