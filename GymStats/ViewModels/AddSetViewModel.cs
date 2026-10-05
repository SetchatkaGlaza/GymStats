using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using GymStats.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace GymStats.ViewModels;

[QueryProperty(nameof(WorkoutExerciseId), "workoutExerciseId")]
public partial class AddSetViewModel : ObservableObject
{
    private readonly AppDbContext _db;
    [ObservableProperty] private int _workoutExerciseId;
    [ObservableProperty] private string _weight = string.Empty;
    [ObservableProperty] private string _reps = "8";
    [ObservableProperty] private string _rpe = "8";
    [ObservableProperty] private string _tempo = string.Empty;
    [ObservableProperty] private string _previousPerformance = string.Empty;
    [ObservableProperty] private bool _hasPreviousPerformance;
    [ObservableProperty] private string _statusMessage = string.Empty;
    [ObservableProperty] private bool _isStatusVisible;

    public AddSetViewModel(AppDbContext db) => _db = db;

    partial void OnWorkoutExerciseIdChanged(int value) => _ = LoadPreviousPerformanceAsync();

    private async Task LoadPreviousPerformanceAsync()
    {
        var current = await _db.WorkoutExercises.Include(x => x.Exercise).FirstOrDefaultAsync(x => x.Id == WorkoutExerciseId);
        if (current is null) return;

        var previous = await (
            from s in _db.Sets
            join we in _db.WorkoutExercises on s.WorkoutExerciseId equals we.Id
            join w in _db.Workouts on we.WorkoutId equals w.Id
            where we.ExerciseId == current.ExerciseId && we.WorkoutId != current.WorkoutId
            orderby w.Date descending, s.Order descending
            select new { w.Date, s.Weight, s.Reps, s.RPE }
        ).FirstOrDefaultAsync();

        if (previous is null)
        {
            HasPreviousPerformance = false;
            return;
        }

        PreviousPerformance = $"Последний раз: {previous.Weight:0.#} кг × {previous.Reps} • RPE {previous.RPE} ({previous.Date:dd.MM.yyyy})";
        HasPreviousPerformance = true;
        if (string.IsNullOrWhiteSpace(Weight) || Weight == "0")
            Weight = previous.Weight.ToString("0.##", CultureInfo.CurrentCulture);
    }

    [RelayCommand]
    private async Task SaveSetAsync()
    {
        if (!double.TryParse(Weight.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out var weight) || weight < 0)
        {
            StatusMessage = "Вес должен быть числом не меньше 0.";
            IsStatusVisible = true;
            return;
        }
        if (!int.TryParse(Reps, out var reps) || reps <= 0)
        {
            StatusMessage = "Количество повторений должно быть больше 0.";
            IsStatusVisible = true;
            return;
        }
        if (!int.TryParse(Rpe, out var rpe) || rpe is < 1 or > 10)
        {
            StatusMessage = "RPE должен быть от 1 до 10.";
            IsStatusVisible = true;
            return;
        }

        var maxOrder = await _db.Sets.Where(s => s.WorkoutExerciseId == WorkoutExerciseId).Select(s => (int?)s.Order).MaxAsync() ?? 0;
        _db.Sets.Add(new Set { WorkoutExerciseId = WorkoutExerciseId, Weight = weight, Reps = reps, RPE = rpe, Tempo = Tempo.Trim(), Order = maxOrder + 1 });
        await _db.SaveChangesAsync();
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand] private Task CancelAsync() => Shell.Current.GoToAsync("..");
}