using System.Globalization;
using OkrManager.Interfaces;
using OkrManager.Models;
using OkrManager.Repositories;
using OkrManager.Services;

namespace OKRManager.UserInterface;

public class ObjectiveInterface
{
    private readonly Objective _selectedObjective;
    private readonly IRepository<KeyResult> _keyResultRepository = new Repository<KeyResult>();
    private readonly IRepository<Objective> _objectiveRepository = new Repository<Objective>();
    private readonly VerificationService _verificationService = new VerificationService();
    private readonly RetrieveService _retrieveService = new RetrieveService();

    public ObjectiveInterface(Objective selectedObjective)
    {
        _selectedObjective = selectedObjective;
    }

    public void RunObjectiveScreen()
    {
        Console.Clear();
        Console.WriteLine($"Título do Objetivo: {_selectedObjective.Title}");
        Console.WriteLine($"Detalhes do Objetivo: {_selectedObjective.Description}");
        Console.WriteLine($"Data de Início: {_selectedObjective.StartDate}");
        Console.WriteLine($"Data de Término: {_selectedObjective.EndDate}");

        DisplayKeyResults();

        int choice;
        do
        {
            Console.WriteLine("\nEscolha uma opção:");
            Console.WriteLine("1. Adicionar Resultado Chave");
            Console.WriteLine("2. Selecionar Resultado Chave");
            Console.WriteLine("3. Alterar Objetivo");
            Console.WriteLine("4. Excluir Resultado Chave");
            Console.WriteLine("0. Voltar para a Tela Anterior");
            Console.Write("Digite sua opção aqui:");

            string choiceStr = Console.ReadLine();
            choice = _verificationService.VerifyIsNumber(choiceStr);

            switch (choice)
            {
                case 1:
                    AddKeyResult();
                    break;
                case 2:
                    SelectKeyResult();
                    break;
                case 3:
                    UpdateObjective();
                    break;
                case 4:
                    DeleteKeyResult();
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

    private void DisplayKeyResults()
    {
        var keyResults = _retrieveService.GetAllKeyResultsForObject(_selectedObjective.ObjectiveId);
        var concludedKeyResults = _retrieveService.GetAllConcludedKeyResultsForObject(_selectedObjective.ObjectiveId);

        if (keyResults.Any() || concludedKeyResults.Any())
        {
            Console.WriteLine("\nResultados Chave Associados a Este Objetivo:");
            Console.WriteLine("{0, -5} {1, -30} {2, -15}", "ID", "Título", "Status");

            foreach (var keyResult in keyResults)
            {
                Console.WriteLine("{0, -5} {1, -30} {2, -15}",
                    keyResult.KeyResultId,
                    keyResult.Title,
                    "Em Andamento");
            }

            if (concludedKeyResults.Any())
            {
                Console.WriteLine("\nResultados Chave Concluídos:");
                foreach (var concludedKeyResult in concludedKeyResults)
                {
                    Console.WriteLine("{0, -5} {1, -30} {2, -15}",
                        concludedKeyResult.KeyResultId,
                        concludedKeyResult.Title,
                        "Concluído");
                }
            }
        }
        else
        {
            Console.WriteLine("\nNão há Resultados Chave associados a este objetivo.");
        }
    }



    private void AddKeyResult()
    {
        try
        {
            Console.WriteLine("Digite o título do novo Resultado Chave:");
            string title = _verificationService.VerifyIsNotNull(Console.ReadLine());

            Console.WriteLine("Digite a descrição do novo Resultado Chave:");
            string description = _verificationService.VerifyIsNotNull(Console.ReadLine());

            Console.WriteLine("Digite a data de início do novo Resultado Chave (formato: dd/MM/yyyy):");
            DateTime startDate;
            while (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate) || startDate < DateTime.Now)
            {
                Console.WriteLine("Data inválida. Certifique-se de inserir uma data válida e futura.");
            }

            Console.WriteLine("Digite a data de término do novo Resultado Chave (formato: dd/MM/yyyy):");
            DateTime endDate;
            while (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate) || endDate < DateTime.Now)
            {
                Console.WriteLine("Data inválida. Certifique-se de inserir uma data válida e futura.");
            }

            var newKeyResult = new KeyResult(title, description, startDate, endDate, false, _selectedObjective);

            _keyResultRepository.Update(newKeyResult);

            Console.WriteLine("Resultado Chave adicionado com sucesso!");
            Thread.Sleep(1500);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao adicionar Resultado Chave: {ex.Message}");
        }
    }



    private void SelectKeyResult()
    {
        Console.Clear();
        Console.WriteLine($"Selecione um Resultado Chave associado ao Objetivo '{_selectedObjective.Title}':");
        
        var keyResults = _retrieveService.GetAllKeyResultsForObject(_selectedObjective.ObjectiveId);
        DisplayKeyResults();

        Console.Write("Digite o número do Resultado Chave desejado:");
        int selectedKeyResultId;
        while (!int.TryParse(Console.ReadLine(), out selectedKeyResultId) || !keyResults.Any(kr => kr.KeyResultId == selectedKeyResultId))
        {
            Console.WriteLine("Opção inválida. Tente novamente.");
        }

        var selectedKeyResult = _keyResultRepository.GetById(selectedKeyResultId);
        var keyResultInterface = new KeyResultInterface(selectedKeyResult);
        keyResultInterface.RunKeyResultScreen();
        Console.WriteLine($"Você selecionou o Resultado Chave com Id {selectedKeyResultId}.");
        Thread.Sleep(1500);
    }

    private void UpdateObjective()
{
    Console.Clear();
    Console.WriteLine("Atualizando Objetivo");
    Console.WriteLine("---------------------");

    // Exibir detalhes do objetivo
    DisplayObjectiveDetails();

    Console.WriteLine("\nOpções de Atualização:");
    Console.WriteLine("1. Atualizar Título");
    Console.WriteLine("2. Atualizar Descrição");
    Console.WriteLine("3. Atualizar Data de Início");
    Console.WriteLine("4. Atualizar Data de Término");
    Console.WriteLine("5. Marcar como Concluído");
    Console.WriteLine("0. Voltar ao Menu Principal");

    Console.Write("Escolha uma opção: ");
    string choiceStr = Console.ReadLine();
    
    if (int.TryParse(choiceStr, out int choice))
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
            case 0:
                // Voltar ao Menu Principal
                break;
            default:
                Console.WriteLine("Opção inválida. Tente novamente.");
                break;
        }
    }
    else
    {
        Console.WriteLine("Opção inválida. Tente novamente.");
    }
}

private void DisplayObjectiveDetails()
{
    Console.WriteLine($"Título: {_selectedObjective.Title}");
    Console.WriteLine($"Descrição: {_selectedObjective.Description}");
    Console.WriteLine($"Data de Início: {_selectedObjective.StartDate.ToShortDateString()}");
    Console.WriteLine($"Data de Término: {_selectedObjective.EndDate.ToShortDateString()}");
}

private void UpdateTitle()
{
    Console.Write("Novo Título: ");
    string newTitle = Console.ReadLine();
    _selectedObjective.Title = newTitle;
    _objectiveRepository.Update(_selectedObjective);
    Console.WriteLine("Título atualizado com sucesso!");
}

private void UpdateDescription()
{
    Console.Write("Nova Descrição: ");
    string newDescription = Console.ReadLine();
    _selectedObjective.Description = newDescription;
    _objectiveRepository.Update(_selectedObjective);
    Console.WriteLine("Descrição atualizada com sucesso!");
}

private void UpdateStartDate()
{
    Console.Write("Nova Data de Início (formato: dd/MM/yyyy): ");
    if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime newStartDate))
    {
        _selectedObjective.StartDate = newStartDate;
        _objectiveRepository.Update(_selectedObjective);
        Console.WriteLine("Data de Início atualizada com sucesso!");
    }
    else
    {
        Console.WriteLine("Formato de data inválido. A data não foi atualizada.");
    }
}

private void UpdateEndDate()
{
    Console.Write("Nova Data de Término (formato: dd/MM/yyyy): ");
    if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime newEndDate))
    {
        _selectedObjective.EndDate = newEndDate;
        _objectiveRepository.Update(_selectedObjective);
        Console.WriteLine("Data de Término atualizada com sucesso!");
    }
    else
    {
        Console.WriteLine("Formato de data inválido. A data não foi atualizada.");
    }
}
private void MarkAsCompleted()
{
    _selectedObjective.Status = true;
    _objectiveRepository.Update(_selectedObjective);
    Console.WriteLine("Objetivo marcado como concluído com sucesso!");
}


    private void DeleteKeyResult()
    {
        Console.Clear();
        Console.WriteLine($"Selecione um Resultado Chave associado ao Objetivo '{_selectedObjective.Title}' para excluir:");
        
        var keyResults = _retrieveService.GetAllKeyResultsForObject(_selectedObjective.ObjectiveId);
        DisplayKeyResults();

        Console.Write("Digite o número do Resultado Chave desejado para excluir:");
        int selectedKeyResultId;
        while (!int.TryParse(Console.ReadLine(), out selectedKeyResultId) || !keyResults.Any(kr => kr.KeyResultId == selectedKeyResultId))
        {
            Console.WriteLine("Opção inválida. Tente novamente.");
        }

        _keyResultRepository.Delete(selectedKeyResultId);
        Console.WriteLine("Resultado Chave excluído com sucesso!");
        Thread.Sleep(2000);
    }
}
