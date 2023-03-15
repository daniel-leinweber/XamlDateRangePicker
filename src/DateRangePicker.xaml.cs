using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace UserControls;

public partial class DateRangePicker : UserControl, INotifyPropertyChanged
{
    public DateRangePicker()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty SelectedDateRangeProperty
        = DependencyProperty.Register(
            nameof(SelectedDateRange),
            typeof(ObservableCollection<DateTime>),
            typeof(DateRangePicker),
            new FrameworkPropertyMetadata(default(ObservableCollection<DateTime>), FrameworkPropertyMetadataOptions.None));

    public ObservableCollection<DateTime> SelectedDateRange
    {
        get => (ObservableCollection<DateTime>)GetValue(SelectedDateRangeProperty);
        set => SetValue(SelectedDateRangeProperty, value);
    }

    #region UserControl Properties

    public string DisplayValue
    {
        get => _displayValue;
        set => SetProperty(ref _displayValue, value, onChanged: () =>
        {
            if (string.IsNullOrWhiteSpace(value) == true)
            {
                DateRangePicker_Calendar?.SelectedDates.Clear();
            }
            else
            {
                var dates = value.Trim().Replace(" ", "").Split('-');
                if (dates.Count() == 2)
                {
                    var isFirstDateValid = DateTime.TryParse(dates.First(), out var firstDate) && dates.First().Length == 10;
                    var isLastDateValid = DateTime.TryParse(dates.Last(), out var lastDate) && dates.Last().Length == 10;

                    if (isFirstDateValid && isLastDateValid)
                    {
                        DateRangePicker_Calendar?.SelectedDates.AddRange(firstDate, lastDate);
                    }
                }
                else if (dates.Count() == 1)
                {
                    var isValidDate = DateTime.TryParse(dates.First(), out var date) && dates.First().Length == 10;
                    if (isValidDate)
                    {
                        DateRangePicker_Calendar?.SelectedDates.Add(date);
                    }
                }
            }
        });
    }
    private string _displayValue;

    public bool IsDropDownOpen
    {
        get => _isDropDownOpen;
        set => SetProperty(ref _isDropDownOpen, value);
    }
    private bool _isDropDownOpen;

    public ObservableCollection<DateTime> SelectedDates
    {
        get => _selectedDate;
        set => SetProperty(ref _selectedDate, value, onChanged: () =>
        {
            IsDropDownOpen = false;
            if (value.Any())
            {
                if (value.Count == 1)
                {
                    DisplayValue = value.First().ToString("dd.MM.yyyy");
                    SelectedDateRange = new ObservableCollection<DateTime> { value.First() };
                }
                else
                {
                    DisplayValue = $"{value.First():dd.MM.yyyy} - {value.Last():dd.MM.yyyy}";
                    SelectedDateRange = new ObservableCollection<DateTime> { value.First(), value.Last() };
                }
            }
            else
            {
                SelectedDateRange = new ObservableCollection<DateTime>();
            }
        });
    }
    private ObservableCollection<DateTime> _selectedDate = new ObservableCollection<DateTime>();

    #endregion

    #region UI Events

    private void Handle_Escape_Button(object sender, System.Windows.Input.KeyEventArgs e)
    {
        var textBox = sender as TextBox;
        if (e.Key == System.Windows.Input.Key.Escape && textBox != null)
        {
            textBox.Text = string.Empty;
        }
    }

    private void DateRangePicker_Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
    {
        var calendar = sender as Calendar;
        if (calendar == null)
        {
            return;
        }

        SelectedDates = new ObservableCollection<DateTime>(calendar.SelectedDates);
    }

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;

    public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
    {
#if DEBUG
        Debug.Print($"RaisePropertyChanged for {propertyName}");
#endif
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null)
    {
        var output = false;

        if (EqualityComparer<T>.Default.Equals(backingStore, value) == false)
        {
            backingStore = value;
            onChanged?.Invoke();
            RaisePropertyChanged(propertyName);

            output = true;
        }

        return output;
    }

    #endregion        
}
