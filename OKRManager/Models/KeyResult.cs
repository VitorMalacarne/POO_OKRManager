using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OkrManager.Models;

public class KeyResult : Task
{
    
    private int _keyResultId;
    private int _objectiveId;
    private Objective _relatedObjective;
    private ICollection<SubTask>? _subTasks;
    
    
    public int KeyResultId {
        get => _keyResultId;
        set => _keyResultId = value;
    }
    public int ObjectiveId {
        get => _objectiveId;
        set => _objectiveId = value;
    }
    public Objective RelatedObjective
    {
        get => _relatedObjective;
        set => _relatedObjective = value;
    }

    public ICollection<SubTask>? SubTasks {
        get => _subTasks;
        set => _subTasks = value;
    }
    

    // Construtor sem parâmetros para o EF Core
    private KeyResult()
    {

    }

    public KeyResult( string title, string description, DateTime startDate, DateTime endDate, bool status, Objective relatedObjective)
    {
        
        Title = title;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        Status = status;
        RelatedObjective = relatedObjective;
    }
}