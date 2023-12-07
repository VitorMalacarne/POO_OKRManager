using OkrManager.Interfaces;
using OkrManager.Models;
using OkrManager.Repositories;

namespace OkrManager.Services;

public class RetrieveService
{
    private readonly IRepository<Objective> _objectiveRepository = new Repository<Objective>();
    private readonly IRepository<KeyResult> _keyResultRepository = new Repository<KeyResult>();
    private readonly IRepository<SubTask> _subTaskRepository = new Repository<SubTask>();

    public List<Objective> GetAllObjectivesForUser(int userId)
    {
        var allObjectives = _objectiveRepository.GetAll();
        var userObjectives = new List<Objective>();

        foreach (var objective in allObjectives)
        {
            if (objective.UserId == userId && !objective.Status)
            {
                userObjectives.Add(objective);
            }
        }

        return userObjectives;
    }
    
    public List<KeyResult> GetAllKeyResultsForObject(int objectiveId)
    {
        var allKeyResults = _keyResultRepository.GetAll();
        var objectiveKeyResults = new List<KeyResult>();

        foreach (var keyResult in allKeyResults)
        {
            if (keyResult.ObjectiveId == objectiveId && !keyResult.Status)
            {
                objectiveKeyResults.Add(keyResult);
            }
        }

        return objectiveKeyResults;
    }

    public List<SubTask> GetAllSubTasksForKeyResult(int keyResultId)
    {
        var allSubTasks = _subTaskRepository.GetAll();
        var keyResultSubTasks = new List<SubTask>();

        foreach (var subTask in allSubTasks)
        {
            if (subTask.KeyResultId == keyResultId && !subTask.Status)
            {
                keyResultSubTasks.Add(subTask);
            }
        }

        return keyResultSubTasks;
    }
    
    public List<Objective> GetAllConcludedObjectivesForUser(int userId)
    {
        var allObjectives = _objectiveRepository.GetAll();
        var userObjectives = new List<Objective>();

        foreach (var objective in allObjectives)
        {
            if (objective.UserId == userId && objective.Status)
            {
                userObjectives.Add(objective);
            }
        }

        return userObjectives;
    }
    
    public List<KeyResult> GetAllConcludedKeyResultsForObject(int objectiveId)
    {
        var allKeyResults = _keyResultRepository.GetAll();
        var objectiveKeyResults = new List<KeyResult>();

        foreach (var keyResult in allKeyResults)
        {
            if (keyResult.ObjectiveId == objectiveId && keyResult.Status)
            {
                objectiveKeyResults.Add(keyResult);
            }
        }

        return objectiveKeyResults;
    }

    public List<SubTask> GetAllConcludedSubTasksForKeyResult(int keyResultId)
    {
        var allSubTasks = _subTaskRepository.GetAll();
        var keyResultSubTasks = new List<SubTask>();

        foreach (var subTask in allSubTasks)
        {
            if (subTask.KeyResultId == keyResultId && subTask.Status)
            {
                keyResultSubTasks.Add(subTask);
            }
        }

        return keyResultSubTasks;
    }
}
