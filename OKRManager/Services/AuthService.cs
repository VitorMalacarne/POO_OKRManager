using OKRManager.Data;
using OkrManager.Interfaces;
using OkrManager.Models;
using OkrManager.Repositories;

namespace OkrManager.Services;

public class AuthService
{
    private readonly ApplicationDbContext _dbContext = new();
    private readonly IRepository<User> _userRepository = new Repository<User>();
    private readonly VerificationService _verificationService = new VerificationService();

    public int? AuthenticateUser(string username)
    {
        try
        {
            var user = _dbContext.User.First(u => u.Name == username);

            Console.WriteLine($"Usuário '{username}' encontrado. Por favor, digite sua senha:");
            string enteredPassword =  _verificationService.VerifyIsNotNull(Console.ReadLine());

            if (enteredPassword == user.Password)
            {
                return user.UserId;
            }
            else
            {
                Console.WriteLine("Senha incorreta. Falha na autenticação.");
                return null;
            }
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine($"Usuário '{username}' não encontrado. Deseja fazer o cadastro? (S/N)");

            if (Console.ReadLine()?.ToUpper() == "S")
            {
                // Tentar fazer o cadastro do usuário
                try
                {
                    Console.WriteLine("Digite uma senha para o novo usuário:");
                    string password = _verificationService.VerifyIsNotNull(Console.ReadLine());

                    RegisterUser(username);
                    return 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ocorreu um erro durante o cadastro: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Login cancelado. Até logo!");
            }

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu um erro durante a autenticação: {ex.Message}");
            return null;
        }
    }


    public bool RegisterUser(string name)
    {
        try
        {
            // Verificar se o usuário já existe
            var existingUser = _dbContext.User.FirstOrDefault(u => u.Name == name);

            if (existingUser != null)
            {
                Console.WriteLine("Usuário já existe. Deseja mudar de nome para continuar o cadastro? (S/N)");
                string changeNameOption = _verificationService.VerifyIsNotNull(Console.ReadLine());

                if (changeNameOption.ToUpper() == "S")
                {
                    Console.WriteLine("Digite um novo nome de usuário:");
                    name = _verificationService.VerifyIsNotNull(Console.ReadLine());

                    // Verificar novamente se o novo nome de usuário já existe
                    existingUser = _dbContext.User.FirstOrDefault(u => u.Name == name);

                    if (existingUser != null)
                    {
                        Console.WriteLine("Usuário já existe. Tente novamente com um nome diferente.");
                        return false; 
                    }
                }
                else
                {
                    Console.WriteLine("Cadastro cancelado. Tente novamente com um nome de usuário diferente.");
                    return false; 
                }
            }
            Console.WriteLine("Digite sua nova senha:");
            string password = _verificationService.VerifyIsNotNull(Console.ReadLine());
            
            var newUser = new User(name, password);

            _userRepository.Create(newUser);
            
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro inesperado: {ex.Message}");
            return false; 
        }
    }

}