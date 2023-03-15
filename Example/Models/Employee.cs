using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace Example.Models;

public partial class Employee : ObservableObject
{
    [ObservableProperty] private string _firstName;
    [ObservableProperty] private string _lastName;
    [ObservableProperty] private string _email;
    [ObservableProperty] private DateTime _birthday;
    [ObservableProperty] private DateTime _postedOn;
}
