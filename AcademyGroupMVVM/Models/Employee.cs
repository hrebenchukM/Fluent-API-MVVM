namespace AcademyGroupMVVM.Models
{
    public class Employee
    {
        public int Ident { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Position { get; set; }

        public virtual Company Company { get; set; }
    }
}
