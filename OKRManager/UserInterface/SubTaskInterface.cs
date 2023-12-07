using System;
using System.Linq;
using OKRManager.Enum;
using OkrManager.Interfaces;
using OkrManager.Models;
using OkrManager.Repositories;
using OkrManager.Services;

namespace OKRManager.UserInterface
{
    public class SubTaskInterface
    {
        private readonly SubTask _selectedSubTask;
        private readonly IRepository<SubTask> _subTaskRepository;
        private readonly VerificationService _verificationService;

        public SubTaskInterface(SubTask selectedSubTask)
        {
            _selectedSubTask = selectedSubTask;
            _subTaskRepository = new Repository<SubTask>();
            _verificationService = new VerificationService();
        }

        public void RunSubTaskScreen()
        {
            Console.Clear();
            Console.WriteLine("Detalhes da SubTask:\n");

            Console.WriteLine($"Título: {_selectedSubTask.Title}");
            Console.WriteLine($"Descrição: {_selectedSubTask.Description}");
            Console.WriteLine($"Data de Início: {_selectedSubTask.StartDate.ToShortDateString()}");
            Console.WriteLine($"Data de Término: {_selectedSubTask.EndDate.ToShortDateString()}");
            Console.WriteLine($"Prioridade: {_selectedSubTask.Priority}");
            Console.WriteLine($"Status: {_selectedSubTask.Status}");

            while (true)
            {
                Console.WriteLine("\nOpções:");
                Console.WriteLine("1. Editar SubTask");
                Console.WriteLine("0. Voltar ao Menu Principal");

                string choiceStr = Console.ReadLine();
                if (int.TryParse(choiceStr, out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            EditSubTask();
                            break;
                        case 0:
                            return;
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
        }


        private void EditSubTask()
        {
            Console.Clear();
            Console.WriteLine("Edição da SubTask:\n");

            Console.WriteLine("Digite o novo título da SubTask:");
            _selectedSubTask.Title = _verificationService.VerifyIsNotNull(Console.ReadLine());

            Console.WriteLine("Digite a nova descrição da SubTask:");
            _selectedSubTask.Description = _verificationService.VerifyIsNotNull(Console.ReadLine());

            Console.WriteLine("Digite a nova data de início da SubTask (formato: dd/MM/yyyy):");
            DateTime newStartDate;
            while (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out newStartDate))
            {
                Console.WriteLine("Formato de data inválido. Tente novamente.");
            }
            _selectedSubTask.StartDate = newStartDate;

            Console.WriteLine("Digite a nova data de término da SubTask (formato: dd/MM/yyyy):");
            DateTime newEndDate;
            while (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out newEndDate))
            {
                Console.WriteLine("Formato de data inválido. Tente novamente.");
            }
            _selectedSubTask.EndDate = newEndDate;

            Console.WriteLine("Digite a nova prioridade da SubTask (Normal, Importante, Urgente):");
            PriorityLevel priority;
            while (!System.Enum.TryParse(Console.ReadLine(), true, out priority))
            {
                Console.WriteLine("Prioridade inválida. Tente novamente (Urgente, Importante, Normal).");
            }

            _selectedSubTask.Priority = priority;

            Console.WriteLine("SubTask editada com sucesso!");
            
        }
    }
}
