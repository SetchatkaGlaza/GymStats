using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using GymStats.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace GymStats.ViewModels
{
    [QueryProperty(nameof(TemplateId), "templateId")]
    public partial class SelectExerciseForTemplateViewModel : ObservableObject
    {
        private readonly AppDbContext _dbContext;

        [ObservableProperty]
        private int _templateId;

        [ObservableProperty]
        private ObservableCollection<Exercise> _exercises = new();

        [ObservableProperty]
        private ObservableCollection<string> _muscleGroups = new();

        [ObservableProperty]
        private string _selectedMuscleGroup;

        public SelectExerciseForTemplateViewModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task LoadExercisesAsync()
        {
            await _dbContext.SeedDataAsync();

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
            // Проверяем, нет ли уже этого упражнения в шаблоне
            var exists = await _dbContext.TemplateExercises
                .AnyAsync(te => te.TemplateId == TemplateId && te.ExerciseId == exercise.Id);

            if (exists)
            {
                // Можно показать уведомление, но пока просто выходим
                await Shell.Current.GoToAsync("..");
                return;
            }

            var maxOrder = await _dbContext.TemplateExercises
                .Where(te => te.TemplateId == TemplateId)
                .Select(te => (int?)te.Order)
                .MaxAsync() ?? 0;

            var templateExercise = new TemplateExercise
            {
                TemplateId = TemplateId,
                ExerciseId = exercise.Id,
                Order = maxOrder + 1
            };

            await _dbContext.TemplateExercises.AddAsync(templateExercise);
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