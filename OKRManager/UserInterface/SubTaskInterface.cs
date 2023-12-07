using System;
using System.Globalization;
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
            Console.WriteLine("Editando Subtarefa");
            Console.WriteLine("------------------");

            // Exibir detalhes da subtarefa
            DisplaySubTaskDetails();

            Console.WriteLine("\nOpções de Edição:");
            Console.WriteLine("1. Editar Título");
            Console.WriteLine("2. Editar Descrição");
            Console.WriteLine("3. Editar Data de Início");
            Console.WriteLine("4. Editar Data de Término");
            Console.WriteLine("5. Editar Prioridade");
            Console.WriteLine("6. Voltar ao Menu Anterior");

            Console.Write("Escolha uma opção: ");
            string choiceStr = Console.ReadLine();

            if (int.TryParse(choiceStr, out int choice))
            {
                switch (choice)
                {
                    case 1:
                        EditTitle();
                        break;
                    case 2:
                        EditDescription();
                        break;
                    case 3:
                        EditStartDate();
                        break;
                    case 4:
                        EditEndDate();
                        break;
                    case 5:
                        EditPriority();
                        break;
                    case 6:
                        // Voltar ao Menu Anterior
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

        private void DisplaySubTaskDetails()
        {
            Console.WriteLine($"Título: {_selectedSubTask.Title}");
            Console.WriteLine($"Descrição: {_selectedSubTask.Description}");
            Console.WriteLine($"Data de Início: {_selectedSubTask.StartDate.ToShortDateString()}");
            Console.WriteLine($"Data de Término: {_selectedSubTask.EndDate.ToShortDateString()}");
            Console.WriteLine($"Prioridade: {_selectedSubTask.Priority}");
        }

        private void EditTitle()
        {
            Console.Write("Novo Título: ");
            string newTitle = Console.ReadLine();
            _selectedSubTask.Title = newTitle;
            _subTaskRepository.Update(_selectedSubTask);
            Console.WriteLine("Título atualizado com sucesso!");
        }

        private void EditDescription()
        {
            Console.Write("Nova Descrição: ");
            string newDescription = Console.ReadLine();
            _selectedSubTask.Description = newDescription;
            _subTaskRepository.Update(_selectedSubTask);
            Console.WriteLine("Descrição atualizada com sucesso!");
        }

        private void EditStartDate()
        {
            Console.Write("Nova Data de Início (formato: dd/MM/yyyy): ");
            if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime newStartDate))
            {
                _selectedSubTask.StartDate = newStartDate;
                _subTaskRepository.Update(_selectedSubTask);
                Console.WriteLine("Data de Início atualizada com sucesso!");
            }
            else
            {
                Console.WriteLine("Formato de data inválido. A data não foi atualizada.");
            }
        }

        private void EditEndDate()
        {
            Console.Write("Nova Data de Término (formato: dd/MM/yyyy): ");
            if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime newEndDate))
            {
                _selectedSubTask.EndDate = newEndDate;
                _subTaskRepository.Update(_selectedSubTask);
                Console.WriteLine("Data de Término atualizada com sucesso!");
            }
            else
            {
                Console.WriteLine("Formato de data inválido. A data não foi atualizada.");
            }
        }

        private void EditPriority()
        {
            Console.WriteLine("\nNova Prioridade:");
            Console.WriteLine("Normal");
            Console.WriteLine("Importante");
            Console.WriteLine("Urgente");

            PriorityLevel priority;
            while (!System.Enum.TryParse(Console.ReadLine(), true, out priority))
            {
                Console.WriteLine("Prioridade inválida. Tente novamente (Urgente, Importante, Normal).");
            }

            _selectedSubTask.Priority = priority;
            _subTaskRepository.Update(_selectedSubTask);
            Console.WriteLine("Prioridade atualizada com sucesso!");
        }

    }
}
    

