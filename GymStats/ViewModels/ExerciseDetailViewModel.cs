using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using GymStats.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace GymStats.ViewModels;

[QueryProperty(nameof(WorkoutExerciseId), "workoutExerciseId")]
public partial class ExerciseDetailViewModel : ObservableObject
{
    private readonly AppDbContext _db;
    [ObservableProperty] private int _workoutExerciseId;
    [ObservableProperty] private string _exerciseName = string.Empty;
    [ObservableProperty] private string _targetSummary = string.Empty;
    [ObservableProperty] private ObservableCollection<Set> _sets = new();
    [ObservableProperty] private bool _isListEmpty = true;

    public ExerciseDetailViewModel(AppDbContext db) => _db = db;

    public async Task LoadDataAsync()
    {
        await _db.EnsureReadyAsync();
        var item = await _db.WorkoutExercises.Include(x => x.Exercise).FirstOrDefaultAsync(x => x.Id == WorkoutExerciseId);
        if (item is null) return;

        ExerciseName = item.Exercise.Name;
        TargetSummary = item.TargetSets > 0
            ? $"Цель: {item.TargetSets} × {item.TargetReps} • {item.TargetWeight:0.#} кг • RPE {item.TargetRPE}"
            : "Цель не задана";

        Sets = new ObservableCollection<Set>(await _db.Sets.Where(x => x.WorkoutExerciseId == WorkoutExerciseId).OrderBy(x => x.Order).ToListAsync());
        IsListEmpty = Sets.Count == 0;
    }

    [RelayCommand] private Task GoToAddSetAsync() => Shell.Current.GoToAsync($"AddSetPage?workoutExerciseId={WorkoutExerciseId}");
    [RelayCommand] private Task GoBackAsync() => Shell.Current.GoToAsync("..");

    [RelayCommand]
    private async Task DeleteSetAsync(Set set)
    {
        _db.Sets.Remove(set);
        await _db.SaveChangesAsync();
        await LoadDataAsync();
    }
}