using AcademyGroupMVVM.Commands;
using AcademyGroupMVVM.Models;
using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Input;

namespace AcademyGroupMVVM.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        public ObservableCollection<GroupViewModel> GroupsList { get; set; }
        public ObservableCollection<StudentViewModel> StudentsList { get; set; }

        public MainViewModel(IQueryable<AcademyGroup> groups, IQueryable<Student> students)
        {
            GroupsList = new ObservableCollection<GroupViewModel>(groups.Select(g => new GroupViewModel(g)));
            StudentsList = new ObservableCollection<StudentViewModel>(students.Select(st => new StudentViewModel(st))); 
        }

        private string name;

        public string GroupName
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(nameof(GroupName));
            }
        }

        private int index_selected_groups = -1;

        public int Index_selected_groups
        {
            get { return index_selected_groups; }
            set
            {
                index_selected_groups = value;
                OnPropertyChanged(nameof(Index_selected_groups));
            }
        }

        private int index_selected_students = -1;

        public int Index_selected_students
        {
            get { return index_selected_students; }
            set
            {
                index_selected_students = value;
                OnPropertyChanged(nameof(Index_selected_students));
            }
        }

        private string firstname;

        public string FirstName
        {
            get
            {
                return firstname;
            }
            set
            {
                firstname = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        private string lastname;

        public string LastName
        {
            get
            {
                return lastname;
            }
            set
            {
                lastname = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        private int? age;

        public int? Age
        {
            get
            {
                return age;
            }
            set
            {
                age = value;
                OnPropertyChanged(nameof(Age));
            }
        }

        private double? gpa;

        public double? GPA
        {
            get
            {
                return gpa;
            }
            set
            {
                gpa = value;
                OnPropertyChanged(nameof(GPA));
            }
        }

        private DelegateCommand refreshGroupCommand;

        public ICommand RefreshGroupCommand
        {
            get
            {
                if (refreshGroupCommand == null)
                {
                    refreshGroupCommand = new DelegateCommand(param => RefreshGroup(), null);
                }
                return refreshGroupCommand;
            }
        }

        private void RefreshGroup()
        {
            try
            {
                using (var db = new AcademyGroupContext())
                {
                    var groups = from g in db.AcademyGroups
                                 select g;
                    GroupsList = new ObservableCollection<GroupViewModel>(groups.Select(g => new GroupViewModel(g)));
                    OnPropertyChanged(nameof(GroupsList));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private DelegateCommand refreshStudentCommand;

        public ICommand RefreshStudentCommand
        {
            get
            {
                if (refreshStudentCommand == null)
                {
                    refreshStudentCommand = new DelegateCommand(param => RefreshStudent(), null);
                }
                return refreshStudentCommand;
            }
        }

        private void RefreshStudent()
        {
            try
            {
                using (var db = new AcademyGroupContext())
                {
                    var groups = from g in db.AcademyGroups
                                 select g;
                    var students = from st in db.Students
                                   select st;
                    GroupsList = new ObservableCollection<GroupViewModel>(groups.Select(g => new GroupViewModel(g)));
                    StudentsList = new ObservableCollection<StudentViewModel>(students.Select(st => new StudentViewModel(st)));
                    OnPropertyChanged(nameof(GroupsList));
                    OnPropertyChanged(nameof(StudentsList));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private DelegateCommand addGroupCommand;

        public ICommand AddGroupCommand
        {
            get
            {
                if (addGroupCommand == null)
                {
                    addGroupCommand = new DelegateCommand(param => AddGroup(), param => CanAddGroup());
                }
                return addGroupCommand;
            }
        }

        private DelegateCommand removeGroupCommand;

        public ICommand RemoveGroupCommand
        {
            get
            {
                if (removeGroupCommand == null)
                {
                    removeGroupCommand = new DelegateCommand(param => RemoveGroup(), param => CanRemoveGroup());
                }
                return removeGroupCommand;
            }
        }

        private DelegateCommand updateGroupCommand;

        public ICommand UpdateGroupCommand
        {
            get
            {
                if (updateGroupCommand == null)
                {
                    updateGroupCommand = new DelegateCommand(param => UpdateGroup(), param => CanUpdateGroup());
                }
                return updateGroupCommand;
            }
        }

        private void AddGroup()
        {
            try
            {
                using (var db = new AcademyGroupContext())
                {
                    var academygroup = new AcademyGroup { Name = GroupName };
                    db.AcademyGroups.Add(academygroup);
                    db.SaveChanges();
                    var groupviewmodel = new GroupViewModel(academygroup);
                    GroupsList.Add(groupviewmodel);
                    MessageBox.Show("Группа добавлена!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CanAddGroup()
        {
            return !GroupName.IsNullOrEmpty();
        }

        private void RemoveGroup()
        {
            try
            {
                var delgroup = GroupsList[Index_selected_groups];
                DialogResult result = MessageBox.Show("Вы действительно желаете удалить группу " + delgroup.Name +
                    " ?", "Удаление группы", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                    return;
                using (var db = new AcademyGroupContext())
                {
                    var query = (from g in db.AcademyGroups
                                where g.Name == delgroup.Name
                                select g).Single();
                    db.AcademyGroups.Remove(query);
                    db.SaveChanges();
                    GroupsList.Remove(delgroup);
                    MessageBox.Show("Группа удалена!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CanRemoveGroup()
        {
            return Index_selected_groups != -1;
        }

        private void UpdateGroup()
        {
            try
            {
                using (var db = new AcademyGroupContext())
                {
                    var updategroup = GroupsList[Index_selected_groups];
                    var query = (from g in db.AcademyGroups
                                 where g.Name == updategroup.Name
                                 select g).Single();
                    query.Name = GroupName;
                    db.SaveChanges();
                    GroupsList[Index_selected_groups] = new GroupViewModel(query);
                    MessageBox.Show("Группа обновлена!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }           
        }

        private bool CanUpdateGroup()
        {
            return !GroupName.IsNullOrEmpty() && Index_selected_groups != -1;
        }

        private DelegateCommand addStudentCommand;

        public ICommand AddStudentCommand
        {
            get
            {
                if (addStudentCommand == null)
                {
                    addStudentCommand = new DelegateCommand(param => AddStudent(), param => CanAddStudent());
                }
                return addStudentCommand;
            }
        }

        private void AddStudent()
        {
            try
            {
                using (var db = new AcademyGroupContext())
                {
                    var academygroup = GroupsList[Index_selected_groups];
                    var query = (from g in db.AcademyGroups
                                 where g.Name == academygroup.Name
                                 select g).Single();

                    var student = new Student
                    {
                        FirstName = firstname,
                        LastName = lastname,
                        Age = age,
                        GPA = gpa,
                        AcademyGroup = query
                    };
                    db.Students.Add(student);
                    db.SaveChanges();
                    var studentviewmodel = new StudentViewModel(student);
                    StudentsList.Add(studentviewmodel);

                    MessageBox.Show("Студент добавлен!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CanAddStudent()
        {
            return !FirstName.IsNullOrEmpty() && !LastName.IsNullOrEmpty() && Age.HasValue && GPA.HasValue && Index_selected_groups != -1;
                
        }

        private DelegateCommand removeStudentCommand;

        public ICommand RemoveStudentCommand
        {
            get
            {
                if (removeStudentCommand == null)
                {
                    removeStudentCommand = new DelegateCommand(param => RemoveStudent(), param => CanRemoveStudent());
                }
                return removeStudentCommand;
            }
        }

        private void RemoveStudent()
        {
            try
            {
                var delstudent = StudentsList[Index_selected_students];
                DialogResult result = MessageBox.Show("Вы действительно желаете удалить студента " + delstudent.LastName +
                    " ?", "Удаление группы", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Cancel)
                    return;
                using (var db = new AcademyGroupContext())
                {
                    var query = from st in db.Students
                                where st.LastName == delstudent.LastName
                                select st;
                    db.Students.RemoveRange(query);
                    db.SaveChanges();
                    StudentsList.Remove(delstudent);
                    MessageBox.Show("Студент удален!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CanRemoveStudent()
        {
            return Index_selected_students != -1;
        }

        private DelegateCommand updateStudentCommand;

        public ICommand UpdateStudentCommand
        {
            get
            {
                if (updateStudentCommand == null)
                {
                    updateStudentCommand = new DelegateCommand(param => UpdateStudent(), param => CanUpdateStudent());
                }
                return updateStudentCommand;
            }
        }

        private void UpdateStudent()
        {
            try
            {
                using (var db = new AcademyGroupContext())
                {
                    var academygroup = GroupsList[Index_selected_groups];
                    var group = (from g in db.AcademyGroups
                                 where g.Name == academygroup.Name
                                 select g).Single();
                    var updatestudent = StudentsList[Index_selected_students];
                    if (group == null)
                        return;

                    var student = (from st in db.Students
                                  where st.LastName == updatestudent.LastName
                                  select st).Single();

                    student.AcademyGroup = group;
                    student.FirstName = firstname;
                    student.LastName = lastname;
                    student.Age = age;
                    student.GPA = gpa;
                    db.SaveChanges();
                    StudentsList[Index_selected_students] = new StudentViewModel(student);
                    MessageBox.Show("Данные о студенте изменены!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool CanUpdateStudent()
        {
            return !FirstName.IsNullOrEmpty() && !LastName.IsNullOrEmpty() && Age.HasValue && 
                GPA.HasValue && Index_selected_groups != -1 && Index_selected_students != -1;

        }
    }
}
