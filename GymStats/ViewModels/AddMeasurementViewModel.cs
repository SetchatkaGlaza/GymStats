using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using GymStats.Models;

namespace GymStats.ViewModels
{
    public partial class AddMeasurementViewModel : ObservableObject
    {
        private readonly AppDbContext _dbContext;

        [ObservableProperty] private DateTime _measurementDate = DateTime.Today;
        [ObservableProperty] private string _weight = "0";
        [ObservableProperty] private string _chest = "0";
        [ObservableProperty] private string _waist = "0";
        [ObservableProperty] private string _hips = "0";
        [ObservableProperty] private string _biceps = "0";
        [ObservableProperty] private string _thigh = "0";
        [ObservableProperty] private string _calf = "0";
        [ObservableProperty] private string _neck = "0";
        [ObservableProperty] private string _statusMessage = string.Empty;
        [ObservableProperty] private bool _isStatusVisible = false;

        public AddMeasurementViewModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [RelayCommand]
        private async Task SaveMeasurementAsync()
        {
            var newMeasurement = new BodyMeasurement
            {
                Date = MeasurementDate,
                Weight = double.TryParse(Weight, out double w) ? w : 0,
                Chest = double.TryParse(Chest, out double c) ? c : 0,
                Waist = double.TryParse(Waist, out double wa) ? wa : 0,
                Hips = double.TryParse(Hips, out double h) ? h : 0,
                Biceps = double.TryParse(Biceps, out double b) ? b : 0,
                Thigh = double.TryParse(Thigh, out double th) ? th : 0,
                Calf = double.TryParse(Calf, out double ca) ? ca : 0,
                Neck = double.TryParse(Neck, out double n) ? n : 0
            };

            await _dbContext.BodyMeasurements.AddAsync(newMeasurement);
            await _dbContext.SaveChangesAsync();

            StatusMessage = "Замеры сохранены!";
            IsStatusVisible = true;

            await Task.Delay(1000);
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}