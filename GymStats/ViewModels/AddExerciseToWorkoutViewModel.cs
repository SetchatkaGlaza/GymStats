using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using GymStats.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace GymStats.ViewModels
{
    // Атрибут для получения workoutId из URL навигации
    [QueryProperty(nameof(WorkoutId), "workoutId")]
    public partial class AddExerciseToWorkoutViewModel : ObservableObject
    {
        private readonly AppDbContext _dbContext;

        // WorkoutId теперь свойство, а не параметр конструктора
        [ObservableProperty]
        private int _workoutId;

        [ObservableProperty]
        private ObservableCollection<WorkoutExercise> _workoutExercises = new();

        [ObservableProperty]
        private bool _isListEmpty = true;

        [ObservableProperty]
        private string _emptyMessage = "Добавь упражнения для тренировки";

        // В конструкторе только AppDbContext (его умеет создавать DI)
        public AddExerciseToWorkoutViewModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task LoadExercisesAsync()
        {
            await _dbContext.Database.EnsureCreatedAsync();

            var exercises = await _dbContext.WorkoutExercises
                .Where(we => we.WorkoutId == WorkoutId)
                .Include(we => we.Exercise)
                .OrderBy(we => we.Order)
                .ToListAsync();

            WorkoutExercises.Clear();
            foreach (var exercise in exercises)
            {
                WorkoutExercises.Add(exercise);
            }

            IsListEmpty = WorkoutExercises.Count == 0;
        }

        [RelayCommand]
        private async Task GoToAddExerciseAsync()
        {
            await Shell.Current.GoToAsync($"SelectExercisePage?workoutId={WorkoutId}");
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task OpenExerciseDetailAsync(WorkoutExercise workoutExercise)
        {
            await Shell.Current.GoToAsync($"ExerciseDetailPage?workoutExerciseId={workoutExercise.Id}");
        }
    }
}