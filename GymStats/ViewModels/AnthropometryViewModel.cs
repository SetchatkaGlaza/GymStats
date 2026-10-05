using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using GymStats.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace GymStats.ViewModels;

public partial class AnthropometryViewModel : ObservableObject
{
    private readonly AppDbContext _db;
    [ObservableProperty] private ObservableCollection<BodyMeasurement> _measurements = new();
    [ObservableProperty] private bool _isListEmpty = true;

    public AnthropometryViewModel(AppDbContext db) => _db = db;

    public async Task LoadMeasurementsAsync()
    {
        await _db.EnsureReadyAsync();
        Measurements = new ObservableCollection<BodyMeasurement>(
            await _db.BodyMeasurements.AsNoTracking().OrderByDescending(x => x.Date).ToListAsync());
        IsListEmpty = Measurements.Count == 0;
    }

    [RelayCommand]
    private async Task GoToAddMeasurementAsync() => await Shell.Current.GoToAsync("AddMeasurementPage");

    [RelayCommand]
    private async Task DeleteMeasurementAsync(BodyMeasurement measurement)
    {
        if (!await Shell.Current.DisplayAlert("Удалить замер?", $"Запись от {measurement.Date:dd.MM.yyyy} будет удалена.", "Удалить", "Отмена"))
            return;

        _db.BodyMeasurements.Remove(measurement);
        await _db.SaveChangesAsync();
        await LoadMeasurementsAsync();
    }

    [RelayCommand] private Task GoBackAsync() => Shell.Current.GoToAsync("..");
}