using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using GymStats.Models;

namespace GymStats.ViewModels
{
    public partial class AddWorkoutViewModel : ObservableObject
    {
        private readonly AppDbContext _dbContext;

        [ObservableProperty]
        private DateTime _workoutDate = DateTime.Today;

        [ObservableProperty]
        private string _notes = string.Empty;

        [ObservableProperty]
        private string _statusMessage = string.Empty;

        [ObservableProperty]
        private bool _isStatusVisible = false;

        public AddWorkoutViewModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [RelayCommand]
        private async Task SaveWorkoutAsync()
        {
            // Создаем новую тренировку
            var newWorkout = new Workout
            {
                Date = WorkoutDate,
                Notes = Notes,
                TemplateId = null // Пока без шаблона
            };

            // Сохраняем в БД
            await _dbContext.Workouts.AddAsync(newWorkout);
            await _dbContext.SaveChangesAsync();

            // Показываем сообщение об успехе
            StatusMessage = "Тренировка успешно сохранена!";
            IsStatusVisible = true;

            // Ждем немного и возвращаемся на главную
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