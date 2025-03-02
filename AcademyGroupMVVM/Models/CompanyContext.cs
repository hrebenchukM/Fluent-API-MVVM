using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System;

namespace AcademyGroupMVVM.Models
{
    // Для работы с БД MS SQL Server необходимо добавить пакет:
    // Microsoft.EntityFrameworkCore.SqlServer(представляет функциональность Entity Framework для работы с MS SQL Server)

    // Lazy loading или ленивая загрузка предполагает неявную автоматическую загрузку связанных данных при обращении к навигационному свойству.
    // Microsoft.EntityFrameworkCore.Proxies

    public class CompanyContext : DbContext
    {
        static DbContextOptions<CompanyContext> _options;

        static CompanyContext()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            string connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<CompanyContext>();
            _options = optionsBuilder.UseLazyLoadingProxies().UseSqlServer(connectionString).Options;
        }
        public CompanyContext() : base(_options)
        {
           
            if (Database.EnsureCreated())
            {
                Company company1 = new Company { Name = "Luxoft" };
                Company company2 = new Company { Name = "Rozetka" };
                Company company3 = new Company { Name = "DataArt" };
                Companies?.Add(company1);
                Companies?.Add(company2);
                Companies?.Add(company3);
                Employees?.Add(new Employee { FirstName = "Богдан", LastName = "Иваненко", Age = 20, Position = "Fullstack developer", Company = company1 });
                Employees?.Add(new Employee { FirstName = "Анна", LastName = "Шевченко", Age = 23, Position = "Front-end developer", Company = company2 });
                Employees?.Add(new Employee { FirstName = "Петро", LastName = "Петренко", Age = 25, Position = "Backend developer", Company = company3 });
                Employees?.Add(new Employee { FirstName = "Елена", LastName = "Артемьева", Age = 42, Position = "Manual QA Engineer", Company = company1 });
                Employees?.Add(new Employee { FirstName = "Елена", LastName = "Алексеева", Age = 47, Position = "Automation QA Engineer", Company = company2 });
                Employees?.Add(new Employee { FirstName = "Виктория", LastName = "Бабенко", Age = 29, Position = "Team Lead", Company = company3 });

                SaveChanges();
            }
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Сопоставление класса с таблицей
            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<Company>().ToTable("Companies");


            // Переопределение первичного ключа
            modelBuilder.Entity<Employee>().HasKey(p => p.Ident);
            modelBuilder.Entity<Company>().HasKey(p => p.Ident);


            // Сопоставление свойств
            modelBuilder.Entity<Employee>().Property(p => p.FirstName).HasColumnName("EmployeeName");
            modelBuilder.Entity<Employee>().Property(p => p.LastName).HasColumnName("EmployeeSurname");
            modelBuilder.Entity<Employee>().Property(u => u.Age).HasDefaultValue(18);
            modelBuilder.Entity<Employee>()
            .ToTable(t => t.HasCheckConstraint("Age", "Age >= 18 AND Age < 80"));
            modelBuilder.Entity<Employee>().Property(p => p.Position).HasColumnName("EmployeePosition");
           



            // Значение для столбца и свойства требуется обязательно
            modelBuilder.Entity<Employee>().Property(p => p.FirstName).IsRequired();
            modelBuilder.Entity<Employee>().Property(p => p.LastName).IsRequired();
            modelBuilder.Entity<Company>().Property(c => c.Name).IsRequired();

            // Настройка строк
            modelBuilder.Entity<Employee>().Property(p => p.FirstName).HasMaxLength(20);
            modelBuilder.Entity<Employee>().Property(p => p.LastName).HasMaxLength(20);
            modelBuilder.Entity<Employee>().Property(p => p.FirstName).IsUnicode(false);
            modelBuilder.Entity<Employee>().Property(p => p.LastName).IsUnicode(false);

          
            // Настройка типа столбцов
            modelBuilder.Entity<Employee>().Property(p => p.Position).HasColumnType("varchar").HasMaxLength(20);


            // Установим связь Один ко Многим между объектом AcademyGroup и объектами Student 

            modelBuilder.Entity<Employee>().HasOne(p => p.Company).WithMany(t => t.Employees).OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
