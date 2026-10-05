using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using GymStats.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace GymStats.ViewModels;

[QueryProperty(nameof(WorkoutId), "workoutId")]
public partial class SelectExerciseViewModel : ObservableObject
{
    private readonly AppDbContext _db;
    [ObservableProperty] private int _workoutId;
    [ObservableProperty] private ObservableCollection<Exercise> _exercises = new();
    [ObservableProperty] private ObservableCollection<string> _muscleGroups = new();
    [ObservableProperty] private ObservableCollection<string> _equipmentOptions = new();
    [ObservableProperty] private ObservableCollection<string> _movementOptions = new();
    [ObservableProperty] private string _selectedMuscleGroup = "Все";
    [ObservableProperty] private string _selectedEquipment = "Все";
    [ObservableProperty] private string _selectedMovement = "Все";
    [ObservableProperty] private string _searchText = string.Empty;

    public SelectExerciseViewModel(AppDbContext db) => _db = db;

    public async Task LoadExercisesAsync()
    {
        await _db.EnsureReadyAsync();
        MuscleGroups = new ObservableCollection<string>(new[] { "Все" }.Concat(await _db.Exercises.Select(x => x.MuscleGroup).Distinct().OrderBy(x => x).ToListAsync()));
        EquipmentOptions = new ObservableCollection<string>(new[] { "Все" }.Concat(await _db.Exercises.Select(x => x.Equipment).Distinct().OrderBy(x => x).ToListAsync()));
        MovementOptions = new ObservableCollection<string>(new[] { "Все" }.Concat(await _db.Exercises.Select(x => x.MovementType).Distinct().OrderBy(x => x).ToListAsync()));
        await FilterExercisesAsync();
    }

    partial void OnSelectedMuscleGroupChanged(string value) => _ = FilterExercisesAsync();
    partial void OnSelectedEquipmentChanged(string value) => _ = FilterExercisesAsync();
    partial void OnSelectedMovementChanged(string value) => _ = FilterExercisesAsync();
    partial void OnSearchTextChanged(string value) => _ = FilterExercisesAsync();

    private async Task FilterExercisesAsync()
    {
        var query = _db.Exercises.AsNoTracking().AsQueryable();
        if (SelectedMuscleGroup != "Все") query = query.Where(x => x.MuscleGroup == SelectedMuscleGroup);
        if (SelectedEquipment != "Все") query = query.Where(x => x.Equipment == SelectedEquipment);
        if (SelectedMovement != "Все") query = query.Where(x => x.MovementType == SelectedMovement);
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var term = SearchText.Trim().ToLower();
            query = query.Where(x => x.Name.ToLower().Contains(term) || x.MuscleGroup.ToLower().Contains(term) || x.Equipment.ToLower().Contains(term));
        }
        Exercises = new ObservableCollection<Exercise>(await query.OrderBy(x => x.MuscleGroup).ThenBy(x => x.Name).ToListAsync());
    }

    [RelayCommand]
    private async Task SelectExerciseAsync(Exercise exercise)
    {
        var exists = await _db.WorkoutExercises.AnyAsync(x => x.WorkoutId == WorkoutId && x.ExerciseId == exercise.Id);
        if (exists) return;
        var maxOrder = await _db.WorkoutExercises.Where(x => x.WorkoutId == WorkoutId).Select(x => (int?)x.Order).MaxAsync() ?? 0;
        _db.WorkoutExercises.Add(new WorkoutExercise { WorkoutId = WorkoutId, ExerciseId = exercise.Id, Order = maxOrder + 1 });
        await _db.SaveChangesAsync();
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand] private Task GoToAddExerciseAsync() => Shell.Current.GoToAsync("AddExercisePage");
    [RelayCommand] private Task GoBackAsync() => Shell.Current.GoToAsync("..");
}