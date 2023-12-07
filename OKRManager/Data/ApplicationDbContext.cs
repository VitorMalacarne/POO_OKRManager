using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OkrManager.Models;
using Task = System.Threading.Tasks.Task;

namespace OKRManager.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<KeyResult> KeyResults { get; set; }
    public DbSet<Objective> Objective { get; set; }
    public DbSet<SubTask> SubTask { get; set; }
    public DbSet<User> User { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql("server=localhost;database=OKRManager;user=root;password=0f8jsiz6ik5y",
            new MySqlServerVersion("10.4.28-MariaDB"));
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u => u.UserId);
        modelBuilder.Entity<Objective>().HasKey(o => o.ObjectiveId);
        modelBuilder.Entity<KeyResult>().HasKey(kr => kr.KeyResultId);
        modelBuilder.Entity<SubTask>().HasKey(st => st.SubTaskId);
        
        modelBuilder.Entity<SubTask>()
            .Property(e => e.Priority)
            .HasConversion<string>();


        base.OnModelCreating(modelBuilder);
    }
    

}