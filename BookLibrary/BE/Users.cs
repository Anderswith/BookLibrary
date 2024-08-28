using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BookLibrary.BE;

public class Users
{
    public string UserName { get; set; }
    public int PhoneNumber{ get; set; }
    public Guid? UserID { get; set; }
}