using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using GymStats.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace GymStats.ViewModels
{
    public partial class AddWorkoutViewModel : ObservableObject
    {
        private readonly AppDbContext _dbContext;

        [ObservableProperty]
        private DateTime _workoutDate = DateTime.Today;

        [ObservableProperty]
        private string _workoutName = "Тренировка";

        [ObservableProperty]
        private ObservableCollection<WorkoutTemplate> _templates = new();

        [ObservableProperty]
        private WorkoutTemplate _selectedTemplate;

        [ObservableProperty]
        private string _statusMessage = string.Empty;

        [ObservableProperty]
        private bool _isStatusVisible = false;

        public AddWorkoutViewModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task LoadTemplatesAsync()
        {
            await _dbContext.Database.EnsureCreatedAsync();

            var templateList = await _dbContext.WorkoutTemplates
                .OrderBy(t => t.Name)
                .ToListAsync();

            // ВАЖНО: Полностью пересоздаем коллекцию, чтобы Picker гарантированно обновился
            Templates = new ObservableCollection<WorkoutTemplate>(templateList);

            // Сбрасываем выбранный шаблон
            SelectedTemplate = null;
        }

        [RelayCommand]
        private async Task SaveWorkoutAsync()
        {
            var newWorkout = new Workout
            {
                Date = WorkoutDate,
                Name = string.IsNullOrWhiteSpace(WorkoutName) ? "Тренировка" : WorkoutName,
                Notes = "",
                TemplateId = SelectedTemplate?.Id
            };

            await _dbContext.Workouts.AddAsync(newWorkout);
            await _dbContext.SaveChangesAsync();

            // Если выбран шаблон, копируем упражнения из него в тренировку
            if (SelectedTemplate != null)
            {
                var templateExercises = await _dbContext.TemplateExercises
                    .Where(te => te.TemplateId == SelectedTemplate.Id)
                    .OrderBy(te => te.Order)
                    .ToListAsync();

                foreach (var templateExercise in templateExercises)
                {
                    var workoutExercise = new WorkoutExercise
                    {
                        WorkoutId = newWorkout.Id,
                        ExerciseId = templateExercise.ExerciseId,
                        Order = templateExercise.Order
                    };

                    await _dbContext.WorkoutExercises.AddAsync(workoutExercise);
                }

                await _dbContext.SaveChangesAsync();
            }

            StatusMessage = "Тренировка создана!";
            IsStatusVisible = true;

            await Task.Delay(1000);
            await Shell.Current.GoToAsync($"AddExerciseToWorkoutPage?workoutId={newWorkout.Id}");
        }

        [RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}