using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using Microsoft.EntityFrameworkCore;

namespace GymStats.ViewModels;

[QueryProperty(nameof(WorkoutId), "workoutId")]
public partial class FinishWorkoutViewModel : ObservableObject
{
    private readonly AppDbContext _db;
    [ObservableProperty] private int _workoutId;
    [ObservableProperty] private string _workoutName = "Тренировка";
    [ObservableProperty] private string _notes = string.Empty;
    [ObservableProperty] private string _statusMessage = string.Empty;
    [ObservableProperty] private bool _isStatusVisible;
    [ObservableProperty] private bool _hasExercises;
    [ObservableProperty] private bool _hasSets;

    public FinishWorkoutViewModel(AppDbContext db) => _db = db;

    public async Task LoadWorkoutDataAsync()
    {
        var workout = await _db.Workouts.FindAsync(WorkoutId);
        if (workout is null)
        {
            await Shell.Current.GoToAsync("//MainPage");
            return;
        }
        WorkoutName = workout.Name;
        Notes = workout.Notes;
        HasExercises = await _db.WorkoutExercises.AnyAsync(x => x.WorkoutId == WorkoutId);
        HasSets = await (
            from s in _db.Sets
            join we in _db.WorkoutExercises on s.WorkoutExerciseId equals we.Id
            where we.WorkoutId == WorkoutId
            select s.Id).AnyAsync();
    }

    [RelayCommand]
    private async Task SaveAndFinishAsync()
    {
        if (!HasExercises)
        {
            StatusMessage = "Добавь хотя бы одно упражнение перед завершением.";
            IsStatusVisible = true;
            return;
        }

        if (!HasSets)
        {
            StatusMessage = "Добавь хотя бы один подход перед завершением.";
            IsStatusVisible = true;
            return;
        }

        var workout = await _db.Workouts.FindAsync(WorkoutId);
        if (workout is null) return;

        workout.Name = string.IsNullOrWhiteSpace(WorkoutName) ? "Тренировка" : WorkoutName.Trim();
        workout.Notes = Notes.Trim();
        await _db.SaveChangesAsync();
        await Shell.Current.GoToAsync("//MainPage");
    }

    [RelayCommand] private Task GoBackAsync() => Shell.Current.GoToAsync("..");
}