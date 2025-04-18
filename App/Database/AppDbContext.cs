using System;
using Microsoft.EntityFrameworkCore;
using ConstructionManagementApp.App.Models;
using ConstructionManagementApp.App.Enums;
using Task = ConstructionManagementApp.App.Models.Task; 

namespace ConstructionManagementApp.App.Database
{
    internal class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TaskAssignment> TaskAssignments { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMembers> TeamMembers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<ProgressReport> ProgressReports { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                string server = "localhost";
                string database = "construction_management";
                string user = "root";
                string password = ""; 

                optionsBuilder.UseMySql($"server={server};database={database};user={user};password={password};", new MySqlServerVersion(new Version(10, 4, 32)));

                if (!optionsBuilder.IsConfigured)
                {
                    optionsBuilder.EnableSensitiveDataLogging(); // Logowanie szczegółowe
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Nie można nawiązać połączenia z bazą danych. Upewnij się, że serwer MySQL jest uruchomiony.", ex);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users"); 
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).HasMaxLength(30).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(80).IsRequired();
                entity.Property(e => e.PasswordHash).HasMaxLength(64).IsFixedLength().IsRequired(); 
                entity.Property(e => e.Role)
                      .HasConversion(v => v.ToString(), v => (Role)Enum.Parse(typeof(Role), v))
                      .IsRequired();
            });

            modelBuilder.Entity<Budget>(entity =>
            {
                entity.ToTable("budgets");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.SpentAmount).HasColumnType("decimal(10,2)").IsRequired();
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("teams");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(255).IsRequired();

                entity.HasOne<User>()
                      .WithMany()
                      .HasForeignKey(e => e.ManagerId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("projects");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Description).HasColumnType("text").IsRequired();

                entity.HasOne<Team>()
                      .WithMany()
                      .HasForeignKey(e => e.TeamId)
                      .IsRequired(false) 
                      .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne<Budget>()
                      .WithMany()
                      .HasForeignKey(e => e.BudgetId)
                      .IsRequired(false) 
                      .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne<User>()
                      .WithMany()
                      .HasForeignKey(e => e.ClientId)
                      .IsRequired(false) 
                      .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.ToTable("equipment");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Status)
                      .HasMaxLength(50) 
                      .HasConversion(v => v.ToString(), v => (EquipmentStatus)Enum.Parse(typeof(EquipmentStatus), v))
                      .IsRequired();


                entity.HasOne<Project>()
                      .WithMany() 
                      .HasForeignKey(e => e.ProjectId)
                      .IsRequired(false) 
                      .OnDelete(DeleteBehavior.Cascade); 
            });

            modelBuilder.Entity<Material>(entity =>
            {
                entity.ToTable("materials");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.Unit).HasMaxLength(50).IsRequired();

                entity.HasOne<Project>()
                      .WithMany() 
                      .HasForeignKey(e => e.ProjectId)
                      .IsRequired(false) 
                      .OnDelete(DeleteBehavior.Cascade); 
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("tasks");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Description).HasColumnType("text").IsRequired(); 
                entity.Property(e => e.Priority)
                      .HasConversion(v => v.ToString(), v => (TaskPriority)Enum.Parse(typeof(TaskPriority), v))
                      .IsRequired();
                entity.Property(e => e.Progress)
                      .HasConversion(v => v.ToString(), v => (TaskProgress)Enum.Parse(typeof(TaskProgress), v))
                      .IsRequired();

                
                entity.HasOne<Project>()
                      .WithMany() 
                      .HasForeignKey(e => e.ProjectId)
                      .IsRequired() 
                      .OnDelete(DeleteBehavior.Cascade); 
            });

            
            modelBuilder.Entity<TaskAssignment>(entity =>
            {
                entity.ToTable("taskassignments");
                entity.HasKey(e => new { e.TaskId, e.UserId }); 

                entity.HasOne(e => e.Task)
                      .WithMany(t => t.TaskAssignments) 
                      .HasForeignKey(e => e.TaskId)
                      .OnDelete(DeleteBehavior.Cascade); 


                entity.HasOne(e => e.User)
                      .WithMany(u => u.TaskAssignments) 
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade); 
            });

            
            modelBuilder.Entity<TeamMembers>(entity =>
            {
                entity.ToTable("teammembers");
                entity.HasKey(e => new { e.TeamId, e.UserId }); 

                
                entity.HasOne(e => e.Team)
                      .WithMany(t => t.TeamMembers) 
                      .HasForeignKey(e => e.TeamId)
                      .OnDelete(DeleteBehavior.Cascade); 

                
                entity.HasOne(e => e.User)
                      .WithMany(u => u.TeamMembers) 
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade); 
            });

           
            modelBuilder.Entity<Issue>(entity =>
            {
                entity.ToTable("issues");
                entity.HasKey(e => e.Id); 
                entity.Property(e => e.Title).HasMaxLength(255).IsRequired(); 
                entity.Property(e => e.Content).HasColumnType("text").IsRequired();
                entity.Property(e => e.CreatedAt).HasColumnType("datetime").IsRequired();
                entity.Property(e => e.ResolvedAt).HasColumnType("datetime").IsRequired(false); 
                entity.Property(e => e.Priority)
                    .HasConversion(v => v.ToString(), v => (TaskPriority)Enum.Parse(typeof(TaskPriority), v))
                    .IsRequired();
                entity.Property(e => e.Status)
                    .HasConversion(v => v.ToString(), v => (IssueStatus)Enum.Parse(typeof(IssueStatus), v))
                    .IsRequired();

                
                entity.HasOne<User>()
                    .WithMany() 
                    .HasForeignKey(e => e.CreatedByUserId)
                    .IsRequired() 
                    .OnDelete(DeleteBehavior.ClientSetNull); 

               
                entity.HasOne<Project>()
                    .WithMany() 
                    .HasForeignKey(e => e.ProjectId)
                    .IsRequired() 
                    .OnDelete(DeleteBehavior.Cascade); 
            });

            modelBuilder.Entity<ProgressReport>(entity =>
            {
                entity.ToTable("progressreports");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Title).HasMaxLength(255).IsRequired();
                entity.Property(e => e.Content).HasColumnType("text").IsRequired();
                entity.Property(e => e.CreatedAt).HasColumnType("datetime").IsRequired();
                entity.Property(e => e.CompletionPercentage).IsRequired();

                entity.HasOne<User>()
                    .WithMany() 
                    .HasForeignKey(e => e.CreatedByUserId)
                    .IsRequired(false) 
                    .OnDelete(DeleteBehavior.ClientSetNull); 

                entity.HasOne<Project>()
                    .WithMany() 
                    .HasForeignKey(e => e.ProjectId)
                    .IsRequired() 
                    .OnDelete(DeleteBehavior.Cascade); 
            });

            // Log Configuration
            modelBuilder.Entity<Log>(entity =>
            {
                entity.ToTable("logs");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Message).HasColumnType("text").IsRequired();
                entity.Property(e => e.Timestamp).HasColumnType("datetime").IsRequired();
                entity.Property(e => e.UserName).HasMaxLength(30).IsRequired();
            });

            // Message Configuration
            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("messages");
                entity.HasKey(e => e.Id); 
                entity.Property(e => e.Content).HasColumnType("text").IsRequired();
                entity.Property(e => e.SentAt).HasColumnType("datetime").IsRequired();

                entity.HasOne<User>()
                      .WithMany() 
                      .HasForeignKey(e => e.SenderId)
                      .IsRequired() 
                      .OnDelete(DeleteBehavior.Restrict); 

                entity.HasOne<User>()
                     .WithMany() 
                     .HasForeignKey(e => e.ReceiverId)
                     .IsRequired() 
                     .OnDelete(DeleteBehavior.Restrict); 
            });
        }
    }
}
