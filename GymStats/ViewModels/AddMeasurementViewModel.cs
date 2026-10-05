using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using GymStats.Models;
using System.Globalization;

namespace GymStats.ViewModels;

public partial class AddMeasurementViewModel : ObservableObject
{
    private readonly AppDbContext _db;
    [ObservableProperty] private DateTime _measurementDate = DateTime.Today;
    [ObservableProperty] private string _weight = string.Empty;
    [ObservableProperty] private string _chest = string.Empty;
    [ObservableProperty] private string _waist = string.Empty;
    [ObservableProperty] private string _hips = string.Empty;
    [ObservableProperty] private string _biceps = string.Empty;
    [ObservableProperty] private string _thigh = string.Empty;
    [ObservableProperty] private string _calf = string.Empty;
    [ObservableProperty] private string _neck = string.Empty;
    [ObservableProperty] private string _statusMessage = string.Empty;
    [ObservableProperty] private bool _isStatusVisible;

    public AddMeasurementViewModel(AppDbContext db) => _db = db;

    private static bool TryNumber(string value, out double result) =>
        double.TryParse(value.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out result);

    private static double Optional(string value) => TryNumber(value, out var result) && result >= 0 ? result : 0;

    [RelayCommand]
    private async Task SaveMeasurementAsync()
    {
        if (!TryNumber(Weight, out var weight) || weight <= 0)
        {
            StatusMessage = "Вес должен быть указан и быть больше 0.";
            IsStatusVisible = true;
            return;
        }

        _db.BodyMeasurements.Add(new BodyMeasurement
        {
            Date = MeasurementDate,
            Weight = weight,
            Chest = Optional(Chest), Waist = Optional(Waist), Hips = Optional(Hips),
            Biceps = Optional(Biceps), Thigh = Optional(Thigh), Calf = Optional(Calf), Neck = Optional(Neck)
        });
        await _db.SaveChangesAsync();
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand] private Task CancelAsync() => Shell.Current.GoToAsync("..");
}