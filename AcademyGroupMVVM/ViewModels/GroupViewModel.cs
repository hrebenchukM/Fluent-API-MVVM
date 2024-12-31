using AcademyGroupMVVM.Models;
namespace AcademyGroupMVVM.ViewModels
{
    public class GroupViewModel : ViewModelBase
    {
        private AcademyGroup Group;

        public GroupViewModel(AcademyGroup group)
        {
            Group = group;
        }

        public string Name
        {
            get { return Group.Name!; }
            set
            {
                Group.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }
}
