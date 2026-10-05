using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using GymStats.Models;
using GymStats.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace GymStats.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    private readonly AppDbContext _db;
    private readonly AnalyticsService _analytics;

    [ObservableProperty] private ObservableCollection<Workout> _workouts = new();
    [ObservableProperty] private bool _isListEmpty = true;
    [ObservableProperty] private int _workoutCount;
    [ObservableProperty] private double _totalVolume;
    [ObservableProperty] private DateTime? _lastWorkoutDate;

    public MainPageViewModel(AppDbContext db, AnalyticsService analytics)
    {
        _db = db;
        _analytics = analytics;
    }

    public async Task LoadAsync()
    {
        await _db.EnsureReadyAsync();
        var list = await _db.Workouts.AsNoTracking().OrderByDescending(w => w.Date).Take(8).ToListAsync();
        Workouts = new ObservableCollection<Workout>(list);
        IsListEmpty = Workouts.Count == 0;
        WorkoutCount = await _analytics.GetWorkoutCountAsync();
        TotalVolume = await _analytics.GetTotalVolumeAsync();
        LastWorkoutDate = Workouts.FirstOrDefault()?.Date;
    }

    [RelayCommand] private Task GoToAddWorkoutAsync() => Shell.Current.GoToAsync("AddWorkoutPage");
    [RelayCommand] private Task OpenWorkoutAsync(Workout workout) => Shell.Current.GoToAsync($"AddExerciseToWorkoutPage?workoutId={workout.Id}");
    [RelayCommand] private Task GoToAnalyticsAsync() => Shell.Current.GoToAsync("//AnalyticsPage");
    [RelayCommand] private Task GoToMediaAsync() => Shell.Current.GoToAsync("//MediaPage");
    [RelayCommand] private Task GoToAnthropometryAsync() => Shell.Current.GoToAsync("//AnthropometryPage");
    [RelayCommand] private Task GoToTemplatesAsync() => Shell.Current.GoToAsync("//TemplateListPage");
    [RelayCommand] private Task GoToBackupAsync() => Shell.Current.GoToAsync("BackupPage");
}