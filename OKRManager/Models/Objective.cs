using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OkrManager.Models;

public class Objective : Task
{
    
    private int _objectiveId;
    private int _userId;
    private User _relatedUser;
    private ICollection<KeyResult>? _keyResults;
    

    public int ObjectiveId {
        get => _objectiveId;
        set => _objectiveId = value;
    }
    public int UserId{
        get => _userId;
        set => _userId = value;
    }
    public User RelatedUser
    {
        get => _relatedUser;
        set => _relatedUser = value;
    }

    public ICollection<KeyResult>? KeyResults {
        get => _keyResults;
        set => _keyResults = value;
    }
    
    
    private Objective()
    {

    }

    public Objective( string title, string description, DateTime startDate, DateTime endDate, bool status, User relatedUser)
    {
        Title = title;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        Status = status;
        RelatedUser = relatedUser;
    }

}