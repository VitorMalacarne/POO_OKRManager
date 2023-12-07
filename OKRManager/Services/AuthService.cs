using OKRManager.Data;
using OkrManager.Interfaces;
using OkrManager.Models;
using OkrManager.Repositories;

namespace OkrManager.Services;

public class AuthService
{
    private readonly ApplicationDbContext _dbContext = new();
    private readonly IRepository<User> _userRepository = new Repository<User>();

    public int? AuthenticateUser(string username, string password)
    {
        
        var user = _dbContext.User.FirstOrDefault(u => u.Name == username);
        
        if (user != null && user.Password == password)
        {
            return user.UserId;
        }

        return null;
    }
    public bool RegisterUser(string name, string password)
    {
        // Verificar se o usuário já existe
        var existingUser = _dbContext.User.FirstOrDefault(u => u.Name == name);

        if (existingUser != null)
        {
            Console.WriteLine("Usuário já existe. Tente outro nome.");
            return false; 
        }
        
        var newUser = new User(name, password);
        
        _userRepository.Create(newUser);

        Console.WriteLine("Usuário registrado com sucesso!");
        
        return true;
    }
}