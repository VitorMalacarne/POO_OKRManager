using System.Globalization;
using OKRManager.Enum;
using OkrManager.Interfaces;
using OkrManager.Models;
using OkrManager.Repositories;
using OkrManager.Services;

namespace OKRManager.UserInterface;

public class KeyResultInterface
{
    private readonly KeyResult _selectedKeyResult;
    private readonly IRepository<SubTask> _subTaskRepository = new Repository<SubTask>();
    private readonly IRepository<KeyResult> _keyResultRepository = new Repository<KeyResult>();
    private readonly VerificationService _verificationService = new VerificationService();
    private readonly RetrieveService _retrieveService = new RetrieveService();

    public KeyResultInterface(KeyResult selectedKeyResult)
    {
        _selectedKeyResult = selectedKeyResult;
    }

    public void RunKeyResultScreen()
    {

        int choice;
        do
        {
            Console.Clear();
            Console.WriteLine($"Título do Resultado Chave: {_selectedKeyResult.Title}");
            Console.WriteLine($"Detalhes do Resultado Chave: {_selectedKeyResult.Description}");
            Console.WriteLine($"Data de Início: {_selectedKeyResult.StartDate}");
            Console.WriteLine($"Data de Término: {_selectedKeyResult.EndDate}");
            Console.WriteLine($"Status: {_selectedKeyResult.Status}");

            DisplaySubTasks();
            
            Console.WriteLine("\nEscolha uma opção:");
            Console.WriteLine("1. Adicionar SubTask");
            Console.WriteLine("2. Selecionar SubTask");
            Console.WriteLine("3. Atualizar Resultado Chave");
            Console.WriteLine("4. Excluir SubTask");
            Console.WriteLine("0. Voltar para a Tela Anterior");
            Console.Write("Digite sua opção aqui:");

            string choiceStr = Console.ReadLine();
            choice = _verificationService.VerifyIsNumber(choiceStr);

            switch (choice)
            {
                case 1:
                    AddSubTask();
                    break;
                case 2:
                    SelectSubTask();
                    break;
                case 3:
                    UpdateKeyResult();
                    break;
                case 4:
                    DeleteSubTask();
                    break;
                case 0:
                    Console.WriteLine("Voltando para a Tela Anterior...");
                    Thread.Sleep(750);
                    break;
                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }

        } while (choice != 0);
    }

    private void DisplaySubTasks()
    {
        var subTasks = _retrieveService.GetAllSubTasksForKeyResult(_selectedKeyResult.KeyResultId);
        var concludedSubTasks = _retrieveService.GetAllConcludedSubTasksForKeyResult(_selectedKeyResult.KeyResultId);

        if (subTasks.Any() || concludedSubTasks.Any())
        {
            Console.WriteLine("\nSubtasks Associadas a Este Resultado Chave:");
            Console.WriteLine("{0, -5} {1, -30} {2, -15} {3, -15} {4, -10} {5, -10}", "ID", "Título", "Status", "Data Início", "Data Fim", "Prioridade");

            foreach (var subTask in subTasks)
            {
                Console.WriteLine("{0, -5} {1, -30} {2, -15} {3, -15} {4, -10} {5, -10}",
                    subTask.SubTaskId,
                    subTask.Title,
                    "Em Andamento",
                    subTask.StartDate.ToShortDateString(),
                    subTask.EndDate.ToShortDateString(),
                    subTask.Priority);
            }

            if (concludedSubTasks.Any())
            {
                Console.WriteLine("\nSubtasks Concluídas:");
                foreach (var concludedSubTask in concludedSubTasks)
                {
                    Console.WriteLine("{0, -5} {1, -30} {2, -15} {3, -15} {4, -10} {5, -10}",
                        concludedSubTask.SubTaskId,
                        concludedSubTask.Title,
                        "Concluída",
                        concludedSubTask.StartDate.ToShortDateString(),
                        concludedSubTask.EndDate.ToShortDateString(),
                        concludedSubTask.Priority);
                }
            }
        }
        else
        {
            Console.WriteLine("\nNão há Subtasks associadas a este resultado chave.");
        }
    }




    private void AddSubTask()
    {
        Console.WriteLine("Digite o título da nova SubTask:");
        string title = _verificationService.VerifyIsNotNull(Console.ReadLine());

        Console.WriteLine("Digite a descrição da nova SubTask:");
        string description = _verificationService.VerifyIsNotNull(Console.ReadLine());

        Console.WriteLine("Digite a prioridade da nova SubTask (Urgente, Importante, Normal):");
        PriorityLevel priority;
        while (!System.Enum.TryParse(Console.ReadLine(), true, out priority))
        {
            Console.WriteLine("Prioridade inválida. Tente novamente (Urgente, Importante, Normal).");
        }

        var newSubTask = new SubTask(title, description, DateTime.Now, DateTime.Now.AddDays(30), false, priority, _selectedKeyResult);

        _subTaskRepository.Update(newSubTask);

        Console.WriteLine("SubTask adicionada com sucesso!");
        Thread.Sleep(1000);
    }

    private void SelectSubTask()
    {
        Console.Clear();
        Console.WriteLine($"Selecione uma SubTask associada ao Resultado Chave '{_selectedKeyResult.Title}':");

        var subTasks = _retrieveService.GetAllSubTasksForKeyResult(_selectedKeyResult.KeyResultId);
        DisplaySubTasks();

        Console.Write("Digite o número da SubTask desejada:");
        int selectedSubTaskId;
        while (!int.TryParse(Console.ReadLine(), out selectedSubTaskId) || !subTasks.Any(st => st.SubTaskId == selectedSubTaskId))
        {
            Console.WriteLine("Opção inválida. Tente novamente.");
        }

        
        Console.WriteLine($"Você selecionou a SubTask com Id {selectedSubTaskId}.");
        Thread.Sleep(1000);
    }

    private void UpdateKeyResult()
    {
        Console.Clear();
        Console.WriteLine("Atualizando Resultado Chave:\n");

        Console.WriteLine($"Título Atual: {_selectedKeyResult.Title}");
        Console.WriteLine($"Descrição Atual: {_selectedKeyResult.Description}");
        Console.WriteLine($"Data de Início Atual: {_selectedKeyResult.StartDate.ToShortDateString()}");
        Console.WriteLine($"Data de Término Atual: {_selectedKeyResult.EndDate.ToShortDateString()}");
        Console.WriteLine($"Status Atual: {_selectedKeyResult.Status}");

        Console.WriteLine("\nDigite os novos dados:");

        Console.Write("Novo Título: ");
        string newTitle = Console.ReadLine();

        Console.Write("Nova Descrição: ");
        string newDescription = Console.ReadLine();

        Console.Write("Nova Data de Início (formato dd/MM/yyyy): ");
        if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime newStartDate))
        {
            _selectedKeyResult.StartDate = newStartDate;
        }

        Console.Write("Nova Data de Término (formato dd/MM/yyyy): ");
        if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime newEndDate))
        {
            _selectedKeyResult.EndDate = newEndDate;
        }

        // Aqui você pode adicionar mais campos conforme necessário.

        // Atualizar os dados no banco de dados (usando o seu método de atualização específico)
        _keyResultRepository.Update(_selectedKeyResult);

        Console.WriteLine("Resultado Chave atualizado com sucesso!");
        Thread.Sleep(2000);
    }


    private void DeleteSubTask()
    {
        Console.Clear();
        Console.WriteLine($"Selecione uma SubTask associada ao Resultado Chave '{_selectedKeyResult.Title}' para excluir:");

        var subTasks = _retrieveService.GetAllSubTasksForKeyResult(_selectedKeyResult.KeyResultId);
        DisplaySubTasks();

        Console.Write("Digite o número da SubTask desejada para excluir:");
        int selectedSubTaskId;
        while (!int.TryParse(Console.ReadLine(), out selectedSubTaskId) || !subTasks.Any(st => st.SubTaskId == selectedSubTaskId))
        {
            Console.WriteLine("Opção inválida. Tente novamente.");
        }

        _subTaskRepository.Delete(selectedSubTaskId);
        Console.WriteLine("SubTask excluída com sucesso!");
        Thread.Sleep(2000);
    }
}
