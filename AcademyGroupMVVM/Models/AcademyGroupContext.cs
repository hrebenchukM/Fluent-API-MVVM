using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace AcademyGroupMVVM.Models
{
    // Для работы с БД MS SQL Server необходимо добавить пакет:
    // Microsoft.EntityFrameworkCore.SqlServer(представляет функциональность Entity Framework для работы с MS SQL Server)

    // Lazy loading или ленивая загрузка предполагает неявную автоматическую загрузку связанных данных при обращении к навигационному свойству.
    // Microsoft.EntityFrameworkCore.Proxies

    public class AcademyGroupContext : DbContext
    {
        static DbContextOptions<AcademyGroupContext> _options;

        static AcademyGroupContext()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            string connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<AcademyGroupContext>();
            _options = optionsBuilder.UseLazyLoadingProxies().UseSqlServer(connectionString).Options;
        }
        public AcademyGroupContext() : base(_options)
        {
            if (Database.EnsureCreated())
            {
                AcademyGroup group1 = new AcademyGroup { Name = "СПУ112" };
                AcademyGroup group2 = new AcademyGroup { Name = "ПВ111" };
                AcademyGroup group3 = new AcademyGroup { Name = "ПР211" };
                AcademyGroups?.Add(group1);
                AcademyGroups?.Add(group2);
                AcademyGroups?.Add(group3);
                Students?.Add(new Student { FirstName = "Богдан", LastName = "Иваненко", Age = 20, GPA = 10.5, AcademyGroup = group1 });
                Students?.Add(new Student { FirstName = "Анна", LastName = "Шевченко", Age = 23, GPA = 11.5, AcademyGroup = group2 });
                Students?.Add(new Student { FirstName = "Петро", LastName = "Петренко", Age = 25, GPA = 12, AcademyGroup = group3 });
                Students?.Add(new Student { FirstName = "Елена", LastName = "Артемьева", Age = 42, GPA = 11.5, AcademyGroup = group1 });
                Students?.Add(new Student { FirstName = "Елена", LastName = "Алексеева", Age = 47, GPA = 12, AcademyGroup = group2 });
                Students?.Add(new Student { FirstName = "Виктория", LastName = "Бабенко", Age = 29, GPA = 10, AcademyGroup = group3 });

                SaveChanges();
            }
        }

        public DbSet<AcademyGroup> AcademyGroups { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Установим связь Один ко Многим между объектом AcademyGroup и объектами Student 

            modelBuilder.Entity<Student>().HasOne(p => p.AcademyGroup).WithMany(t => t.Students).OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
