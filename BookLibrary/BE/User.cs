using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookLibrary.BE;

public class User
{
    //burde generer en ny guid når der laves en ny bruger - skal testes.
    public User()
    {
        UserID = Guid.NewGuid();
        LoanedBooks = new List<Book>();
    }
    public string UserName { get; set; }
    public int PhoneNumber{ get; set; }
    public Guid? UserID { get; set; }
    public List<Book> LoanedBooks { get; set; }
    
}