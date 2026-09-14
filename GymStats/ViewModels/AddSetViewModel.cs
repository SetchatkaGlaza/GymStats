using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using GymStats.Models;
using Microsoft.EntityFrameworkCore;

namespace GymStats.ViewModels
{
    [QueryProperty(nameof(WorkoutExerciseId), "workoutExerciseId")]
    public partial class AddSetViewModel : ObservableObject
    {
        private readonly AppDbContext _dbContext;

        [ObservableProperty]
        private int _workoutExerciseId;

        [ObservableProperty]
        private string _weight = "0";

        [ObservableProperty]
        private string _reps = "0";

        [ObservableProperty]
        private string _rpe = "7";

        [ObservableProperty]
        private string _tempo = string.Empty;

        [ObservableProperty]
        private string _statusMessage = string.Empty;

        [ObservableProperty]
        private bool _isStatusVisible = false;

        public AddSetViewModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [RelayCommand]
        private async Task SaveSetAsync()
        {
            if (!double.TryParse(Weight, out double weightValue) ||
                !int.TryParse(Reps, out int repsValue) ||
                !int.TryParse(Rpe, out int rpeValue))
            {
                StatusMessage = "Проверь правильность ввода данных";
                IsStatusVisible = true;
                return;
            }

            var maxOrder = await _dbContext.Sets
                .Where(s => s.WorkoutExerciseId == WorkoutExerciseId)
                .Select(s => (int?)s.Order)
                .MaxAsync() ?? 0;

            var newSet = new Set
            {
                WorkoutExerciseId = WorkoutExerciseId,
                Weight = weightValue,
                Reps = repsValue,
                RPE = rpeValue,
                Tempo = Tempo,
                Order = maxOrder + 1
            };

            await _dbContext.Sets.AddAsync(newSet);
            await _dbContext.SaveChangesAsync();

            StatusMessage = "Подход сохранен!";
            IsStatusVisible = true;

            await Task.Delay(1000);
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}