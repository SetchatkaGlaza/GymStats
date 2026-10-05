using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using GymStats.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace GymStats.ViewModels;

public partial class AddWorkoutViewModel : ObservableObject
{
    private readonly AppDbContext _db;
    [ObservableProperty] private DateTime _workoutDate = DateTime.Today;
    [ObservableProperty] private string _workoutName = "Тренировка";
    [ObservableProperty] private ObservableCollection<WorkoutTemplate> _templates = new();
    [ObservableProperty] private WorkoutTemplate? _selectedTemplate;

    public AddWorkoutViewModel(AppDbContext db) => _db = db;

    public async Task LoadTemplatesAsync()
    {
        await _db.EnsureReadyAsync();
        Templates = new ObservableCollection<WorkoutTemplate>(await _db.WorkoutTemplates.OrderBy(x => x.Name).ToListAsync());
        SelectedTemplate = null;
    }

    [RelayCommand]
    private async Task SaveWorkoutAsync()
    {
        var workout = new Workout
        {
            Date = WorkoutDate,
            Name = string.IsNullOrWhiteSpace(WorkoutName) ? "Тренировка" : WorkoutName.Trim(),
            TemplateId = SelectedTemplate?.Id
        };

        _db.Workouts.Add(workout);
        await _db.SaveChangesAsync();

        if (SelectedTemplate is not null)
        {
            var templateExercises = await _db.TemplateExercises.Where(x => x.TemplateId == SelectedTemplate.Id).OrderBy(x => x.Order).ToListAsync();
            foreach (var item in templateExercises)
            {
                _db.WorkoutExercises.Add(new WorkoutExercise
                {
                    WorkoutId = workout.Id, ExerciseId = item.ExerciseId, Order = item.Order,
                    TargetSets = item.TargetSets, TargetReps = item.TargetReps,
                    TargetWeight = item.TargetWeight, TargetRPE = item.TargetRPE
                });
            }
            await _db.SaveChangesAsync();
        }

        await Shell.Current.GoToAsync($"AddExerciseToWorkoutPage?workoutId={workout.Id}");
    }

    [RelayCommand] private Task CancelAsync() => Shell.Current.GoToAsync("..");
}