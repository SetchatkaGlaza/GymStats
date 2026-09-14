using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using GymStats.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace GymStats.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly AppDbContext _dbContext;

        [ObservableProperty]
        private ObservableCollection<Workout> _workouts = new();

        [ObservableProperty]
        private string _emptyMessage = "Пока нет тренировок. Добавь первую!";

        [ObservableProperty]
        private bool _isListEmpty = true;

        public MainPageViewModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task LoadWorkoutsAsync()
        {
            await _dbContext.SeedDataAsync(); // Гарантируем заполнение базы

            var workoutList = await _dbContext.Workouts
                .OrderByDescending(w => w.Date)
                .ToListAsync();

            Workouts.Clear();
            foreach (var workout in workoutList)
            {
                Workouts.Add(workout);
            }

            IsListEmpty = Workouts.Count == 0;
        }

        [RelayCommand]
        private async Task GoToAddWorkoutAsync()
        {
            await Shell.Current.GoToAsync("AddWorkoutPage");
        }

        [RelayCommand]
        private async Task OpenWorkoutAsync(Workout workout)
        {
            await Shell.Current.GoToAsync($"AddExerciseToWorkoutPage?workoutId={workout.Id}");
        }

        [RelayCommand]
        private async Task GoToTemplatesAsync()
        {
            await Shell.Current.GoToAsync("TemplateListPage");
        }
        [RelayCommand]
        private async Task GoToAnthropometryAsync()
        {
            await Shell.Current.GoToAsync("AnthropometryPage");
        }
    }
}