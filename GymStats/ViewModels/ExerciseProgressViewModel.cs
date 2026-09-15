using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymStats.Data;
using GymStats.Models;
using Microsoft.EntityFrameworkCore;
using Microcharts;
using SkiaSharp;
using System.Collections.ObjectModel;

namespace GymStats.ViewModels
{
    public partial class ExerciseProgressViewModel : ObservableObject
    {
        private readonly AppDbContext _dbContext;

        [ObservableProperty]
        private ObservableCollection<Exercise> _exercises = new();

        [ObservableProperty]
        private Exercise _selectedExercise;

        [ObservableProperty]
        private Chart _exerciseChart;

        [ObservableProperty]
        private bool _isChartVisible = false;

        [ObservableProperty]
        private string _chartTitle = "Выбери упражнение";

        public ExerciseProgressViewModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task LoadExercisesAsync()
        {
            await _dbContext.Database.EnsureCreatedAsync();
            var list = await _dbContext.Exercises.OrderBy(e => e.Name).ToListAsync();
            Exercises = new ObservableCollection<Exercise>(list);
        }

        partial void OnSelectedExerciseChanged(Exercise value)
        {
            if (value != null)
            {
                BuildChart(value);
            }
        }

        private async void BuildChart(Exercise exercise)
        {
            // Получаем все подходы по этому упражнению вместе с датами тренировок
            var sets = await _dbContext.Sets
                .Include(s => s.WorkoutExercise)
                .ThenInclude(we => we.Workout)
                .Where(s => s.WorkoutExercise.ExerciseId == exercise.Id)
                .OrderBy(s => s.WorkoutExercise.Workout.Date)
                .ToListAsync();

            if (sets.Count == 0)
            {
                ChartTitle = "Нет данных по этому упражнению";
                IsChartVisible = false;
                return;
            }

            // Группируем по дате тренировки и берем максимальный вес
            var groupedData = sets
                .GroupBy(s => s.WorkoutExercise.Workout.Date.Date)
                .Select(g => new { Date = g.Key, MaxWeight = g.Max(s => s.Weight) })
                .OrderBy(x => x.Date)
                .ToList();

            var entries = new List<ChartEntry>();
            foreach (var data in groupedData)
            {
                entries.Add(new ChartEntry((float)data.MaxWeight)
                {
                    Label = data.Date.ToString("dd.MM"),
                    ValueLabel = data.MaxWeight.ToString("F1"),
                    Color = SKColor.Parse("#2E8B57")
                });
            }

            ExerciseChart = new LineChart
            {
                Entries = entries,
                LineMode = LineMode.Straight,
                PointMode = PointMode.Circle,
                PointSize = 12,
                BackgroundColor = SKColors.Transparent,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                IsAnimated = true
            };

            ChartTitle = $"Прогресс: {exercise.Name}";
            IsChartVisible = true;
        }

        [RelayCommand]
        private async Task GoBackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}