using OkrManager.Interfaces;
using OkrManager.Models;
using OkrManager.Repositories;
using OkrManager.Services;

namespace OKRManager.UserInterface;

public class ObjectiveInterface
{
    private readonly Objective _selectedObjective;
    private readonly IRepository<KeyResult> _keyResultRepository = new Repository<KeyResult>();
    private readonly VerificationService _verificationService = new VerificationService();
    private readonly RetrieveService _retrieveService = new RetrieveService();

    public ObjectiveInterface(Objective selectedObjective)
    {
        _selectedObjective = selectedObjective;
    }

    public void RunObjectiveScreen()
    {
        Console.Clear();
        Console.WriteLine($"Detalhes do Objetivo: {_selectedObjective.Title}");
        Console.WriteLine($"Data de Início: {_selectedObjective.StartDate}");
        Console.WriteLine($"Data de Término: {_selectedObjective.EndDate}");
        Console.WriteLine($"Status: {_selectedObjective.Status}");

        DisplayKeyResults();

        int choice;
        do
        {
            Console.WriteLine("Escolha uma opção:");
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
                    Thread.Sleep(2000);
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

        if (keyResults.Any())
        {
            Console.WriteLine("\nResultados Chave Associados a Este Objetivo:");
            foreach (var keyResult in keyResults)
            {
                Console.WriteLine($"{keyResult.KeyResultId}. {keyResult.Title}");
            }
        }
        else
        {
            Console.WriteLine("\nNão há Resultados Chave associados a este objetivo.");
        }
    }

    private void AddKeyResult()
    {
        Console.WriteLine("Digite o título do novo Resultado Chave:");
        string title = _verificationService.VerifyIsNotNull(Console.ReadLine());

        // Adicione a lógica para permitir ao usuário inserir outras informações do Resultado Chave

        var newKeyResult = new KeyResult(title, "Descrição padrão", DateTime.Now, DateTime.Now.AddDays(30), false, _selectedObjective);

        _keyResultRepository.Update(newKeyResult);

        Console.WriteLine("Resultado Chave adicionado com sucesso!");
        Thread.Sleep(2000);
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
        Thread.Sleep(2000);
    }

    private void UpdateObjective()
    {
        // Adicione a lógica para permitir ao usuário atualizar as informações do Objetivo
        Console.WriteLine("Lógica para atualizar o Objetivo...");
        Thread.Sleep(2000);
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
