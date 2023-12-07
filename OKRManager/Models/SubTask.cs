using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OKRManager.Enum;

namespace OkrManager.Models;

public class SubTask : Task
{
    
    private int _subTaskId;
    private int _keyResultId;
    private KeyResult _relatedKeyResult;
    private PriorityLevel _priority;
    
    public int SubTaskId{
        get => _subTaskId;
        set => _subTaskId = value;
    }
    public int KeyResultId {
        get => _keyResultId;
        set => _keyResultId = value;
    }
    public KeyResult  RelatedKeyResult
    {
        get => _relatedKeyResult;
        set => _relatedKeyResult = value;
    }

    public PriorityLevel Priority
    {
        get => _priority;
        set => _priority = value;
    }

    // Construtor sem parâmetros para o EF Core
    private SubTask()
    {

    }

    public SubTask(string title, string description, DateTime startDate, DateTime endDate, bool status, PriorityLevel priority, KeyResult  relatedKeyResult)
    {
        Title = title;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        Status = status;
        Priority = priority;
        RelatedKeyResult = relatedKeyResult;
    }
}