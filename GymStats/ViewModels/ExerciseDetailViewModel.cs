using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using GymStats.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace GymStats.ViewModels
{
    [QueryProperty(nameof(WorkoutExerciseId), "workoutExerciseId")]
    public partial class ExerciseDetailViewModel : ObservableObject
    {
        private readonly AppDbContext _dbContext;

        [ObservableProperty]
        private int _workoutExerciseId;

        [ObservableProperty]
        private string _exerciseName = string.Empty;

        [ObservableProperty]
        private ObservableCollection<Set> _sets = new();

        [ObservableProperty]
        private bool _isListEmpty = true;

        public ExerciseDetailViewModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task LoadDataAsync()
        {
            await _dbContext.Database.EnsureCreatedAsync();

            // Загружаем название упражнения
            var workoutExercise = await _dbContext.WorkoutExercises
                .Include(we => we.Exercise)
                .FirstOrDefaultAsync(we => we.Id == WorkoutExerciseId);

            if (workoutExercise != null)
            {
                ExerciseName = workoutExercise.Exercise.Name;
            }

            // Загружаем подходы
            var sets = await _dbContext.Sets
                .Where(s => s.WorkoutExerciseId == WorkoutExerciseId)
                .OrderBy(s => s.Order)
                .ToListAsync();

            Sets.Clear();
            foreach (var set in sets)
            {
                Sets.Add(set);
            }

            IsListEmpty = Sets.Count == 0;
        }

        [RelayCommand]
        private async Task GoToAddSetAsync()
        {
            await Shell.Current.GoToAsync($"AddSetPage?workoutExerciseId={WorkoutExerciseId}");
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}