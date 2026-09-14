using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using GymStats.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;

namespace GymStats.ViewModels
{
    public partial class AnthropometryViewModel : ObservableObject
    {
        private readonly AppDbContext _dbContext;

        [ObservableProperty]
        private ObservableCollection<BodyMeasurement> _measurements = new();

        [ObservableProperty]
        private bool _isListEmpty = true;

        public AnthropometryViewModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task LoadMeasurementsAsync()
        {
            await _dbContext.Database.EnsureCreatedAsync();

            var list = await _dbContext.BodyMeasurements
                .OrderByDescending(m => m.Date)
                .ToListAsync();

            // Пересоздаем коллекцию, чтобы UI гарантированно обновился
            Measurements = new ObservableCollection<BodyMeasurement>(list);
            IsListEmpty = Measurements.Count == 0;
        }

        [RelayCommand]
        private async Task GoToAddMeasurementAsync()
        {
            await Shell.Current.GoToAsync("AddMeasurementPage");
        }

        [RelayCommand]
        private async Task DeleteMeasurementAsync(BodyMeasurement measurement)
        {
            _dbContext.BodyMeasurements.Remove(measurement);
            await _dbContext.SaveChangesAsync();
            await LoadMeasurementsAsync();
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}