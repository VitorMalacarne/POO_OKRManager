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
    private readonly VerificationService _verificationService = new VerificationService();
    private readonly RetrieveService _retrieveService = new RetrieveService();

    public KeyResultInterface(KeyResult selectedKeyResult)
    {
        _selectedKeyResult = selectedKeyResult;
    }

    public void RunKeyResultScreen()
    {
        Console.Clear();
        Console.WriteLine($"Detalhes do Resultado Chave: {_selectedKeyResult.Title}");
        Console.WriteLine($"Data de Início: {_selectedKeyResult.StartDate}");
        Console.WriteLine($"Data de Término: {_selectedKeyResult.EndDate}");
        Console.WriteLine($"Status: {_selectedKeyResult.Status}");

        DisplaySubTasks();

        int choice;
        do
        {
            Console.WriteLine("Escolha uma opção:");
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
                    Thread.Sleep(2000);
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

        if (subTasks.Any())
        {
            Console.WriteLine("\nSubTasks Associadas a Este Resultado Chave:");
            foreach (var subTask in subTasks)
            {
                Console.WriteLine($"{subTask.SubTaskId}. {subTask.Title} - Prioridade: {subTask.Priority}");
            }
        }
        else
        {
            Console.WriteLine("\nNão há SubTasks associadas a este Resultado Chave.");
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
        Thread.Sleep(2000);
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
        Thread.Sleep(2000);
    }

    private void UpdateKeyResult()
    {
        // Adicione a lógica para permitir ao usuário atualizar as informações do Resultado Chave
        Console.WriteLine("Lógica para atualizar o Resultado Chave...");
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
