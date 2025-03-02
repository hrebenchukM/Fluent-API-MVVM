namespace AcademyGroupMVVM.Models
{
    public class Company
    {
        public int Ident { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
