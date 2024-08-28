using BookLibrary.BE;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.DAL;

public class UserRepository
{
    private LibraryContext context;
    public UserRepository()
    { 
        var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
        context = new LibraryContext(optionsBuilder.Options); 
    }
    
    public void AddUser(User user)
    {
        context.Users.Add(user);
        context.SaveChanges();
    }
    public List<User> GetUsers()
    {
       return context.Users.ToList();
    }
    
    
}