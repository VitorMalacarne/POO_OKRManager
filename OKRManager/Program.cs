using System;
using Microsoft.EntityFrameworkCore;
using OKRManager.UserInterface;
using OKRManager.Data;
using OkrManager.Interfaces;
using OkrManager.Models;
using OkrManager.Repositories;
using OkrManager.Services;
using OKRManager.UserInterface;

namespace OKRManager
{
    class Program
    {
        static void Main(string[] args)
        {
            
           
            var authService = new AuthService();
            var authInterface = new AuthenticationInterface(authService);
            
            authInterface.RunAuthentication();

        }
    }
}
