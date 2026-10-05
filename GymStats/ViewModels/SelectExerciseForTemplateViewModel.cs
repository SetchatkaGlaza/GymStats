using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using GymStats.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace GymStats.ViewModels;

[QueryProperty(nameof(TemplateId), "templateId")]
public partial class SelectExerciseForTemplateViewModel : ObservableObject
{
    private readonly AppDbContext _db;
    [ObservableProperty] private int _templateId;
    [ObservableProperty] private ObservableCollection<Exercise> _exercises = new();
    [ObservableProperty] private ObservableCollection<string> _muscleGroups = new();
    [ObservableProperty] private ObservableCollection<string> _equipmentOptions = new();
    [ObservableProperty] private ObservableCollection<string> _movementOptions = new();
    [ObservableProperty] private string _selectedMuscleGroup = "Все";
    [ObservableProperty] private string _selectedEquipment = "Все";
    [ObservableProperty] private string _selectedMovement = "Все";
    [ObservableProperty] private string _searchText = string.Empty;

    public SelectExerciseForTemplateViewModel(AppDbContext db) => _db = db;

    public async Task LoadExercisesAsync()
    {
        await _db.EnsureReadyAsync();
        MuscleGroups = new ObservableCollection<string>(new[] { "Все" }.Concat(await _db.Exercises.Select(x => x.MuscleGroup).Distinct().OrderBy(x => x).ToListAsync()));
        EquipmentOptions = new ObservableCollection<string>(new[] { "Все" }.Concat(await _db.Exercises.Select(x => x.Equipment).Distinct().OrderBy(x => x).ToListAsync()));
        MovementOptions = new ObservableCollection<string>(new[] { "Все" }.Concat(await _db.Exercises.Select(x => x.MovementType).Distinct().OrderBy(x => x).ToListAsync()));
        await FilterAsync();
    }

    partial void OnSelectedMuscleGroupChanged(string value) => _ = FilterAsync();
    partial void OnSelectedEquipmentChanged(string value) => _ = FilterAsync();
    partial void OnSelectedMovementChanged(string value) => _ = FilterAsync();
    partial void OnSearchTextChanged(string value) => _ = FilterAsync();

    private async Task FilterAsync()
    {
        var query = _db.Exercises.AsNoTracking().AsQueryable();
        if (SelectedMuscleGroup != "Все") query = query.Where(x => x.MuscleGroup == SelectedMuscleGroup);
        if (SelectedEquipment != "Все") query = query.Where(x => x.Equipment == SelectedEquipment);
        if (SelectedMovement != "Все") query = query.Where(x => x.MovementType == SelectedMovement);
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var term = SearchText.Trim().ToLower();
            query = query.Where(x => x.Name.ToLower().Contains(term) || x.MuscleGroup.ToLower().Contains(term));
        }
        Exercises = new ObservableCollection<Exercise>(await query.OrderBy(x => x.MuscleGroup).ThenBy(x => x.Name).ToListAsync());
    }

    [RelayCommand]
    private async Task SelectExerciseAsync(Exercise exercise)
    {
        if (await _db.TemplateExercises.AnyAsync(x => x.TemplateId == TemplateId && x.ExerciseId == exercise.Id))
        {
            await Shell.Current.GoToAsync("..");
            return;
        }

        var maxOrder = await _db.TemplateExercises.Where(x => x.TemplateId == TemplateId).Select(x => (int?)x.Order).MaxAsync() ?? 0;
        _db.TemplateExercises.Add(new TemplateExercise { TemplateId = TemplateId, ExerciseId = exercise.Id, Order = maxOrder + 1 });
        await _db.SaveChangesAsync();
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand] private Task GoBackAsync() => Shell.Current.GoToAsync("..");
}