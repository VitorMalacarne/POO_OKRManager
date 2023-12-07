
using OkrManager.Models;
using OkrManager.Repositories;
using OkrManager.Services;
using Pomelo.EntityFrameworkCore.MySql.Query.ExpressionTranslators.Internal;

namespace OKRManager.UserInterface;

public class AuthenticationInterface
{
    private readonly AuthService _authService;
    private readonly VerificationService _verificationService;
    private readonly Repository<User> _userRepository;

    public AuthenticationInterface(AuthService authService)
    {
        _authService = authService;
        _verificationService = new VerificationService();
        _userRepository = new Repository<User>();
    }

    public void RunAuthentication()
    {
        int choice;
        do
        {
            Console.Clear();
            Console.WriteLine("Bem-vindo ao OKRManager!");
            Console.WriteLine("Escolha uma opção:");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Cadastrar-se");
            Console.WriteLine("0. Sair");
            Console.Write("Digite sua opção aqui:");
            string choiceStr = Console.ReadLine();
            choice = _verificationService.VerifyIsNumber(choiceStr);

            switch (choice)
            {
                case 1:
                    Login();
                    break;
                case 2:
                    Register();
                    break;
                case 0:
                    Console.WriteLine("Saindo do sistema. Até logo!");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }

        } while (choice != 0);
    }
    private void Login()
    {
        Console.Clear();
        Console.WriteLine("Digite seu nome de usuário:");
        string username = _verificationService.VerifyIsNotNull(Console.ReadLine());

        Console.WriteLine("Digite sua senha:");
        string password = _verificationService.VerifyIsNotNull(Console.ReadLine());

        int? userId = _authService.AuthenticateUser(username, password);

        if (userId != null)
        {
            Console.WriteLine($"Login bem-sucedido!");
            Thread.Sleep(1000);
            var loggedInUser = _userRepository.GetById(userId.Value);
            var mainInterface = new MainInterface(loggedInUser);
            mainInterface.RunMainScreen();
        }
        else
        {
            Console.WriteLine("Falha na autenticação. Verifique suas credenciais e tente novamente.");
        }
    }

    private void Register()
    {
        Console.WriteLine("Digite seu nome de usuário:");
        string username = _verificationService.VerifyIsNotNull(Console.ReadLine());

        Console.WriteLine("Digite sua senha:");
        string password = _verificationService.VerifyIsNotNull(Console.ReadLine());

        bool registrationSuccess = _authService.RegisterUser(username, password);

        if (registrationSuccess)
        {
            Console.WriteLine("Cadastro bem-sucedido! Faça o login para acessar o sistema.");
        }
        else
        {
            Console.WriteLine("Falha no cadastro. Tente novamente com um nome de usuário diferente.");
        }
    }
}