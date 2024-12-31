using AcademyGroupMVVM.Models;
using AcademyGroupMVVM.ViewModels;
using System.Data;
using System.Windows;

namespace AcademyGroupMVVM
{
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            try
            {
                using (var db = new AcademyGroupContext())
                {
                    var groups = from g in db.AcademyGroups
                                select g;
                    var students = from st in db.Students
                                 select st;
                    MainWindow view = new MainWindow();
                    MainViewModel viewModel = new MainViewModel(groups, students);
                    view.DataContext = viewModel;
                    view.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
