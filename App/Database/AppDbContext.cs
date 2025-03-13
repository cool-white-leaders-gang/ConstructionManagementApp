using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Enums;

namespace ConstructionManagementApp.App.Database
{
    internal class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }      //odwołanie do tabeli Users w bazie danych
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)   //elegnackie połączenie z baza danych
        {
            string server = "localhost";
            string database = "construction_management";
            string user = "root";
            string password = "";
            optionsBuilder.UseMySql($"server={server};database={database};user={user};password={password};", new MySqlServerVersion(new Version(10, 5, 9)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)  //konfiguracja modelu user, żeby był poprawnie wpisywany
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Password).IsRequired();
                entity.Property(e => e.Role)
                      .HasConversion
                      (
                          v => v.ToString(),                                //konwersja enuma, bo EF nie obsługuje tego typu danych         
                          v => (Role)Enum.Parse(typeof(Role), v)
                      )
                      .IsRequired();
            });
        }
    }
}
