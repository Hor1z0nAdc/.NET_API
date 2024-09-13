using FirstProject.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata;

namespace FirstProject.Data
{
    public class ProjectDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public ProjectDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sqlite database
            options.UseSqlite(Configuration.GetConnectionString("ProjectDatabase"));
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> Tasks { get; set; }

        private static readonly Guid ProjectTask1Id = Guid.Parse("ED743247-3136-4CAC-A5D3-F9917EDB9B32");
        private static readonly Guid ProjectTask2Id = Guid.Parse("DFEF3B56-E96D-417E-80AB-6BCE20D5A9F3");
        private static readonly Guid Project1Id = Guid.Parse("AC195296-02D9-41B8-B2A6-D52E03C1790B");
        private static readonly Guid Project2Id = Guid.Parse("685F0F79-E1C0-4B33-ADBF-D9BC8085FE86");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProjectTask>().ToTable("Tasks");

            modelBuilder.Entity<Project>()
               .HasMany(e => e.Tasks)
               .WithOne(e => e.Project)
               .HasForeignKey(e => e.ProjectId)
               .IsRequired();

             var projects = new List<Project>()
             {
                 new Project() { Id =  Project1Id, Name = "Desktop App" , NumOfDevelopers= 10, },
                 new Project() { Id =  Project2Id, Name = "Mobile App" , NumOfDevelopers = 4, }
             };

             var projectTasks = new List<ProjectTask>()
             {
                 new ProjectTask() { Id =  ProjectTask1Id, Title = "Desktop App design" , Description = "Description: Lorem ipsum", ProjectId = Project1Id },
                 new ProjectTask() { Id =  ProjectTask2Id, Title = "Mobile App design" , Description = "Description: Ispum lorem", ProjectId = Project1Id },
             };

             modelBuilder.Entity<Project>().HasData(projects);
             modelBuilder.Entity<ProjectTask>().HasData(projectTasks);
        }
    }
}
