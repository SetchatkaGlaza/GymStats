using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using GymStats.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace GymStats.ViewModels;

[QueryProperty(nameof(WorkoutId), "workoutId")]
public partial class AddExerciseToWorkoutViewModel : ObservableObject
{
    private readonly AppDbContext _db;
    [ObservableProperty] private int _workoutId;
    [ObservableProperty] private ObservableCollection<WorkoutExercise> _workoutExercises = new();
    [ObservableProperty] private bool _isListEmpty = true;
    [ObservableProperty] private string _workoutName = "Тренировка";

    public AddExerciseToWorkoutViewModel(AppDbContext db) => _db = db;

    public async Task LoadExercisesAsync()
    {
        await _db.EnsureReadyAsync();
        var workout = await _db.Workouts.AsNoTracking().FirstOrDefaultAsync(x => x.Id == WorkoutId);
        if (workout is null)
        {
            await Shell.Current.GoToAsync("//MainPage");
            return;
        }

        WorkoutName = workout.Name;
        WorkoutExercises = new ObservableCollection<WorkoutExercise>(
            await _db.WorkoutExercises.Where(x => x.WorkoutId == WorkoutId)
                .Include(x => x.Exercise).OrderBy(x => x.Order).ToListAsync());
        IsListEmpty = WorkoutExercises.Count == 0;
    }

    [RelayCommand] private Task GoToAddExerciseAsync() => Shell.Current.GoToAsync($"SelectExercisePage?workoutId={WorkoutId}");
    [RelayCommand] private Task GoToFinishWorkoutAsync() => Shell.Current.GoToAsync($"FinishWorkoutPage?workoutId={WorkoutId}");
    [RelayCommand] private Task GoBackAsync() => Shell.Current.GoToAsync("..");

    [RelayCommand]
    private async Task DeleteWorkoutAsync()
    {
        var workout = await _db.Workouts.FindAsync(WorkoutId);
        if (workout is null) return;

        var workoutExercises = await _db.WorkoutExercises.Where(x => x.WorkoutId == WorkoutId).ToListAsync();
        var ids = workoutExercises.Select(x => x.Id).ToList();
        var sets = await _db.Sets.Where(x => ids.Contains(x.WorkoutExerciseId)).ToListAsync();

        _db.Sets.RemoveRange(sets);
        _db.WorkoutExercises.RemoveRange(workoutExercises);
        _db.Workouts.Remove(workout);
        await _db.SaveChangesAsync();
        await Shell.Current.GoToAsync("//MainPage");
    }

    [RelayCommand]
    private Task OpenExerciseDetailAsync(WorkoutExercise workoutExercise) =>
        Shell.Current.GoToAsync($"ExerciseDetailPage?workoutExerciseId={workoutExercise.Id}");
}