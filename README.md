# XamlDateRangePicker
A simple WPF DateRangePicker, that allows to select a date range from a calendar control.

## Motivation
I needed a way to let a user select a date range for a DataGrid filter. After some research I was not able to find a control that was capable of what I wanted, so I created this WPF User Control to have a combination of the WPF DatePicker and the WPF Calendar controls, that allows either selecting a single date or a date range.

## Technologies used
- C#
- .NET 6
- WPF (XAML)

## How to use
The DateRangePicker User Control can be used as follows.

1. Add the project or the user control to your solution/project.
2. Add the namespace for the user control to your XAML

```xaml
xmlns:userControls="clr-namespace:UserControls;assembly=DateRangePicker"
```

3. Use the control in your XAML

```xaml
<userControls:DateRangePicker 
    SelectedDateRange="{Binding SelectedDates, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}" />
```

You can bind the SelectedDateRange Property to your ViewModel. This property holds a `ObservableCollection<DateTime>` of either one or two dates. Either the single selected DateTime or the first and the last date of the selected date range.
