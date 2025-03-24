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
        public DbSet<Task> Tasks {get;set;}     //odwołanie się do tabeli Tasks w bazie
        public DbSet<TaskAssignment> TaskAssignments { get; set; }  //odwołanie się do tabeli TaskAssignments w bazie
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
                entity.HasOne(e => e.user)
                      .WithMany(e => e.TaskAssignments)
                      .HasForeignKey(e => e.UserId);
                entity.HasOne(e => e.task)
                        .WithMany(e => e.TaskAssignments)
                        .HasForeignKey(e => e.TaskId);
            }
            );

        }
    }
}
