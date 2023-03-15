using CommunityToolkit.Mvvm.ComponentModel;
using Example.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;

namespace Example.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    public MainWindowViewModel()
    {
        Employees = new ObservableCollection<Employee>
        {
            new Employee
            {
                FirstName = "Max",
                LastName = "Mustermann",
                Email = "max@mustermann.de",
                Birthday = DateTime.Now.AddYears(-18),
                PostedOn = DateTime.Now
            },
            new Employee
            {
                FirstName = "Tom",
                LastName = "Thomas",
                Email = "tom@thomas.de",
                Birthday = DateTime.Now.AddYears(-32).AddDays(2),
                PostedOn = DateTime.Now.AddDays(2)
            },
            new Employee
            {
                FirstName = "Julia",
                LastName = "Jules",
                Email = "julia@jules.de",
                Birthday = DateTime.Now.AddYears(-20).AddDays(5),
                PostedOn = DateTime.Now.AddDays(5)
            }
        };

        DataGridViewSource.Filter += Filter;
    }

    private void Filter(object sender, FilterEventArgs e)
    {
        var accepted = true;

        var employee = (Employee)e.Item;

        if (SelectedDates != null && SelectedDates.Any())
        {
            if (SelectedDates.Count == 1)
            {
                accepted &= employee.PostedOn == SelectedDates.First().Date;
            }
            else
            {
                accepted &= employee.PostedOn >= SelectedDates.First().Date && employee.PostedOn <= SelectedDates.Last().Date;
            }
        }

        e.Accepted = accepted;
    }

    public CollectionViewSource DataGridViewSource { get; set; } = new CollectionViewSource();

    [ObservableProperty] private ObservableCollection<DateTime> _selectedDates;
    partial void OnSelectedDatesChanged(ObservableCollection<DateTime> value) => DataGridViewSource.View.Refresh();

    [ObservableProperty]
    private ObservableCollection<Employee> _employees = new ObservableCollection<Employee>();
    partial void OnEmployeesChanged(ObservableCollection<Employee> value) => DataGridViewSource.Source = value;
}
