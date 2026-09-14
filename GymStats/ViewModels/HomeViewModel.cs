using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using GymStats.Models;
using Microsoft.EntityFrameworkCore;

namespace GymStats.ViewModels
{
    // partial нужен для работы CommunityToolkit.Mvvm (он сам генерирует код)
    public partial class HomeViewModel : ObservableObject
    {
        private readonly AppDbContext _dbContext;

        // Это поле будет автоматически обновлять текст на экране при изменении
        [ObservableProperty]
        private int _workoutCount;

        [ObservableProperty]
        private string _statusMessage = "Нажми кнопку для теста";

        // База данных сама "прилетит" сюда благодаря DI
        public HomeViewModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Метод для загрузки данных из БД
        public async Task InitializeAsync()
        {
            // ЭТА СТРОКА СОЗДАСТ ВСЕ ТАБЛИЦЫ В БАЗЕ ДАННЫХ ПРИ ПЕРВОМ ЗАПУСКЕ
            await _dbContext.Database.EnsureCreatedAsync();

            WorkoutCount = await _dbContext.Workouts.CountAsync();
        }

        // Команда, которая повиснет на кнопку
        [RelayCommand]
        private async Task AddTestWorkoutAsync()
        {
            // Создаем новую сущность
            var newWorkout = new Workout
            {
                Date = DateTime.Now,
                Notes = "Моя первая тестовая тренировка"
            };

            // Сохраняем в БД
            await _dbContext.Workouts.AddAsync(newWorkout);
            await _dbContext.SaveChangesAsync();

            // Обновляем счетчик и статус
            WorkoutCount = await _dbContext.Workouts.CountAsync();
            StatusMessage = $"Тренировка сохранена! Всего записей: {WorkoutCount}";
        }
    }
}