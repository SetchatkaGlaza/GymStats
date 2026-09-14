using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using GymStats.Models;
using Microsoft.EntityFrameworkCore;

namespace GymStats.ViewModels
{
    [QueryProperty(nameof(WorkoutId), "workoutId")]
    public partial class FinishWorkoutViewModel : ObservableObject
    {
        private readonly AppDbContext _dbContext;

        [ObservableProperty]
        private int _workoutId;

        [ObservableProperty]
        private string _workoutName = "Тренировка";

        [ObservableProperty]
        private string _notes = string.Empty;

        [ObservableProperty]
        private string _statusMessage = string.Empty;

        [ObservableProperty]
        private bool _isStatusVisible = false;

        public FinishWorkoutViewModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task LoadWorkoutDataAsync()
        {
            var workout = await _dbContext.Workouts.FindAsync(WorkoutId);
            if (workout != null)
            {
                WorkoutName = workout.Name;
                Notes = workout.Notes;
            }
        }

        [RelayCommand]
        private async Task SaveAndFinishAsync()
        {
            var workout = await _dbContext.Workouts.FindAsync(WorkoutId);
            if (workout != null)
            {
                workout.Name = string.IsNullOrWhiteSpace(WorkoutName) ? "Тренировка" : WorkoutName;
                workout.Notes = Notes;

                await _dbContext.SaveChangesAsync();

                StatusMessage = "Тренировка успешно завершена и сохранена!";
                IsStatusVisible = true;

                await Task.Delay(1000);

                // Возвращаемся на главный экран, очищая стек навигации
                await Shell.Current.GoToAsync("//MainPage");
            }
        }
    }
}