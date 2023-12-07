using System.Globalization;
using OkrManager.Interfaces;
using OkrManager.Models;
using OkrManager.Repositories;
using OkrManager.Services;

namespace OKRManager.UserInterface;

public class MainInterface
{
    private readonly User _loggedInUser;
    private readonly IRepository<Objective> _objectiveRepository= new Repository<Objective>();
    private readonly VerificationService _verificationService = new VerificationService();
    private readonly RetrieveService _retrieveService = new RetrieveService();

    public MainInterface(User loggedInUser)
    {
        _loggedInUser = loggedInUser;
    }

    public void RunMainScreen()
    {
        Console.Clear();
        Console.WriteLine($"Olá, {_loggedInUser.Name}! Bem-vindo ao OKRManager.");
        Console.WriteLine("Entrando no sistema...");
        Thread.Sleep(2000);
        int choice;
        do
        {
            Console.WriteLine($"Olá, {_loggedInUser.Name}!");
            DisplayObjectives();
            Console.WriteLine("Escolha uma opção:");
            Console.WriteLine("1. Adicionar Objetivo");
            Console.WriteLine("2. Selecionar Objetivo");
            Console.WriteLine("3. Excluir Objetivo");
            Console.WriteLine("4. Objetivos Concluídos");
            Console.WriteLine("0. Sair da Sessão");
            Console.Write("Digite sua opção aqui:");

            string choiceStr = Console.ReadLine();
            choice = _verificationService.VerifyIsNumber(choiceStr);

            switch (choice)
            {
                case 1:
                    AddObjective();
                    break;
                case 2:
                    SelectObjective();
                    break;
                case 3:
                    DeleteObjective();
                    break;
                case 4:
                    DisplayConcludedObjectives();
                    break;
                case 0:
                    Console.WriteLine("Saindo da sessão. Até logo!");
                    break;
                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }

        } while (choice != 0);
    }

    private void DisplayObjectives()
    {
        var objectives = _retrieveService.GetAllObjectivesForUser(_loggedInUser.UserId);

        if (objectives.Any())
        {
            Console.WriteLine("Seus Objetivos:\n");
            foreach (var objective in objectives)
            {
                Console.WriteLine($"{objective.ObjectiveId}. {objective.Title}");
            }
        }
        else
        {
            Console.WriteLine("Você não possui objetivos cadastrados.");
        }
    }

    private void AddObjective()
    {
        Console.WriteLine("Digite o título do novo objetivo:");
        string title = _verificationService.VerifyIsNotNull(Console.ReadLine());

        Console.WriteLine("Digite a descrição do novo objetivo:");
        string description = _verificationService.VerifyIsNotNull(Console.ReadLine());
        
        Console.WriteLine("Digite a data de início do objetivo (formato: dd/MM/yyyy):");
        DateTime startDate;
        while (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
        {
            Console.WriteLine("Formato de data inválido. Tente novamente.");
        }

        Console.WriteLine("Digite a data de término do objetivo (formato: dd/MM/yyyy):");
        DateTime endDate;
        while (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
        {
            Console.WriteLine("Formato de data inválido. Tente novamente.");
        }

        var newObjective = new Objective(title, description, DateTime.Now, DateTime.Now.AddDays(30), false, _loggedInUser);

        _objectiveRepository.Update(newObjective);

        Console.WriteLine("Objetivo adicionado com sucesso!");
    }

    private void SelectObjective()
    {
        Console.Clear();
        Console.WriteLine($"{_loggedInUser.Name}, escolha o objetivo que deseja visualizar.");
        
        var objectives = _retrieveService.GetAllObjectivesForUser(_loggedInUser.UserId);
        DisplayObjectives();

        // Solicite ao usuário que escolha um objetivo
        Console.Write("Digite o número do objetivo desejado:");
        int selectedObjectiveId;
        while (!int.TryParse(Console.ReadLine(), out selectedObjectiveId) || !objectives.Any(o => o.ObjectiveId == selectedObjectiveId))
        {
            Console.WriteLine("Opção inválida. Tente novamente.");
        }

        var selectedObjective = _objectiveRepository.GetById(selectedObjectiveId);
        var objectInterface = new ObjectiveInterface(selectedObjective);
        objectInterface.RunObjectiveScreen();
        Console.WriteLine($"Você selecionou o objetivo com Id {selectedObjectiveId}.");

    }

    private void DeleteObjective()
    {
        Console.Clear();
        Console.WriteLine($"{_loggedInUser.Name}, escolha o objetivo que deseja visualizar.");
        
        var objectives = _retrieveService.GetAllObjectivesForUser(_loggedInUser.UserId);
        DisplayObjectives();
        
        Console.Write("Digite o número do objetivo desejado:");
        int selectedObjectiveId;
        while (!int.TryParse(Console.ReadLine(), out selectedObjectiveId) || !objectives.Any(o => o.ObjectiveId == selectedObjectiveId))
        {
            Console.WriteLine("Opção inválida. Tente novamente.");
        }
        _objectiveRepository.Delete(selectedObjectiveId);
        Console.WriteLine("Objetivo deletado!");
        Thread.Sleep(2000);
    }
    public void DisplayConcludedObjectives()
    {
        Console.Clear();
        Console.WriteLine($"Olá, {_loggedInUser.Name}! Aqui estão seus objetivos concluídos:");

        var concludedObjectives = _retrieveService.GetAllConcludedObjectivesForUser(_loggedInUser.UserId);

        if (concludedObjectives.Any())
        {
            foreach (var objective in concludedObjectives)
            {
                Console.WriteLine($"ID: {objective.ObjectiveId}");
                Console.WriteLine($"Título: {objective.Title}");
                Console.WriteLine($"Descrição: {objective.Description}");
                Console.WriteLine($"Data de Início: {objective.StartDate}");
                Console.WriteLine($"Data de Término: {objective.EndDate}");
                Console.WriteLine("--------");
            }
        }
        else
        {
            Console.WriteLine("Você não possui objetivos concluídos.");
        }

        Console.WriteLine("Pressione qualquer tecla para voltar ao menu principal...");
        Console.ReadKey();
    }

}