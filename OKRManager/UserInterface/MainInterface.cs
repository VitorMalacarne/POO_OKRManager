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
        Thread.Sleep(1000);
        int choice = 10;

        do
        {
            Console.Clear();
            Console.WriteLine($"Olá, {_loggedInUser.Name}!");
            DisplayObjectives();
            Console.WriteLine("\nEscolha uma opção:");
            Console.WriteLine("1. Adicionar Objetivo");
            Console.WriteLine("2. Selecionar Objetivo");
            Console.WriteLine("3. Excluir Objetivo");
            Console.WriteLine("4. Objetivos Concluídos");
            Console.WriteLine("0. Sair da Sessão");
            Console.Write("Digite sua opção aqui:");

            try
            {
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
                        Thread.Sleep(1000);
                        break;
                    default:
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }

        } while (choice != 0);
    }


    private void DisplayObjectives()
    {
        try
        {
            var objectives = _retrieveService.GetAllObjectivesForUser(_loggedInUser.UserId);

            if (objectives.Any())
            {
                Console.WriteLine("Seus Objetivos:\n");
                Console.WriteLine("{0, -5} {1, -30} {2, -15} {3, -15}", "ID", "Título", "Data Início", "Data Fim");

                foreach (var objective in objectives)
                {
                    Console.WriteLine("{0, -5} {1, -30} {2, -15} {3, -15}",
                        objective.ObjectiveId,
                        objective.Title,
                        objective.StartDate.ToString("dd/MM/yyyy"),
                        objective.EndDate.ToString("dd/MM/yyyy"));
                }
            }
            else
            {
                Console.WriteLine("Você não possui objetivos cadastrados.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao exibir objetivos: {ex.Message}");
        }
    }



    private void AddObjective()
    {
        try
        {
            Console.WriteLine("Digite o título do novo objetivo:");
            string title = _verificationService.VerifyIsNotNull(Console.ReadLine());

            Console.WriteLine("Digite a descrição do novo objetivo:");
            string description = _verificationService.VerifyIsNotNull(Console.ReadLine());

            Console.WriteLine("Digite a data de início do objetivo (formato: dd/MM/yyyy):");
            DateTime startDate;
            while (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate) || startDate < DateTime.Now)
            {
                Console.WriteLine("Data inválida. Certifique-se de inserir uma data válida e futura.");
            }

            Console.WriteLine("Digite a data de término do objetivo (formato: dd/MM/yyyy):");
            DateTime endDate;
            while (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate) || endDate < DateTime.Now)
            {
                Console.WriteLine("Data inválida. Certifique-se de inserir uma data válida e futura.");
            }

            var newObjective = new Objective(title, description, startDate, endDate, false, _loggedInUser);

            _objectiveRepository.Update(newObjective);

            Console.WriteLine("Objetivo adicionado com sucesso!");
            Thread.Sleep(1000);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao adicionar objetivo: {ex.Message}");
        }
    }


    private void SelectObjective()
    {
        Console.Clear();
        Console.WriteLine($"{_loggedInUser.Name}, escolha o objetivo que deseja visualizar.");
        
        var objectives = _retrieveService.GetAllObjectivesForUser(_loggedInUser.UserId);
        DisplayObjectives();
        
        Console.Write("Digite o número do objetivo desejado:");
        int selectedObjectiveId;
        while (!int.TryParse(Console.ReadLine(), out selectedObjectiveId) || !objectives.Any(o => o.ObjectiveId == selectedObjectiveId))
        {
            Console.Write("\nOpção inválida. Tente novamente:");
        }

        var selectedObjective = _objectiveRepository.GetById(selectedObjectiveId);
        var objectInterface = new ObjectiveInterface(selectedObjective);
        objectInterface.RunObjectiveScreen();

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
            Console.Write("\nOpção inválida. Tente novamente:");
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

            Console.WriteLine("Digite o ID do objetivo que deseja marcar como não concluído (ou '0' para voltar):");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int selectedObjectiveId) && selectedObjectiveId != 0)
            {
                MarkObjectiveAsNotConcluded(selectedObjectiveId);
            }
        }
        else
        {
            Console.WriteLine("Você não possui objetivos concluídos.");
        }

        Console.WriteLine("Pressione qualquer tecla para voltar ao menu principal...");
        Console.ReadKey();
    }

    private void MarkObjectiveAsNotConcluded(int objectiveId)
    {
        try
        {
            // Suponha que você tenha um serviço ou repositório adequado para manipular objetivos
            var objective = _objectiveRepository.GetById(objectiveId);

            if (objective != null)
            {
                // Atualize o status do objetivo para não concluído
                objective.Status = false;
                _objectiveRepository.Update(objective);

                Console.WriteLine($"Objetivo com ID {objectiveId} marcado como não concluído com sucesso!");
            }
            else
            {
                Console.WriteLine($"Objetivo com ID {objectiveId} não encontrado.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu um erro ao marcar o objetivo como não concluído: {ex.Message}");
        }

        Thread.Sleep(1000);
    }



}