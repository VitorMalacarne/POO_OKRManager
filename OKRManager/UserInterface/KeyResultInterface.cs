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
            Console.WriteLine($"Status: {(_selectedKeyResult.Status ? "Concluído" : "Em andamento")}");
            DisplayKeyResultCompletionPercentage(_selectedKeyResult);


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
        try
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

            Console.WriteLine("Digite a data de início da nova SubTask (formato: dd/MM/yyyy):");
            DateTime startDate;
            while (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate) || startDate < DateTime.Now)
            {
                Console.WriteLine("Data inválida. Certifique-se de inserir uma data válida e futura.");
            }

            Console.WriteLine("Digite a data de término da nova SubTask (formato: dd/MM/yyyy):");
            DateTime endDate;
            while (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate) || endDate < DateTime.Now)
            {
                Console.WriteLine("Data inválida. Certifique-se de inserir uma data válida e futura.");
            }

            var newSubTask = new SubTask(title, description, startDate, endDate, false, priority, _selectedKeyResult);

            _subTaskRepository.Update(newSubTask);

            Console.WriteLine("SubTask adicionada com sucesso!");
            Thread.Sleep(1000);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao adicionar SubTask: {ex.Message}");
        }
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
    Console.WriteLine("Atualizando Resultado Chave");
    Console.WriteLine("---------------------------");

    // Exibir detalhes do resultado chave
    DisplayKeyResultDetails();

    Console.WriteLine("\nOpções de Atualização:");
    Console.WriteLine("1. Atualizar Título");
    Console.WriteLine("2. Atualizar Descrição");
    Console.WriteLine("3. Atualizar Data de Início");
    Console.WriteLine("4. Atualizar Data de Término");
    Console.WriteLine("5. Marcar como Concluído");
    Console.WriteLine("6. Voltar ao Menu Anterior");

    Console.Write("Escolha uma opção: ");
    string choiceStr = Console.ReadLine();

    if (int.TryParse(choiceStr, out int choice))
    {
        try
        {
            switch (choice)
            {
                case 1:
                    UpdateTitle();
                    break;
                case 2:
                    UpdateDescription();
                    break;
                case 3:
                    UpdateStartDate();
                    break;
                case 4:
                    UpdateEndDate();
                    break;
                case 5:
                    MarkAsCompleted();
                    break;
                case 6:
                    // Voltar ao Menu Anterior
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
    }
    else
    {
        Console.WriteLine("Opção inválida. Tente novamente.");
    }
}

private void MarkAsCompleted()
{
    try
    {
        _selectedKeyResult.Status = true;
        _keyResultRepository.Update(_selectedKeyResult);
        Console.WriteLine("Resultado Chave marcado como concluído com sucesso!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao marcar como concluído: {ex.Message}");
    }
}

private void UpdateTitle()
{
    try
    {
        Console.Write("Novo Título: ");
        string newTitle = Console.ReadLine();
        _selectedKeyResult.Title = newTitle;
        _keyResultRepository.Update(_selectedKeyResult);
        Console.WriteLine("Título atualizado com sucesso!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao atualizar o título: {ex.Message}");
    }
}

private void UpdateDescription()
{
    try
    {
        Console.Write("Nova Descrição: ");
        string newDescription = Console.ReadLine();
        _selectedKeyResult.Description = newDescription;
        _keyResultRepository.Update(_selectedKeyResult);
        Console.WriteLine("Descrição atualizada com sucesso!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao atualizar a descrição: {ex.Message}");
    }
}

private void UpdateStartDate()
{
    try
    {
        Console.Write("Nova Data de Início (formato: dd/MM/yyyy): ");
        if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime newStartDate))
        {
            _selectedKeyResult.StartDate = newStartDate;
            _keyResultRepository.Update(_selectedKeyResult);
            Console.WriteLine("Data de Início atualizada com sucesso!");
        }
        else
        {
            Console.WriteLine("Formato de data inválido. A data não foi atualizada.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao atualizar a data de início: {ex.Message}");
    }
}

private void UpdateEndDate()
{
    try
    {
        Console.Write("Nova Data de Término (formato: dd/MM/yyyy): ");
        if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime newEndDate))
        {
            _selectedKeyResult.EndDate = newEndDate;
            _keyResultRepository.Update(_selectedKeyResult);
            Console.WriteLine("Data de Término atualizada com sucesso!");
        }
        else
        {
            Console.WriteLine("Formato de data inválido. A data não foi atualizada.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao atualizar a data de término: {ex.Message}");
    }
}

private void DisplayKeyResultDetails()
{
    Console.WriteLine($"Título: {_selectedKeyResult.Title}");
    Console.WriteLine($"Descrição: {_selectedKeyResult.Description}");
    Console.WriteLine($"Data de Início: {_selectedKeyResult.StartDate.ToShortDateString()}");
    Console.WriteLine($"Data de Término: {_selectedKeyResult.EndDate.ToShortDateString()}");
    Console.WriteLine($"Status: {(_selectedKeyResult.Status ? "Concluído" : "Em andamento")}");
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
        Thread.Sleep(1500);
    }
    public void DisplayKeyResultCompletionPercentage(KeyResult keyResult)
    {
        var allSubTasks = _retrieveService.GetAllSubTasksForKeyResult(keyResult.KeyResultId);
        var concludedSubTasks = _retrieveService.GetAllConcludedSubTasksForKeyResult(keyResult.KeyResultId);

        if (allSubTasks.Any())
        {
            double completionPercentage = (double)concludedSubTasks.Count / allSubTasks.Count * 100;

            Console.WriteLine($"Porcentagem de Conclusão do Resultado Chave '{keyResult.Title}': {completionPercentage:F2}%");
        }
        else
        {
            Console.WriteLine($"O Resultado Chave '{keyResult.Title}' não possui Subtarefas associadas.");
        }
    }

}
