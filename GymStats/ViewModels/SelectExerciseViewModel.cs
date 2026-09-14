using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using GymStats.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace GymStats.ViewModels
{
    [QueryProperty(nameof(WorkoutId), "workoutId")]
    public partial class SelectExerciseViewModel : ObservableObject
    {
        private readonly AppDbContext _dbContext;

        [ObservableProperty]
        private int _workoutId;

        [ObservableProperty]
        private ObservableCollection<Exercise> _exercises = new();

        [ObservableProperty]
        private ObservableCollection<string> _muscleGroups = new();

        [ObservableProperty]
        private string _selectedMuscleGroup;

        public SelectExerciseViewModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task LoadExercisesAsync()
        {
            await _dbContext.SeedDataAsync(); // Гарантируем наличие данных

            var groups = await _dbContext.Exercises
                .Select(e => e.MuscleGroup)
                .Distinct()
                .OrderBy(g => g)
                .ToListAsync();

            MuscleGroups.Clear();
            MuscleGroups.Add("Все");
            foreach (var group in groups)
            {
                MuscleGroups.Add(group);
            }

            SelectedMuscleGroup = "Все";
            FilterExercises();
        }

        partial void OnSelectedMuscleGroupChanged(string value)
        {
            FilterExercises();
        }

        private async void FilterExercises()
        {
            if (string.IsNullOrEmpty(SelectedMuscleGroup) || SelectedMuscleGroup == "Все")
            {
                var allExercises = await _dbContext.Exercises
                    .OrderBy(e => e.MuscleGroup)
                    .ThenBy(e => e.Name)
                    .ToListAsync();

                Exercises.Clear();
                foreach (var exercise in allExercises) Exercises.Add(exercise);
            }
            else
            {
                var filtered = await _dbContext.Exercises
                    .Where(e => e.MuscleGroup == SelectedMuscleGroup)
                    .OrderBy(e => e.Name)
                    .ToListAsync();

                Exercises.Clear();
                foreach (var exercise in filtered) Exercises.Add(exercise);
            }
        }

        [RelayCommand]
        private async Task SelectExerciseAsync(Exercise exercise)
        {
            var maxOrder = await _dbContext.WorkoutExercises
                .Where(we => we.WorkoutId == WorkoutId)
                .Select(we => (int?)we.Order)
                .MaxAsync() ?? 0;

            var workoutExercise = new WorkoutExercise
            {
                WorkoutId = WorkoutId,
                ExerciseId = exercise.Id,
                Order = maxOrder + 1
            };

            await _dbContext.WorkoutExercises.AddAsync(workoutExercise);
            await _dbContext.SaveChangesAsync();

            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}