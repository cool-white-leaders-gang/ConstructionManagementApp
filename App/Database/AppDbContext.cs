using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Enums;
using Task = ConstructionManagementApp.App.Models.Task;
using System.Security.Cryptography.X509Certificates;

namespace ConstructionManagementApp.App.Database
{
    internal class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }      //odwołanie do tabeli Users w bazie danych
        public DbSet<Task> Tasks { get; set; }     //odwołanie się do tabeli Tasks w bazie
        public DbSet<TaskAssignment> TaskAssignments { get; set; }  //odwołanie się do tabeli TaskAssignments w bazie
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMembers> TeamMembers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<ProgressReport> ProgressReports { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)   //elegnackie połączenie z baza danych
        {
            string server = "localhost";
            string database = "construction_management";
            string user = "root";
            string password = "";
            optionsBuilder.UseMySql($"server={server};database={database};user={user};password={password};", new MySqlServerVersion(new Version(10, 5, 9)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)  //konfiguracja modeli, żeby były poprawnie wpisywane
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.Role)
                      .HasConversion
                      (
                          v => v.ToString(),                                //konwersja enuma, bo EF nie obsługuje tego typu danych         
                          v => (Role)Enum.Parse(typeof(Role), v)
                      )
                      .IsRequired();
            });


            modelBuilder.Entity<Task>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Priority)
                      .HasConversion
                      (
                          v => v.ToString(),
                          v => (TaskPriority)Enum.Parse(typeof(TaskPriority), v)
                      )
                      .IsRequired();
                entity.Property(e => e.Progress)
                      .HasConversion
                      (
                          v => v.ToString(),
                          v => (TaskProgress)Enum.Parse(typeof(TaskProgress), v)
                      )
                      .IsRequired();
            });

            modelBuilder.Entity<TaskAssignment>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.TaskId });
                entity.HasOne(e => e.User)
                      .WithMany(e => e.TaskAssignments)
                      .HasForeignKey(e => e.UserId);
                entity.HasOne(e => e.Task)
                        .WithMany(e => e.TaskAssignments)
                        .HasForeignKey(e => e.TaskId);
            }
            );

            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Status)
                      .HasConversion
                      (
                          v => v.ToString(),
                          v => (EquipmentStatus)Enum.Parse(typeof(EquipmentStatus), v)
                      )
                      .IsRequired();
                entity.HasOne<Project>()                //kluczobcy ktory jest id tabeli project
                        .WithMany()
                        .HasForeignKey(e => e.ProjectId);
            });

            modelBuilder.Entity<Budget>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TotalAmount).IsRequired();
                entity.Property(e => e.SpentAmount).IsRequired();
            });
            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.HasOne<User>()                //klucz obcy ktory jest id managera z tabeli user
                      .WithMany()
                      .HasForeignKey(e => e.ManagerId);
            });

            modelBuilder.Entity<TeamMembers>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.TeamId });
                entity.HasOne(e => e.User)
                      .WithMany(e => e.TeamMembers)
                      .HasForeignKey(e => e.UserId);                //tabela lacznikowa miedzy user a team
                entity.HasOne(e => e.Team)
                        .WithMany(e => e.TeamMembers)
                        .HasForeignKey(e => e.TeamId);
            });                                    

            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Description).IsRequired();
                entity.HasOne<Team>()
                      .WithMany()
                      .HasForeignKey(e => e.TeamId);                //klucze obce reprezentujące id z tabeli team, budget oraz user
                entity.HasOne<Budget>()
                        .WithMany()
                        .HasForeignKey(e => e.BudgetId);
                entity.HasOne<User>()
                        .WithMany()
                        .HasForeignKey(e => e.ClientId);
            });

            modelBuilder.Entity<Issue>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.Priority)
                    .HasConversion(
                        v => v.ToString(),
                        v => (TaskPriority)Enum.Parse(typeof(TaskPriority), v)
                    )
                    .IsRequired();
                entity.Property(e => e.Status)
                    .HasConversion(
                        v => v.ToString(),
                        v => (IssueStatus)Enum.Parse(typeof(IssueStatus), v)
                    )
                    .IsRequired();
                entity.Property(e => e.ResolvedAt).IsRequired(false);
                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne<Project>()
                    .WithMany()
                    .HasForeignKey(e => e.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Material>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.Unit).IsRequired();
                entity.HasOne<Project>()
                      .WithMany()
                      .HasForeignKey(e => e.ProjectId);
            });

            modelBuilder.Entity<ProgressReport>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired();
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.CompletionPercentage)
                    .IsRequired(); // Ensure CompletionPercentage is required
                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.CreatedByUserId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne<Project>()
                    .WithMany()
                    .HasForeignKey(e => e.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            
        }
    }
}
