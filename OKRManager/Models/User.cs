using System.ComponentModel.DataAnnotations;
using OkrManager.Repositories;

namespace OkrManager.Models;

public class User
{
	
	private int _userId;
	private string _name;
	private string _password;
	private ICollection<Objective>? _objectives;

	public int UserId
	{
		get => _userId;
		set => _userId = value;
	}
	public string Name
	{
		get => _name;
		set => _name = value;
	}
	public string Password
	{
		get => _password;
		set => _password = value;
	}

	public ICollection<Objective>? Objectives {
		get => _objectives;
		set => _objectives = value;
	}

	// Construtor sem parâmetros para o EF Core
	private User()
	{
		
	}
	
	public User(string name, string password)
	{
		Name = name;
		Password = password;
	}
	
	
}
