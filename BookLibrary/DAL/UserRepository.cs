using BookLibrary.BE;

namespace BookLibrary.DAL;

public class UserRepository
{
    public UserRepository()
    {
        var OptionsBuilder = new DbContextOptions<LibraryContext>();
        
    }
    public void AddUser(Users user)
    {
        using (LibraryContext context = new LibraryContext())
        {
            context.Users.Add(user);
            context.SaveChanges();
        }
    }

    
    public List<Users> GetUsers()
    {
        using (LibraryContext context = new LibraryContext())
        {
            context.Users.Find();
        }
    }
    
}