using System.ComponentModel.DataAnnotations;

namespace OkrManager.Models;

public abstract class Task
{
    private string _title;
    private string _description;
    private DateTime _startDate;
    private DateTime _endDate;
    private bool _status;

    public string Title
    {
        get => _title;
        set => _title = value;
    }

    public string Description
    {
        get => _description;
        set => _description = value;
    }

    public DateTime StartDate
    {
        get => _startDate;
        set => _startDate = value;
    }

    public DateTime EndDate
    {
        get => _endDate;
        set => _endDate = value;
    }

    public bool Status
    {
        get => _status;
        set => _status = value;
    }
}